using EExpansions.AspNetCore.Session;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions
/// </summary>
public static class ESessionServiceCollectionExtensions
{
    /// <summary>
    /// Adds services required for application session state.
    /// </summary>
    /// <typeparam name="TDistributedCache">The type of distributed cache.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> is <see langword="null" />.
    /// </exception>
    public static IServiceCollection AddSession<TDistributedCache>(this IServiceCollection services)
        where TDistributedCache : IDistributedCache
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.TryAddTransient<ISessionStore, DistributedSessionStore<TDistributedCache>>();
        services.AddDataProtection();
        return services;
    }

    /// <summary>
    /// Adds services required for application session state.
    /// </summary>
    /// <typeparam name="TDistributedCache">The type of distributed cache.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configure">The session options to configure the middleware with.</param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> or <paramref name="configure"/> are <see langword="null" />.
    /// </exception>
    public static IServiceCollection AddSession<TDistributedCache>(
        this IServiceCollection services,
        Action<SessionOptions> configure
    ) where TDistributedCache : IDistributedCache
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));
        _ = configure ?? throw new ArgumentNullException(nameof(configure));

        services.Configure(configure);
        services.AddSession<TDistributedCache>();

        return services;
    }
}
