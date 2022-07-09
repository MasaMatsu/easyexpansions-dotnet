namespace EExpansions.AspNetCore.MultiTenancy.ColumnBased;

public static class ColumnBasedModelBuilderExtensions
{
    /// <summary>
    /// Performs configuration of entity types that inherit <see cref="IMultiTenancyEntity"/>.
    /// <para>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use column-based multi-tenancy.
    /// </para>
    /// </summary>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
    /// <param name="tenandId">The tenant ID.</param>
    public static void UseColumnBasedMultiTenancy(this ModelBuilder modelBuilder, Guid tenandId)
    {
        modelBuilder.Entities<IMultiTenancyEntity>(builder =>
        {
            builder.AddQueryFilter<IMultiTenancyEntity>(e => e.TenantId == tenandId);
        });
    }
}
