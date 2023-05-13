namespace EExpansions.AspNetCore.Session;

/// <summary>
/// Extensions
/// </summary>
public static class SessionExtensions
{
    /// <summary>
    /// Gets a value from <see cref="ISession"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="session">The <see cref="ISession"/>.</param>
    /// <param name="key">The key to load.</param>
    /// <returns>The value loaded, or <see langword="null" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="session"/> is <see langword="null" />.
    /// </exception>
    public static TValue? Get<TValue>(this ISession session, string key)
    {
        _ = session ?? throw new ArgumentNullException(nameof(session));

        var json = session.GetString(key);
        return json.IsNullOrEmpty() ? default : JsonSerializer.Deserialize<TValue>(json);
    }

    /// <summary>
    /// Gets a value from <see cref="ISession"/>.
    /// </summary>
    /// <param name="session">The <see cref="ISession"/>.</param>
    /// <param name="valueType">The type of the value.</param>
    /// <param name="key">The key to load.</param>
    /// <returns>The value loaded, or <see langword="null" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="session"/> or <paramref name="valueType"/> are <see langword="null" />.
    /// </exception>
    public static object? Get(this ISession session, Type valueType, string key)
    {
        _ = session ?? throw new ArgumentNullException(nameof(session));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));

        var json = session.GetString(key);
        return json.IsNullOrEmpty() ? default : JsonSerializer.Deserialize(json, valueType);
    }

    /// <summary>
    /// Sets a value in the <see cref="ISession"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="session">The <see cref="ISession"/>.</param>
    /// <param name="key">The key to assign.</param>
    /// <param name="value">The value to assign.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="session"/> is <see langword="null" />.
    /// </exception>
    public static void Set<TValue>(this ISession session, string key, TValue value)
    {
        _ = session ?? throw new ArgumentNullException(nameof(session));

        var json = JsonSerializer.Serialize(value);
        session.SetString(key, json);
    }

    /// <summary>
    /// Sets a value in the <see cref="ISession"/>.
    /// </summary>
    /// <param name="session">The <see cref="ISession"/>.</param>
    /// <param name="valueType">The type of the value.</param>
    /// <param name="key">The key to assign.</param>
    /// <param name="value">The value to assign.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="session"/> or <paramref name="valueType"/> are <see langword="null" />.
    /// </exception>
    public static void Set(this ISession session, Type valueType, string key, object value)
    {
        _ = session ?? throw new ArgumentNullException(nameof(session));
        _ = valueType ?? throw new ArgumentNullException(nameof(valueType));

        var json = JsonSerializer.Serialize(value, valueType);
        session.SetString(key, json);
    }
}
