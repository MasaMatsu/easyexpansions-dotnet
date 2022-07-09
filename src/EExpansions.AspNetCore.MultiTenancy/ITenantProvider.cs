namespace EExpansions.AspNetCore.MultiTenancy;

/// <summary>
/// The provider that provides tenant information.
/// </summary>
public interface ITenantProvider
{
    /// <summary>
    /// Get tenant ID.
    /// </summary>
    /// <returns>The tenant ID.</returns>
    Guid GetTenantId();

    /// <summary>
    /// Get connection string of tenant store.
    /// </summary>
    /// <returns>The connection string.</returns>
    string GetConnectionString();
}
