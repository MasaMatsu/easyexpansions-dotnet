namespace EExpansions.AspNetCore.MultiTenancy;

/// <summary>
/// The tenant model.
/// </summary>
public class Tenant
{
    /// <summary>
    /// The tenant id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The tenant name to display.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The connection string of the tenant store.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// The schema name of the tenant store.
    /// </summary>
    public string? SchemaName { get; set; }
}
