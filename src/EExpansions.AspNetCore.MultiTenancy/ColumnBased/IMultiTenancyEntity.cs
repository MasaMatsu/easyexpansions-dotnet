namespace EExpansions.AspNetCore.MultiTenancy.ColumnBased;

/// <summary>
/// Defines a property for recording a tenant ID.
/// </summary>
public interface IMultiTenancyEntity
{
    /// <summary>
    /// The tenant ID.
    /// </summary>
    Guid TenantId { get; set; }
}
