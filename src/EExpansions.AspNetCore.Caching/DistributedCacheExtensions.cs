namespace EExpansions.AspNetCore.Caching;

/// <summary>
/// Extensions
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Gets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <typeparam name="TValue">The type of the returned value.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <returns>The value from the stored cache key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static TValue? Get<TValue>(this IDistributedCache cache, string key)
        where TValue : class
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = cache.GetString(key);
        if (json.IsNullOrEmpty())
        {
            return null;
        }

        return JsonSerializer.Deserialize<TValue>(json);
    }

    /// <summary>
    /// Gets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="valueType">The type of the returned value.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <returns>The value from the stored cache key.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/>, <paramref name="valueType"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static object? Get(this IDistributedCache cache, Type valueType, string key)
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = cache.GetString(key);
        if (json.IsNullOrEmpty())
        {
            return null;
        }

        return JsonSerializer.Deserialize(json, valueType);
    }

    /// <summary>
    /// Gets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <typeparam name="TValue">The type of the returned value.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the value from the stored cache key.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static async Task<TValue?> GetAsync<TValue>(
        this IDistributedCache cache,
        string key,
        CancellationToken cancellationToken = default
    ) where TValue : class
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = await cache.GetStringAsync(key, cancellationToken);
        if (json.IsNullOrEmpty())
        {
            return null;
        }

        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        return await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="valueType">The type of the returned value.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the value from the stored cache key.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/>, <paramref name="valueType"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static async Task<object?> GetAsync(
        this IDistributedCache cache,
        Type valueType,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = await cache.GetStringAsync(key, cancellationToken);
        if (json.IsNullOrEmpty())
        {
            return null;
        }

        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        return await JsonSerializer.DeserializeAsync(stream, valueType, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Sets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <typeparam name="TValue">The type of the returned value.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static void Set<TValue>(
        this IDistributedCache cache,
        string key,
        TValue value,
        DistributedCacheEntryOptions? options = null
    ) where TValue : class
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = JsonSerializer.Serialize(value);
        if (options is null)
        {
            cache.SetString(key, json);
        }
        else
        {
            cache.SetString(key, json, options);
        }
    }

    /// <summary>
    /// Sets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="valueType">The type of the returned value.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/>, <paramref name="valueType"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static void Set(
        this IDistributedCache cache,
        Type valueType,
        string key,
        object value,
        DistributedCacheEntryOptions? options = null
    )
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        var json = JsonSerializer.Serialize(value, valueType);
        if (options is null)
        {
            cache.SetString(key, json);
        }
        else
        {
            cache.SetString(key, json, options);
        }
    }

    /// <summary>
    /// Sets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <typeparam name="TValue">The type of the returned value.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static async Task SetAsync<TValue>(
        this IDistributedCache cache,
        string key,
        TValue value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default
    ) where TValue : class
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        await using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, value, cancellationToken: cancellationToken);
        var json = Encoding.UTF8.GetString(stream.ToArray());

        if (options is null)
        {
            await cache.SetStringAsync(key, json, cancellationToken);
        }
        else
        {
            await cache.SetStringAsync(key, json, options, cancellationToken);
        }
    }

    /// <summary>
    /// Sets a value with the given key.
    /// The value are stored in cache in JSON format.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="valueType">The type of the returned value.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="cache"/>, <paramref name="valueType"/> or <paramref name="key"/> are <see langword="null" />.
    /// </exception>
    public static async Task SetAsync(
        this IDistributedCache cache,
        Type valueType,
        string key,
        object value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        _ = cache ?? throw new ArgumentNullException(nameof(cache));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));
        _ = key ?? throw new ArgumentNullException(nameof(key));

        await using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, value, cancellationToken: cancellationToken);
        var json = Encoding.UTF8.GetString(stream.ToArray());

        if (options is null)
        {
            await cache.SetStringAsync(key, json, cancellationToken);
        }
        else
        {
            await cache.SetStringAsync(key, json, options, cancellationToken);
        }
    }
}
