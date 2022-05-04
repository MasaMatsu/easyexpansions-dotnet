namespace EExpansions.EntityFrameworkCore;

public static class DbSetExtensions
{
    #region Soft remove

    /// <summary>
    /// Enables the <see cref="IEntitySoftDeletionRecordable.IsDeleted"/> of the given entity to begin tracking
    /// such that it will be soft deleted from the database when <see cref="DbContext.SaveChanges"/> is called.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <param name="set"><see cref="DbSet{TEntity}"/> for the method chain.</param>
    /// <param name="entity">The entity to soft remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="set"/> or <paramref name="entity"/> are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="entity"/> does not inherit <see cref="IEntitySoftDeletionRecordable"/>.
    /// </exception>
    public static void SoftRemove<TEntity>(this DbSet<TEntity> set, TEntity entity)
        where TEntity : class
    {
        _ = set ?? throw new ArgumentNullException(nameof(set));
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        if (entity is IEntitySoftDeletionRecordable deletable)
        {
            deletable.IsDeleted = true;
        }
        else
        {
            throw new InvalidOperationException(
                $"The '{entity.GetType().Name}' does not inherit {nameof(IEntitySoftDeletionRecordable)}."
            );
        }
    }

    /// <summary>
    /// Enables the <see cref="IEntitySoftDeletionRecordable.IsDeleted"/> of the given entity to begin tracking
    /// such that it will be soft deleted from the database when <see cref="DbContext.SaveChanges"/> is called.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <param name="set"><see cref="DbSet{TEntity}"/> for the method chain.</param>
    /// <param name="entities">The entity to soft remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="set"/> or <paramref name="entities"/> are <see langword="null" />.
    /// </exception>
    public static void SoftRemoveRange<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        _ = set ?? throw new ArgumentNullException(nameof(set));
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            set.SoftRemove(entity);
        }
    }

    #endregion
}
