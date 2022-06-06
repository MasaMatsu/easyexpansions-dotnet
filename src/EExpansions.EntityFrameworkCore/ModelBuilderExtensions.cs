using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EExpansions.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Performs configuration of entity types that inherits a given entity type in the model.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <param name="buildAction">
    /// An action that performs configuration of the entity type that inherits the entity type.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> or <paramref name="buildAction"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    public static ModelBuilder Entities<TEntity>(
        this ModelBuilder modelBuilder,
        Action<EntityTypeBuilder> buildAction
    )
        where TEntity : class
    {
        _ = modelBuilder ?? throw new ArgumentNullException();
        _ = buildAction ?? throw new ArgumentNullException();

        var targetTypes =
            modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(TEntity).IsAssignableFrom(t.ClrType));
        foreach (var t in targetTypes)
        {
            buildAction(modelBuilder.Entity(t.ClrType));
        }
        return modelBuilder;
    }
}
