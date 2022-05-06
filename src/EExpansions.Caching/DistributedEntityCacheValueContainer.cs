namespace EExpansions.AspNetCore.Caching;

/// <summary>
/// The service to provides value container for <see cref="IEntityCache"/>
/// </summary>
/// <typeparam name="TDistributedCache">The type of the value storage.</typeparam>
public class DistributedEntityCacheValueContainer<TDistributedCache> : IEntityCacheValueContainer
    where TDistributedCache : IDistributedCache
{
    private TDistributedCache Cache { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DistributedEntityCacheValueContainer{TDistributedCache}" /> class.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> to get <typeparamref name="TDistributedCache"/>.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DistributedEntityCacheValueContainer(IServiceProvider serviceProvider)
    {
        _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        Cache =
            serviceProvider
            .GetServices<IDistributedCache>()
            .OfType<TDistributedCache>()
            .First();
    }

    /// <inheritdoc/>
    public TEntity? Get<TEntity>(string key) where TEntity : class =>
        Cache.Get<TEntity>(key);

    /// <inheritdoc/>
    public object? Get(Type entityType, string key)
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));

        return Cache.Get(entityType, key);
    }

    /// <inheritdoc/>
    public Task<TEntity?> GetAsync<TEntity>(
        string key,
        CancellationToken cancellationToken = default
    ) where TEntity : class =>
        Cache.GetAsync<TEntity>(key);

    /// <inheritdoc/>
    public Task<object?> GetAsync(
        Type entityType,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));

        return Cache.GetAsync(entityType, key, cancellationToken);
    }

    /// <inheritdoc/>
    public void Set<TEntity>(string key, TEntity value) where TEntity : class =>
        Cache.Set(key, value);

    public void Set(Type entityType, string key, object value)
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));

        Cache.Set(entityType, key, value);
    }

    /// <inheritdoc/>
    public Task SetAsync<TEntity>(
        string key,
        TEntity value,
        CancellationToken cancellationToken = default
    ) where TEntity : class =>
        Cache.SetAsync(key, value, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public Task SetAsync(
        Type entityType,
        string key,
        object value,
        CancellationToken cancellationToken = default
    )
    {
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));

        return Cache.SetAsync(entityType, key, value, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public void Remove(string key) => Cache.Remove(key);

    /// <inheritdoc/>
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        Cache.RemoveAsync(key, cancellationToken);
}
