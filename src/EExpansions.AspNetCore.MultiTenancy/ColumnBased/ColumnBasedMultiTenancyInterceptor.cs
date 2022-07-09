using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EExpansions.AspNetCore.MultiTenancy.ColumnBased;

/// <summary>
/// The interceptor that provides column-based multi-tenancy.
/// </summary>
public class ColumnBasedMultiTenancyInterceptor : SaveChangesInterceptor
{
    private ITenantProvider _tenantProvider;

    public ColumnBasedMultiTenancyInterceptor(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    private void SaveChanges(DbContext? context)
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var tenantId = _tenantProvider.GetTenantId();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is IMultiTenancyEntity entity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.TenantId = tenantId;
                        break;
                    case EntityState.Modified:
                        if (entity.TenantId != tenantId)
                        {
                            throw new AcrossTenantsException(entity.TenantId.ToString(), tenantId.ToString());
                        }
                        entity.TenantId = tenantId;
                        break;
                    case EntityState.Deleted:
                        if (entity.TenantId != tenantId)
                        {
                            throw new AcrossTenantsException(entity.TenantId.ToString(), tenantId.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <inheritdoc />
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        SaveChanges(eventData.Context);

        return base.SavedChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        SaveChanges(eventData.Context);

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
