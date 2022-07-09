namespace EExpansions.AspNetCore.MultiTenancy;

/// <summary>
/// The exception that throws when an entity with an unexpected tenant ID is refered.
/// </summary>
public class AcrossTenantsException : Exception
{
    public AcrossTenantsException(string? expected, string? actual)
        : base($"expected tenant ID:{expected}, actual tenant ID: {actual}")
    { }
}
