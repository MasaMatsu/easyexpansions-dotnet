﻿using System.Runtime.CompilerServices;
using EExpansions.EntityFrameworkCore.ValueGeneration;

namespace EExpansions.EntityFrameworkCore.Internal;

public static class EEDbContextImplementations
{
    #region OnModelCreating

    public static void OnModelCreating(ModelBuilder modelBuilder, bool enableGlobalQueryFilterToFilterSoftDeletedEntities)
    {
        modelBuilder.Entities<IEntityCreationRecordable>(b =>
            b.Property<DateTimeOffset>(
                nameof(IEntityCreationRecordable.CreatedAt)
            )
            .IsRequired(true)
            .HasValueGenerator<UtcNowValueGenerator>()
        );
        modelBuilder.Entities<IEntityUpdationRecordable>(b =>
            b.Property<DateTimeOffset>(
                nameof(IEntityUpdationRecordable.UpdatedAt)
            )
            .IsRequired(true)
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

            if (enableGlobalQueryFilterToFilterSoftDeletedEntities)
            {
                b.AddQueryFilter<IEntitySoftDeletionRecordable>(e => !e.IsDeleted);
            }
        });
    }

    public static void OnModelCreating<TKey>(ModelBuilder modelBuilder)
        where TKey : struct, IEquatable<TKey>
    {
        modelBuilder.Entities<IEntityCreationRecordable<TKey>>(b =>
            b.Property<TKey?>(
                nameof(IEntityCreationRecordable<TKey>.CreatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntityUpdationRecordable<TKey>>(b =>
            b.Property<TKey?>(
                nameof(IEntityUpdationRecordable<TKey>.UpdatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordable<TKey>>(b =>
            b.Property<TKey?>(
                nameof(IEntitySoftDeletionRecordable<TKey>.DeletedBy)
            )
            .IsRequired(false)
        );
    }

    public static void OnModelCreating<TKey, TUser>(ModelBuilder modelBuilder)
        where TKey : struct, IEquatable<TKey>
        where TUser : class
    {
        modelBuilder.Entities<IEntityCreationRecordable<TKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityCreationRecordable<TKey, TUser>.Creator)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityCreationRecordable<TKey, TUser>.CreatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntityUpdationRecordable<TKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityUpdationRecordable<TKey, TUser>.Updater)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityUpdationRecordable<TKey, TUser>.UpdatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordable<TKey, TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntitySoftDeletionRecordable<TKey, TUser>.Deleter)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntitySoftDeletionRecordable<TKey, TUser>.DeletedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
    }

    public static void OnModelCreatingWithStringKey(ModelBuilder modelBuilder)
    {
        modelBuilder.Entities<IEntityCreationRecordableWithStringKey>(b =>
            b.Property<string?>(
                nameof(IEntityCreationRecordableWithStringKey.CreatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntityUpdationRecordableWithStringKey>(b =>
            b.Property<string?>(
                nameof(IEntityUpdationRecordableWithStringKey.UpdatedBy)
            )
            .IsRequired(false)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordableWithStringKey>(b =>
            b.Property<string?>(
                nameof(IEntitySoftDeletionRecordableWithStringKey.DeletedBy)
            )
            .IsRequired(false)
        );
    }

    public static void OnModelCreatingWithStringKey<TUser>(ModelBuilder modelBuilder)
        where TUser : class
    {
        modelBuilder.Entities<IEntityCreationRecordableWithStringKey<TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityCreationRecordableWithStringKey<TUser>.Creator)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityCreationRecordableWithStringKey<TUser>.CreatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntityUpdationRecordableWithStringKey<TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntityUpdationRecordableWithStringKey<TUser>.Updater)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntityUpdationRecordableWithStringKey<TUser>.UpdatedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
        modelBuilder.Entities<IEntitySoftDeletionRecordableWithStringKey<TUser>>(b =>
            b.HasOne(typeof(TUser),
                nameof(IEntitySoftDeletionRecordableWithStringKey<TUser>.Deleter)
            )
            .WithMany()
            .HasForeignKey(
                nameof(IEntitySoftDeletionRecordableWithStringKey<TUser>.DeletedBy)
            )
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientSetNull)
        );
    }

    #endregion

    public static void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now)
    {
        entity.CreatedAt = now;
    }

    public static void OnCreating<TKey>(IEntityCreationRecordable<TKey> entity, TKey? id)
        where TKey : struct, IEquatable<TKey>
    {
        entity.CreatedBy = id;
    }

    public static void OnCreating(IEntityCreationRecordableWithStringKey entity, string? id)
    {
        entity.CreatedBy = id;
    }

    public static void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now)
    {
        entity.UpdatedAt = now;
    }

    public static void OnUpdating<TKey>(IEntityUpdationRecordable<TKey> entity, TKey? id)
        where TKey : struct, IEquatable<TKey>
    {
        entity.UpdatedBy = id;
    }

    public static void OnUpdating(IEntityUpdationRecordableWithStringKey entity, string? id)
    {
        entity.UpdatedBy = id;
    }

    public static void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now)
    {
        entity.DeletedAt = now;
    }

    public static void OnDeleting<TKey>(IEntitySoftDeletionRecordable<TKey> entity, TKey? id)
        where TKey : struct, IEquatable<TKey>
    {
        entity.DeletedBy = id;
    }

    public static void OnDeleting(IEntitySoftDeletionRecordableWithStringKey entity, string? id)
    {
        entity.DeletedBy = id;
    }

    public static void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        entity.DeletedAt = null;
    }

    public static void OnRestoring<TKey>(IEntitySoftDeletionRecordable<TKey> entity)
        where TKey : struct, IEquatable<TKey>
    {
        entity.DeletedBy = default;
    }

    public static void OnRestoring(IEntitySoftDeletionRecordableWithStringKey entity)
    {
        entity.DeletedBy = null;
    }

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

    public static void OnSaveChanges<TContext, TKey>(
        TContext context,
        Action<IEntityCreationRecordable, DateTimeOffset, TKey?> onCreating,
        Action<IEntityUpdationRecordable, DateTimeOffset, TKey?> onUpdating,
        Action<IEntitySoftDeletionRecordable, DateTimeOffset, TKey?> onDeleting,
        Action<IEntitySoftDeletionRecordable> onRestoring,
        DateTimeOffset now,
        TKey? userId
    )
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
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

    public static void OnSaveChanges<TContext>(
        TContext context,
        Action<IEntityCreationRecordable, DateTimeOffset, string?> onCreating,
        Action<IEntityUpdationRecordable, DateTimeOffset, string?> onUpdating,
        Action<IEntitySoftDeletionRecordable, DateTimeOffset, string?> onDeleting,
        Action<IEntitySoftDeletionRecordable> onRestoring,
        DateTimeOffset now,
        string? userId
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
