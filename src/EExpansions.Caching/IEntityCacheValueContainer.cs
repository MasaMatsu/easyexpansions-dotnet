namespace EExpansions.Caching;

/// <summary>
/// The service to provides value container for <see cref="IEntityCache"/>
/// </summary>
public interface IEntityCacheValueContainer
{
    /// <summary>
    /// Gets an entity with the given key.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="key">A string key the requested value.</param>
    /// <returns>
    /// The entity found, or <see langword="null" />.
    /// </returns>
    TEntity? Get<TEntity>(string key)
        where TEntity : class;

    /// <summary>
    /// Gets an entity with the given key.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="key">A string key the requested value.</param>
    /// <returns>
    /// The entity found, or <see langword="null" />.
    /// </returns>
    object? Get(Type entityType, string key);

    /// <summary>
    /// Gets an entity with the given key.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken = default)
        where TEntity : class;

    /// <summary>
    /// Gets an entity with the given key.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<object?> GetAsync(Type entityType, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the entity with the given key.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="value">The entity to set in the cache.</param>
    void Set<TEntity>(string key, TEntity value)
        where TEntity : class;

    /// <summary>
    /// Sets the entity with the given key.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="value">The entity to set in the cache.</param>
    void Set(Type entityType, string key, object value);

    /// <summary>
    /// Sets the entity with the given key.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="value">The entity to set in the cache.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task SetAsync<TEntity>(string key, TEntity value, CancellationToken cancellationToken = default)
        where TEntity : class;

    /// <summary>
    /// Sets the entity with the given key.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="key">A string key the requested value.</param>
    /// <param name="value">The entity to set in the cache.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task SetAsync(Type entityType, string key, object value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the entity with the given key.
    /// </summary>
    /// <param name="key">A string key the requested value.</param>
    void Remove(string key);

    /// <summary>
    /// Removes the entity with the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
