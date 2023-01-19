using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EExpansions.EntityFrameworkCore;

using Internal;

/// <summary>
/// The interceptor that activates
/// <see cref="IEntityCreationRecordable"/>,
/// <see cref="IEntityUpdationRecordable"/> and
/// <see cref="IEntitySoftDeletionRecordable"/>
/// instead of <see cref="EEDbContext"/>.
/// <para>
/// It is ignored if <typeparamref name="TContext"/> has <see cref="HasEEDbContextLogicsAttribute"/>.
/// </para>
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
public class EESaveChangesInterceptor<TContext> : SaveChangesInterceptor
    where TContext : DbContext
{
    protected static bool IsEEDbContext => ModelBuilderExtensions.IsEEDbContext<TContext>();

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnCreating(entity, now);
    }

    /// <summary>
    /// Configures the entity on updation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnUpdating(entity, now);
    }

    /// <summary>
    /// Configures the entity on deletion.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnDeleting(entity, now);
    }

    /// <summary>
    /// Configures the entity on restoring deleted entity.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    protected virtual void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        EEDbContextImplementations.OnRestoring(entity);
    }

    #region PrimitiveSavingChanges

    protected InterceptionResult<int> PrimitiveSavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        return base.SavingChanges(eventData, result);
    }

    protected ValueTask<InterceptionResult<int>> PrimitiveSavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken
    )
    {
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    #endregion

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (!IsEEDbContext && eventData.Context is TContext context)
        {
            var now = DateTimeOffset.UtcNow;

            EEDbContextImplementations.OnSaveChanges(
                context,
                OnCreating,
                OnUpdating,
                OnDeleting,
                OnRestoring,
                now
            );
        }

        return PrimitiveSavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (!IsEEDbContext && eventData.Context is TContext context)
        {
            var now = DateTimeOffset.UtcNow;

            EEDbContextImplementations.OnSaveChanges(
                context,
                OnCreating,
                OnUpdating,
                OnDeleting,
                OnRestoring,
                now
            );
        }

        return PrimitiveSavingChangesAsync(eventData, result, cancellationToken);
    }
}

/// <summary>
/// The interceptor that activates
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey}"/>
/// and their derived interfaces instead of <see cref="EEDbContext{TUserForeignKey}"/>.
/// <para>
/// It is ignored if <typeparamref name="TContext"/> has <see cref="HasEEDbContextLogicsAttribute"/>.
/// </para>
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public class EESaveChangesInterceptor<TContext, TUserForeignKey> : EESaveChangesInterceptor<TContext>
    where TContext : DbContext, IUserIdGettable<TUserForeignKey>
{
    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnCreating(entity, now);

        if (entity is IEntityCreationRecordable<TUserForeignKey> creatable)
        {
            EEDbContextImplementations.OnCreating(creatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on updation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordable<TUserForeignKey> updatable)
        {
            EEDbContextImplementations.OnUpdating(updatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on deletion.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordable<TUserForeignKey> deletable)
        {
            EEDbContextImplementations.OnDeleting(deletable, id);
        }
    }

    /// <inheritdoc/>
    protected override void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        base.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordable<TUserForeignKey> deletable)
        {
            EEDbContextImplementations.OnRestoring(deletable);
        }
    }

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (!IsEEDbContext && eventData.Context is TContext context)
        {
            var now = DateTimeOffset.UtcNow;
            var id = context.GetUserId();

            EEDbContextImplementations.OnSaveChanges(
                context,
                OnCreating,
                OnUpdating,
                OnDeleting,
                OnRestoring,
                now,
                id
            );
        }

        return PrimitiveSavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (!IsEEDbContext && eventData.Context is TContext context)
        {
            var now = DateTimeOffset.UtcNow;
            var id = context.GetUserId();

            EEDbContextImplementations.OnSaveChanges(
                context,
                OnCreating,
                OnUpdating,
                OnDeleting,
                OnRestoring,
                now,
                id
            );
        }

        return PrimitiveSavingChangesAsync(eventData, result, cancellationToken);
    }
}
