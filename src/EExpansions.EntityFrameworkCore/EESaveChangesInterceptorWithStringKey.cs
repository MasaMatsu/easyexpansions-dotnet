using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EExpansions.EntityFrameworkCore;

using Internal;

/// <summary>
/// The interceptor that activates
/// <see cref="IEntityCreationRecordableWithStringKey"/>,
/// <see cref="IEntityUpdationRecordableWithStringKey"/>,
/// <see cref="IEntitySoftDeletionRecordableWithStringKey"/>
/// and their derived interfaces instead of <see cref="EEDbContextWithStringKey"/>.
/// <para>
/// It is ignored if <typeparamref name="TContext"/> has <see cref="HasEEDbContextLogicsAttribute"/>.
/// </para>
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
public class EESaveChangesInterceptorWithStringKey<TContext> : EESaveChangesInterceptor<TContext>
    where TContext : DbContext, IUserIdGettableWithStringKey
{
    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnCreating(entity, now);

        if (entity is IEntityCreationRecordableWithStringKey creatable)
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
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordableWithStringKey updatable)
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
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
        {
            EEDbContextImplementations.OnDeleting(deletable, id);
        }
    }

    /// <inheritdoc/>
    protected override void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        base.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
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
