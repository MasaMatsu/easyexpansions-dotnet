using System.Reflection;

namespace EExpansions;

public static class CustomAttributeProviderExtensions
{
    /// <summary>
    /// Returns a sequence of custom attributes defined on this member, or an empty array if there are no custom attributes of that type.
    /// </summary>
    /// <param name="provider"><see cref="ICustomAttributeProvider"/> value.</param>
    /// <param name="inherit">When <see langword="true" />, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>
    /// A sequence of custom attributes, or an empty array.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="provider" /> is <see langword="null" />.
    /// </exception>
    public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this ICustomAttributeProvider provider, bool inherit)
        where TAttribute : Attribute
    {
        _ = provider ?? throw new ArgumentNullException(nameof(provider));

        return provider.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();
    }

    /// <summary>
    /// Returns a first <typeparamref name="TAttribute"/> defined on this member, or <see langword="null"/> if there are no custom attributes of that type.
    /// </summary>
    /// <param name="provider"><see cref="ICustomAttributeProvider"/> value.</param>
    /// <param name="inherit">When <see langword="true" />, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>
    /// A first <typeparamref name="TAttribute"/>, or <see langword="null" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="provider" /> is <see langword="null" />.
    /// </exception>
    public static TAttribute? GetCustomAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit)
        where TAttribute : Attribute
    {
        _ = provider ?? throw new ArgumentNullException(nameof(provider));

        return provider.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
    }
}
