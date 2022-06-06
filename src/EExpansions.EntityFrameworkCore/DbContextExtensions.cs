using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EExpansions.EntityFrameworkCore;

public static class DbContextExtensions
{
    #region Soft remove

    /// <summary>
    /// Enables the <see cref="IEntitySoftDeletionRecordable.IsDeleted"/> of the given entity to begin tracking
    /// such that it will be soft deleted from the database when <see cref="DbContext.SaveChanges"/> is called.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context for the method chain.</param>
    /// <param name="entity">The entity to soft remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="entity"/> are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="entity"/> does not inherit <see cref="IEntitySoftDeletionRecordable"/>.
    /// </exception>
    public static void SoftRemove<TContext>(this TContext context, object entity)
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
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
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <param name="context">The context for the method chain.</param>
    /// <param name="entity">The entity to soft remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="entity"/> are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="entity"/> does not inherit <see cref="IEntitySoftDeletionRecordable"/>.
    /// </exception>
    public static void SoftRemove<TContext, TEntity>(this TContext context, TEntity entity)
        where TContext : DbContext
        where TEntity : class
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
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
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context for the method chain.</param>
    /// <param name="entities">The sequence of the entities to soft remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="entities"/> are <see langword="null" />.
    /// </exception>
    public static void SoftRemoveRange<TContext>(this TContext context, IEnumerable<object> entities)
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            context.SoftRemove(entity);
        }
    }

    /// <summary>
    /// Enables the <see cref="IEntitySoftDeletionRecordable.IsDeleted"/> of the given entity to begin tracking
    /// such that it will be soft deleted from the database when <see cref="DbContext.SaveChanges"/> is called.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context for the method chain.</param>
    /// <param name="entities">The entities to remove.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="entities"/> are <see langword="null" />.
    /// </exception>
    public static void SoftRemoveRange<TContext>(this TContext context, params object[] entities)
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            context.SoftRemove(entity);
        }
    }

    #endregion

    #region Transaction

    internal static async Task<TResult> InternalExecuteInTransactionAsync<TContext, TResult>(
        this TContext context,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        Func<TContext, Task<IDbContextTransaction>> beginTransactionAsync,
        CancellationToken cancellationToken
    )
        where TContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();
        var result = await strategy.ExecuteAsync(async ct => {
            await using var transaction = await beginTransactionAsync(context);
            try
            {
                var r = await funcAsync(context, ct);
                await transaction.CommitAsync(cancellationToken);
                return r;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }, cancellationToken);

        context.ChangeTracker.AcceptAllChanges();
        return result;
    }

    internal static TResult InternalExecuteInTransaction<TContext, TResult>(
        this TContext context,
        Func<TContext, TResult> func,
        Func<TContext, IDbContextTransaction> beginTransaction
    )
        where TContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();
        var result = strategy.Execute(() =>
        {
            using var transaction = beginTransaction(context);
            try
            {
                var r = func(context);
                transaction.Commit();
                return r;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });

        context.ChangeTracker.AcceptAllChanges();
        return result;
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="funcAsync">The function executed in the tracsaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static async Task ExecuteInTransactionAsync<TContext>(
        this TContext context,
        Func<TContext, CancellationToken, Task> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await context.InternalExecuteInTransactionAsync(
            async (ctx, ct) =>
            {
                await funcAsync(ctx, ct);
                return 0;
            },
            ctx => ctx.Database.BeginTransactionAsync(cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="funcAsync">The function executed in the tracsaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static async Task ExecuteInTransactionAsync<TContext>(
        this TContext context,
        Func<TContext, CancellationToken, Task> funcAsync,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await context.InternalExecuteInTransactionAsync(
            async (ctx, ct) =>
            {
                await funcAsync(ctx, ct);
                return 0;
            },
            ctx => ctx.Database.BeginTransactionAsync(isolationLevel, cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="funcAsync">The function executed in the tracsaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the return value of <paramref name="funcAsync"/>.
    /// </returns>
    public static async Task<TResult> ExecuteInTransactionAsync<TContext, TResult>(
        this TContext context,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        return await context.InternalExecuteInTransactionAsync(
            funcAsync,
            ctx => ctx.Database.BeginTransactionAsync(cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="funcAsync">The function executed in the tracsaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the return value of <paramref name="funcAsync"/>.
    /// </returns>
    public static async Task<TResult> ExecuteInTransactionAsync<TContext, TResult>(
        this TContext context,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        return await context.InternalExecuteInTransactionAsync(
            funcAsync,
            ctx => ctx.Database.BeginTransactionAsync(isolationLevel, cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="action">The action executed in the tracsaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    public static void ExecuteInTransaction<TContext>(
        this TContext context,
        Action<TContext> action
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        context.InternalExecuteInTransaction(
            ctx =>
            {
                action(ctx);
                return 0;
            },
            ctx => ctx.Database.BeginTransaction()
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="action">The action executed in the tracsaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    public static void ExecuteInTransaction<TContext>(
        this TContext context,
        Action<TContext> action,
        IsolationLevel isolationLevel
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        context.InternalExecuteInTransaction(
            ctx =>
            {
                action(ctx);
                return 0;
            },
            ctx => ctx.Database.BeginTransaction(isolationLevel)
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="func">The function executed in the tracsaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="func"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// The return value of <paramref name="func"/>.
    /// </returns>
    public static TResult ExecuteInTransaction<TContext, TResult>(
        this TContext context,
        Func<TContext, TResult> func
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = func ?? throw new ArgumentNullException(nameof(func));

        return context.InternalExecuteInTransaction(
            func,
            ctx => ctx.Database.BeginTransaction()
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="context">The context to execute the function.</param>
    /// <param name="func">The function executed in the tracsaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="context"/> or <paramref name="func"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// The return value of <paramref name="func"/>.
    /// </returns>
    public static TResult ExecuteInTransaction<TContext, TResult>(
        this TContext context,
        Func<TContext, TResult> func,
        IsolationLevel isolationLevel
    )
        where TContext : DbContext
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));
        _ = func ?? throw new ArgumentNullException(nameof(func));

        return context.InternalExecuteInTransaction(
            func,
            ctx => ctx.Database.BeginTransaction(isolationLevel)
        );
    }

    #endregion
}
