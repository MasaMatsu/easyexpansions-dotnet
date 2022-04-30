namespace EExpansions.Caching;

/// <summary>
/// The service that provides caching logic of efcore.
/// </summary>
public interface IEntityCache
{
    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// The entity found, or <see langword="null" />.
    /// </returns>
    TEntity? Get<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// The entity found, or <see langword="null" />.
    /// </returns>
    object? Get(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<TEntity?> GetAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<object?> GetAsync(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<TEntity?> GetAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class;

    /// <summary>
    /// Finds an entity with the given primary key values from the cache.
    /// If an entity with the given primary key values is not found,
    /// then a query is made to the database for an entity with the given primary key values and this entity,
    /// if found, is set to the cache and returned.
    /// If no entity is found, then <see langword="null"/> is returned.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the entity found, or <see langword="null" />.
    /// </returns>
    Task<object?> GetAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken);

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    void Expire<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    void Expire(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task ExpireAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task ExpireAsync(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task ExpireAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class;

    /// <summary>
    /// Expires an entity with the given primary key values from the cache.
    /// </summary>
    /// <param name="entityType">The type of the entity.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task ExpireAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken);
}
