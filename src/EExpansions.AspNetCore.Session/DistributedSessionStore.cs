namespace EExpansions.AspNetCore.Session;

public class DistributedSessionStore<TDistributedCache> : DistributedSessionStore
    where TDistributedCache : IDistributedCache
{
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
