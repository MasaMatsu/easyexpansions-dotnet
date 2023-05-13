namespace EExpansions.AspNetCore.Caching;

/// <summary>
/// The service that provides caching logic of efcore.
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
/// <typeparam name="TKeyContainer">The type of the key container.</typeparam>
public class EntityCache<TContext, TKeyContainer> : IEntityCache
    where TContext : DbContext
    where TKeyContainer : IDistributedCache
{
    private TKeyContainer KeyContainer { get; }

    private IEntityCacheValueContainer ValueContainer { get; }

    private IDbContextFactory<TContext> ContextFactory { get; }

    private EntityCacheOptions Options { get; }

    private SemaphoreSlim SemaphoreSlim { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityCache{TContext, TKeyContainer}" /> class.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to get services.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="serviceProvider"/> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The key prefix may not be empty.
    /// </exception>
    public EntityCache(IServiceProvider serviceProvider)
    {
        _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        KeyContainer =
            serviceProvider
            .GetServices<IDistributedCache>()
            .OfType<TKeyContainer>()
            .First();
        ValueContainer = serviceProvider.GetRequiredService<IEntityCacheValueContainer>();
        ContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();

        Options = serviceProvider.GetRequiredService<IOptions<EntityCacheOptions>>().Value;
        if (Options.KeyPrefix.IsNullOrEmpty())
        {
            throw new InvalidOperationException("The key prefix may not be empty.");
        }

        SemaphoreSlim = new SemaphoreSlim(1, 1);
    }

    /// <inheritdoc/>
    public TEntity? Get<TEntity>(params object?[]? keyValues) where TEntity : class =>
        keyValues is null || keyValues.Any(v => v is null)
        ? null
        : GetOrCreateCachedEntity<TEntity>(keyValues);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public object? Get(Type entityType, params object?[]? keyValues)
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        return
            keyValues is null || keyValues.Any(v => v is null)
            ? null
            : GetOrCreateCachedEntity(entityType, keyValues);
    }

    /// <inheritdoc/>
    public Task<TEntity?> GetAsync<TEntity>(params object?[]? keyValues) where TEntity : class =>
        keyValues is null || keyValues.Any(v => v is null)
        ? Task.FromResult<TEntity?>(null)
        : GetOrCreateCachedEntityAsync<TEntity>(keyValues, default);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public Task<object?> GetAsync(Type entityType, params object?[]? keyValues)
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        return
            keyValues is null || keyValues.Any(v => v is null)
            ? Task.FromResult<object?>(null)
            : GetOrCreateCachedEntityAsync(entityType, keyValues, default);
    }

    /// <inheritdoc/>
    public Task<TEntity?> GetAsync<TEntity>(
        object?[]? keyValues,
        CancellationToken cancellationToken
    ) where TEntity : class =>
        keyValues is null || keyValues.Any(v => v is null)
        ? Task.FromResult<TEntity?>(null)
        : GetOrCreateCachedEntityAsync<TEntity>(keyValues, cancellationToken);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public Task<object?> GetAsync(
        Type entityType,
        object?[]? keyValues,
        CancellationToken cancellationToken
    )
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        return
            keyValues is null || keyValues.Any(v => v is null)
            ? Task.FromResult<object?>(null)
            : GetOrCreateCachedEntityAsync(entityType, keyValues, cancellationToken);
    }

    /// <inheritdoc/>
    public void Expire<TEntity>(params object?[]? keyValues) where TEntity : class
    {
        if (keyValues is null || keyValues.Any(v => v is null))
        {
            return;
        }
        SemaphoreSlim.ExecuteInLock(() =>
        {
            var keyCacheKey = GenKeyCacheKey(typeof(TEntity), keyValues);
            var valueCacheKey = GenValueCacheKey(keyCacheKey);
            KeyContainer.Remove(keyCacheKey);
            ValueContainer.Remove(valueCacheKey);
        }, Options.SemaphoreTimeout);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public void Expire(Type entityType, params object?[]? keyValues)
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        if (keyValues is null || keyValues.Any(v => v is null))
        {
            return;
        }
        SemaphoreSlim.ExecuteInLock(() =>
        {
            var keyCacheKey = GenKeyCacheKey(entityType, keyValues);
            var valueCacheKey = GenValueCacheKey(keyCacheKey);
            KeyContainer.Remove(keyCacheKey);
            ValueContainer.Remove(valueCacheKey);
        }, Options.SemaphoreTimeout);
    }

    /// <inheritdoc/>
    public Task ExpireAsync<TEntity>(params object?[]? keyValues) where TEntity : class =>
        ExpireAsync<TEntity>(keyValues, cancellationToken: default);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public Task ExpireAsync(Type entityType, params object?[]? keyValues) =>
        ExpireAsync(entityType, keyValues, cancellationToken: default);

    /// <inheritdoc/>
    public async Task ExpireAsync<TEntity>(
        object?[]? keyValues,
        CancellationToken cancellationToken
    ) where TEntity : class
    {
        if (keyValues is null || keyValues.Any(v => v is null))
        {
            return;
        }
        await SemaphoreSlim.ExecuteInLockAsync(async () =>
        {
            var keyCacheKey = GenKeyCacheKey(typeof(TEntity), keyValues);
            var valueCacheKey = GenValueCacheKey(keyCacheKey);
            await KeyContainer.RemoveAsync(keyCacheKey, cancellationToken);
            await ValueContainer.RemoveAsync(valueCacheKey, cancellationToken);
        }, Options.SemaphoreTimeout, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="entityType"/> is <see langword="null" />.
    /// </exception>
    public async Task ExpireAsync(
        Type entityType,
        object?[]? keyValues,
        CancellationToken cancellationToken
    )
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        if (keyValues is null || keyValues.Any(v => v is null))
        {
            return;
        }
        await SemaphoreSlim.ExecuteInLockAsync(async () =>
        {
            var keyCacheKey = GenKeyCacheKey(entityType, keyValues);
            var valueCacheKey = GenValueCacheKey(keyCacheKey);
            await KeyContainer.RemoveAsync(keyCacheKey, cancellationToken);
            await ValueContainer.RemoveAsync(valueCacheKey, cancellationToken);
        }, Options.SemaphoreTimeout, cancellationToken: cancellationToken);
    }

    #region Key generation logics

    private string GetPrimaryKeyValueString(object?[] keyValues) =>
        $"{{{string.Join(",", keyValues)}}}";

    private string GenKeyCacheKey(Type entityType, object?[] keyValues) =>
        $"{Options.KeyPrefix}:{entityType.Name}:pk:{GetPrimaryKeyValueString(keyValues)}";

    private string GenValueCacheKey(string keyCacheKey) =>
        $"{keyCacheKey}:value";

    #endregion

    #region Get entity in cache

    private TEntity? GetEntityInCache<TEntity>(string keyCacheKey) where TEntity : class
    {
        var valueCacheKey = KeyContainer.GetString(keyCacheKey);
        if (valueCacheKey.IsNullOrEmpty())
        {
            return null;
        }
        return ValueContainer.Get<TEntity>(valueCacheKey);
    }

    private object? GetEntityInCache(Type entityType, string keyCacheKey)
    {
        var valueCacheKey = KeyContainer.GetString(keyCacheKey);
        if (valueCacheKey.IsNullOrEmpty())
        {
            return null;
        }
        return ValueContainer.Get(entityType, valueCacheKey);
    }

    private async Task<TEntity?> GetEntityInCacheAsync<TEntity>(
        string keyCacheKey,
        CancellationToken cancellationToken
    ) where TEntity : class
    {
        var valueCacheKey = await KeyContainer.GetStringAsync(keyCacheKey, cancellationToken);
        if (valueCacheKey.IsNullOrEmpty())
        {
            return null;
        }
        return await ValueContainer.GetAsync<TEntity>(valueCacheKey, cancellationToken);
    }

    private async Task<object?> GetEntityInCacheAsync(
        Type entityType,
        string keyCacheKey,
        CancellationToken cancellationToken
    )
    {
        var valueCacheKey = await KeyContainer.GetStringAsync(keyCacheKey, cancellationToken);
        if (valueCacheKey.IsNullOrEmpty())
        {
            return null;
        }
        return await ValueContainer.GetAsync(entityType, valueCacheKey, cancellationToken);
    }

    #endregion

    #region Get or create entity in cache

    private TEntity? GetOrCreateCachedEntity<TEntity>(object?[] keyValues) where TEntity : class
    {
        var keyCacheKey = GenKeyCacheKey(typeof(TEntity), keyValues);

        var cachedEntity = GetEntityInCache<TEntity>(keyCacheKey);

        if (cachedEntity is null)
        {
            SemaphoreSlim.ExecuteInLock(() =>
            {
                cachedEntity = GetEntityInCache<TEntity>(keyCacheKey);
                if (cachedEntity is null)
                {
                    var entity = ContextFactory.Execute(context =>
                    {
                        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        return context.Find<TEntity>(keyValues);
                    });
                    if (entity is not null)
                    {
                        var valueCacheKey = GenValueCacheKey(keyCacheKey);
                        KeyContainer.SetString(
                            keyCacheKey,
                            valueCacheKey,
                            Options.DistributedCacheEntryOptions
                        );
                        ValueContainer.Set(valueCacheKey, entity);
                        cachedEntity = entity;
                    }
                }
            }, Options.SemaphoreTimeout);
        }

        return cachedEntity;
    }

    private object? GetOrCreateCachedEntity(Type entityType, object?[] keyValues)
    {
        var keyCacheKey = GenKeyCacheKey(entityType, keyValues);

        var cachedEntity = GetEntityInCache(entityType, keyCacheKey);

        if (cachedEntity is null)
        {
            SemaphoreSlim.ExecuteInLock(() =>
            {
                cachedEntity = GetEntityInCache(entityType, keyCacheKey);
                if (cachedEntity is null)
                {
                    var entity = ContextFactory.Execute(context =>
                    {
                        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        return context.Find(entityType, keyValues);
                    });
                    if (entity is not null)
                    {
                        var valueCacheKey = GenValueCacheKey(keyCacheKey);
                        KeyContainer.SetString(
                            keyCacheKey,
                            valueCacheKey,
                            Options.DistributedCacheEntryOptions
                        );
                        ValueContainer.Set(entityType, valueCacheKey, entity);
                        cachedEntity = entity;
                    }
                }
            }, Options.SemaphoreTimeout);
        }

        return cachedEntity;
    }

    private async Task<TEntity?> GetOrCreateCachedEntityAsync<TEntity>(
        object?[] keyValues,
        CancellationToken cancellationToken
    ) where TEntity : class
    {
        var keyCacheKey = GenKeyCacheKey(typeof(TEntity), keyValues);

        var cachedEntity = await GetEntityInCacheAsync<TEntity>(keyCacheKey, cancellationToken);

        if (cachedEntity is null)
        {
            await SemaphoreSlim.ExecuteInLockAsync(async () =>
            {
                cachedEntity = await GetEntityInCacheAsync<TEntity>(keyCacheKey, cancellationToken);
                if (cachedEntity is null)
                {
                    var entity = await ContextFactory.ExecuteAsync(
                        context =>
                        {
                            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                            return context.FindAsync<TEntity>(keyValues, cancellationToken).AsTask();
                        },
                        cancellationToken
                    );
                    if (entity is not null)
                    {
                        var valueCacheKey = GenValueCacheKey(keyCacheKey);
                        await KeyContainer.SetStringAsync(
                            keyCacheKey,
                            valueCacheKey,
                            Options.DistributedCacheEntryOptions,
                            cancellationToken
                        );
                        await ValueContainer.SetAsync(valueCacheKey, entity, cancellationToken);
                        cachedEntity = entity;
                    }
                }
            }, Options.SemaphoreTimeout, cancellationToken);
        }

        return cachedEntity;
    }

    private async Task<object?> GetOrCreateCachedEntityAsync(
        Type entityType,
        object?[] keyValues,
        CancellationToken cancellationToken
    )
    {
        var keyCacheKey = GenKeyCacheKey(entityType, keyValues);

        var cachedEntity = await GetEntityInCacheAsync(entityType, keyCacheKey, cancellationToken);

        if (cachedEntity is null)
        {
            await SemaphoreSlim.ExecuteInLockAsync(async () =>
            {
                cachedEntity = await GetEntityInCacheAsync(entityType, keyCacheKey, cancellationToken);
                if (cachedEntity is null)
                {
                    var entity = await ContextFactory.ExecuteAsync(
                        context =>
                        {
                            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                            return context.FindAsync(entityType, keyValues, cancellationToken).AsTask();
                        },
                        cancellationToken
                    );
                    if (entity is not null)
                    {
                        var valueCacheKey = GenValueCacheKey(keyCacheKey);
                        await KeyContainer.SetStringAsync(
                            keyCacheKey,
                            valueCacheKey,
                            Options.DistributedCacheEntryOptions,
                            cancellationToken
                        );
                        await ValueContainer.SetAsync(valueCacheKey, entity, cancellationToken);
                        cachedEntity = entity;
                    }
                }
            }, Options.SemaphoreTimeout, cancellationToken);
        }

        return cachedEntity;
    }

    #endregion
}
