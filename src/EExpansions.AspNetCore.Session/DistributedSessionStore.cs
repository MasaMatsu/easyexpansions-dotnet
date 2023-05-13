namespace EExpansions.AspNetCore.Session;

/// <summary>
/// Generic DistributedSessionStore.
/// </summary>
/// <typeparam name="TDistributedCache">The type of IDistributedCache</typeparam>
public class DistributedSessionStore<TDistributedCache> : DistributedSessionStore
    where TDistributedCache : IDistributedCache
{
    /// <summary>
    /// Generic DistributedSessionStore.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DistributedSessionStore(IServiceProvider serviceProvider)
        : base(
            serviceProvider
                .GetServices<IDistributedCache>()
                .OfType<TDistributedCache>()
                .First(),
            serviceProvider.GetRequiredService<ILoggerFactory>()
        )
    {
        _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
}
