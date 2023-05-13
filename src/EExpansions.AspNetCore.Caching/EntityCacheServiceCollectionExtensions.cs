namespace EExpansions.AspNetCore.Caching;

/// <summary>
/// Extensions
/// </summary>
public static class EntityCacheServiceCollectionExtensions
{
    /// <summary>
    /// Adds Entity services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TKeyContainer">The type of the cache key container.</typeparam>
    /// <typeparam name="TValueContainer">The type of the cache value container.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configure">
    /// An <see cref="Action{T}"/> to configure the provided <see cref="EntityCacheOptions"/>.
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> or <paramref name="configure"/> are <see langword="null" />.
    /// </exception>
    public static IServiceCollection AddEntityCache<TContext, TKeyContainer, TValueContainer>(
        this IServiceCollection services,
        Action<EntityCacheOptions> configure
    )
        where TContext : DbContext
        where TKeyContainer : IDistributedCache
        where TValueContainer : class, IEntityCacheValueContainer
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));
        _ = configure ?? throw new ArgumentNullException(nameof(configure));

        services.Configure(configure);
        services.TryAddSingleton<IEntityCacheValueContainer, TValueContainer>();
        services.TryAddSingleton<IEntityCache, EntityCache<TContext, TKeyContainer>>();

        return services;
    }
}
