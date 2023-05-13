using System.Runtime.CompilerServices;
using EExpansions.EntityFrameworkCore.ValueGeneration;

namespace EExpansions.EntityFrameworkCore.Internal;

/// <summary>
/// EEDbContext internal implemantations.
/// </summary>
public static class EEDbContextImplementations
{
    #region OnModelCreating

    /// <summary>
    /// OnModelCreating implementation.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entities<IEntityCreationRecordable>(b =>
            b.Property<DateTimeOffset?>(
                nameof(IEntityCreationRecordable.CreatedAt)
            )
            .IsRequired(false)
            .HasValueGenerator<UtcNowValueGenerator>()
        );
        modelBuilder.Entities<IEntityUpdationRecordable>(b =>
            b.Property<DateTimeOffset?>(
                nameof(IEntityUpdationRecordable.UpdatedAt)
            )
            .IsRequired(false)
            .HasValueGenerator<UtcNowValueGenerator>()
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordable>(b =>
        {
            b.Property<bool>(
                nameof(IEntitySoftDeletionRecordable.IsDeleted)
            )
            .IsRequired(true)
            .HasDefaultValue(false);

            b.Property<DateTimeOffset?>(
                nameof(IEntitySoftDeletionRecordable.DeletedAt)
            )
            .IsRequired(false);
        });
    }

    /// <summary>
    /// OnModelCreating implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating<TUserForeignKey>(ModelBuilder modelBuilder)
    {
        modelBuilder.Entities<IEntityCreationRecordable<TUserForeignKey>>(b =>
            b.Property<TUserForeignKey?>(
                nameof(IEntityCreationRecordable<TUserForeignKey>.CreatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntityUpdationRecordable<TUserForeignKey>>(b =>
            b.Property<TUserForeignKey?>(
                nameof(IEntityUpdationRecordable<TUserForeignKey>.UpdatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordable<TUserForeignKey>>(b =>
            b.Property<TUserForeignKey?>(
                nameof(IEntitySoftDeletionRecordable<TUserForeignKey>.DeletedBy)
            )
            .IsRequired(false)
        );
    }

    /// <summary>
    /// OnModelCreating implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating<TUserForeignKey, TUser>(ModelBuilder modelBuilder)
        where TUser : class
    {
        modelBuilder.Entities<IEntityCreationRecordable<TUserForeignKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityCreationRecordable<TUserForeignKey, TUser>.Creator)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityCreationRecordable<TUserForeignKey, TUser>.CreatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntityUpdationRecordable<TUserForeignKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityUpdationRecordable<TUserForeignKey, TUser>.Updater)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityUpdationRecordable<TUserForeignKey, TUser>.UpdatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordable<TUserForeignKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntitySoftDeletionRecordable<TUserForeignKey, TUser>.Deleter)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntitySoftDeletionRecordable<TUserForeignKey, TUser>.DeletedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
    }

    #endregion

    /// <summary>
    /// OnCreating implementation.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="now"></param>
    public static void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now)
    {
        entity.CreatedAt = now;
    }

    /// <summary>
    /// OnCreating implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="entity"></param>
    /// <param name="id"></param>
    public static void OnCreating<TUserForeignKey>(IEntityCreationRecordable<TUserForeignKey> entity, TUserForeignKey id)
    {
        entity.CreatedBy = id;
    }

    /// <summary>
    /// OnUpdating implementation.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="now"></param>
    public static void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now)
    {
        entity.UpdatedAt = now;
    }

    /// <summary>
    /// OnUpdating implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="entity"></param>
    /// <param name="id"></param>
    public static void OnUpdating<TUserForeignKey>(IEntityUpdationRecordable<TUserForeignKey> entity, TUserForeignKey id)
    {
        entity.UpdatedBy = id;
    }

    /// <summary>
    /// OnDeleting implementation.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="now"></param>
    public static void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now)
    {
        entity.DeletedAt = now;
    }

    /// <summary>
    /// OnDeleting implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="entity"></param>
    /// <param name="id"></param>
    public static void OnDeleting<TUserForeignKey>(IEntitySoftDeletionRecordable<TUserForeignKey> entity, TUserForeignKey id)
    {
        entity.DeletedBy = id;
    }

    /// <summary>
    /// OnRestoring implementation.
    /// </summary>
    /// <param name="entity"></param>
    public static void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        entity.DeletedAt = null;
    }

    /// <summary>
    /// OnRestoring implementation.
    /// </summary>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="entity"></param>
    public static void OnRestoring<TUserForeignKey>(IEntitySoftDeletionRecordable<TUserForeignKey> entity)
    {
        entity.DeletedBy = default!;
    }

    /// <summary>
    /// SaveChanges implementation.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="context"></param>
    /// <param name="onCreating"></param>
    /// <param name="onUpdating"></param>
    /// <param name="onDeleting"></param>
    /// <param name="onRestoring"></param>
    /// <param name="now"></param>
    public static void OnSaveChanges<TContext>(
        TContext context,
        Action<IEntityCreationRecordable, DateTimeOffset> onCreating,
        Action<IEntityUpdationRecordable, DateTimeOffset> onUpdating,
        Action<IEntitySoftDeletionRecordable, DateTimeOffset> onDeleting,
        Action<IEntitySoftDeletionRecordable> onRestoring,
        DateTimeOffset now
    )
        where TContext : DbContext
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    {
                        if (entry.Entity is IEntityCreationRecordable creatable)
                        {
                            onCreating(creatable, now);
                        }
                        if (entry.Entity is IEntityUpdationRecordable updatable)
                        {
                            onUpdating(updatable, now);
                        }
                    }
                    break;
                case EntityState.Modified:
                    {
                        if (entry.Entity is IEntityUpdationRecordable updatable)
                        {
                            onUpdating(updatable, now);
                        }
                        if (entry.Entity is IEntitySoftDeletionRecordable deletable)
                        {
                            var isModified =
                                entry.Property(
                                    nameof(IEntitySoftDeletionRecordable.IsDeleted)
                                )
                                .IsModified;
                            if (deletable.IsDeleted && isModified)
                            {
                                onDeleting(deletable, now);
                            }
                            else if (!deletable.IsDeleted && isModified)
                            {
                                onRestoring(deletable);
                            }
                        }
                    }
                    break;
                case EntityState.Deleted:
                    {
                        if (entry.Entity is IEntitySoftDeletionRecordableIgnoringHardDeletion deletable)
                        {
                            entry.State = EntityState.Modified;
                            deletable.IsDeleted = true;
                            onDeleting(deletable, now);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// SaveChanges implementation.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TUserForeignKey"></typeparam>
    /// <param name="context"></param>
    /// <param name="onCreating"></param>
    /// <param name="onUpdating"></param>
    /// <param name="onDeleting"></param>
    /// <param name="onRestoring"></param>
    /// <param name="now"></param>
    /// <param name="userId"></param>
    public static void OnSaveChanges<TContext, TUserForeignKey>(
        TContext context,
        Action<IEntityCreationRecordable, DateTimeOffset, TUserForeignKey> onCreating,
        Action<IEntityUpdationRecordable, DateTimeOffset, TUserForeignKey> onUpdating,
        Action<IEntitySoftDeletionRecordable, DateTimeOffset, TUserForeignKey> onDeleting,
        Action<IEntitySoftDeletionRecordable> onRestoring,
        DateTimeOffset now,
        TUserForeignKey userId
    )
        where TContext : DbContext
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    {
                        if (entry.Entity is IEntityCreationRecordable creatable)
                        {
                            onCreating(creatable, now, userId);
                        }
                        if (entry.Entity is IEntityUpdationRecordable updatable)
                        {
                            onUpdating(updatable, now, userId);
                        }
                    }
                    break;
                case EntityState.Modified:
                    {
                        if (entry.Entity is IEntityUpdationRecordable updatable)
                        {
                            onUpdating(updatable, now, userId);
                        }
                        if (entry.Entity is IEntitySoftDeletionRecordable deletable)
                        {
                            var isModified =
                                entry.Property(
                                    nameof(IEntitySoftDeletionRecordable.IsDeleted)
                                )
                                .IsModified;
                            if (deletable.IsDeleted && isModified)
                            {
                                onDeleting(deletable, now, userId);
                            }
                            else if (!deletable.IsDeleted && isModified)
                            {
                                onRestoring(deletable);
                            }
                        }
                    }
                    break;
                case EntityState.Deleted:
                    {
                        if (entry.Entity is IEntitySoftDeletionRecordableIgnoringHardDeletion deletable)
                        {
                            entry.State = EntityState.Modified;
                            deletable.IsDeleted = true;
                            onDeleting(deletable, now, userId);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
