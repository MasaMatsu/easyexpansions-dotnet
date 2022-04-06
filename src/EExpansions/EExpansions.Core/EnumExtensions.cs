using System.ComponentModel.DataAnnotations;

namespace EExpansions.Core;

public static class EnumExtensions
{
    /// <summary>
    /// Get a value that is used for display in the UI.
    /// </summary>
    /// <param name="enumValue">enum value</param>
    /// <returns>A value that is used for display in the UI.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="enumValue" /> is <see langword="null" />.
    /// </exception>
    public static string? GetDisplayName(this Enum enumValue)
    {
        _ = enumValue ?? throw new ArgumentNullException(nameof(enumValue));

        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>(true);
        return attribute?.Name;
    }
}
