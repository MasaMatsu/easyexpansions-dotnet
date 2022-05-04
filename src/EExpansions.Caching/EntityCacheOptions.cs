namespace EExpansions.Caching;

/// <summary>
/// The options to be used by a <see cref="EntityCache{TContext}" />.
/// </summary>
public class EntityCacheOptions
{
    /// <summary>
    /// The prefix literal of cache key.
    /// </summary>
    public string KeyPrefix { get; set; } = string.Empty;

    /// <summary>
    /// The cache options for an entry in <see cref="IDistributedCache"/>.
    /// </summary>
    public DistributedCacheEntryOptions? DistributedCacheEntryOptions { get; set; } =
        new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromHours(24))
        .SetSlidingExpiration(TimeSpan.FromHours(6));

    /// <summary>
    /// A <see cref="TimeSpan"/> that called from <see cref="SemaphoreSlim.Wait(TimeSpan)"/>
    /// </summary>
    public TimeSpan SemaphoreTimeout { get; set; } = Timeout.InfiniteTimeSpan;
}
