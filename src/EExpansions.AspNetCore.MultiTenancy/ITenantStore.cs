using System.Diagnostics.CodeAnalysis;

namespace EExpansions.AspNetCore.MultiTenancy;

public interface ITenantStore
{
    void SetTenant(Guid tenantId, [NotNull] string? connectionString);

    Task SetTenantAsync(Guid tenantId, [NotNull] string? connectionString, CancellationToken cancellationToken = default);
}
