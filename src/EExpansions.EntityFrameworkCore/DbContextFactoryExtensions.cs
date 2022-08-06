using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EExpansions.EntityFrameworkCore;

public static class DbContextFactoryExtensions
{
    #region Execute

    /// <summary>
    /// Executes the asynchronously function in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static async Task ExecuteAsync<TContext>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, Task> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await using var context = await factory.CreateDbContextAsync(cancellationToken);
        await funcAsync(context);
    }

    /// <summary>
    /// Executes the asynchronously function in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the return value of <paramref name="funcAsync"/>.
    /// </returns>
    public static async Task<TResult> ExecuteAsync<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, Task<TResult>> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await using var context = await factory.CreateDbContextAsync(cancellationToken);
        return await funcAsync(context);
    }

    /// <summary>
    /// Executes the action in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="action">The action executed in the context.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    public static void Execute<TContext>(
        this IDbContextFactory<TContext> factory,
        Action<TContext> action
    )
        where TContext: DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        using var context = factory.CreateDbContext();
        action(context);
    }

    /// <summary>
    /// Executes the action in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="func">The function executed in the context.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="func"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// The return value of <paramref name="func"/>.
    /// </returns>
    public static TResult Execute<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, TResult> func
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = func ?? throw new ArgumentNullException(nameof(func));

        using var context = factory.CreateDbContext();
        return func(context);
    }

    #endregion

    #region Execute in transaction

    internal static async Task<TResult> InternalExecuteInTransactionAsync<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        Func<TContext, Task<IDbContextTransaction>> beginTransactionAsync,
        CancellationToken cancellationToken
    )
        where TContext : DbContext
    {
        await using var context = await factory.CreateDbContextAsync(cancellationToken);
        return await context.InternalExecuteInTransactionAsync(funcAsync, beginTransactionAsync, cancellationToken);
    }

    internal static TResult InternalExecuteInTransaction<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, TResult> func,
        Func<TContext, IDbContextTransaction> beginTransaction
    )
        where TContext : DbContext
    {
        using var context = factory.CreateDbContext();
        return context.InternalExecuteInTransaction(func, beginTransaction);
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static async Task ExecuteInTransactionAsync<TContext>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, CancellationToken, Task> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await factory.InternalExecuteInTransactionAsync(
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
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public static async Task ExecuteInTransactionAsync<TContext>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, CancellationToken, Task> funcAsync,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        await factory.InternalExecuteInTransactionAsync(
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
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the return value of <paramref name="funcAsync"/>.
    /// </returns>
    public static async Task<TResult> ExecuteInTransactionAsync<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        return await factory.InternalExecuteInTransactionAsync(
            funcAsync,
            ctx => ctx.Database.BeginTransactionAsync(cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the asynchronously function in the transaction.
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="funcAsync">The function executed in the context.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the return value of <paramref name="funcAsync"/>.
    /// </returns>
    public static async Task<TResult> ExecuteInTransactionAsync<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, CancellationToken, Task<TResult>> funcAsync,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        return await factory.InternalExecuteInTransactionAsync(
            funcAsync,
            ctx => ctx.Database.BeginTransactionAsync(isolationLevel, cancellationToken),
            cancellationToken
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="action">The action executed in the tracsaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    public static void ExecuteInTransaction<TContext>(
        this IDbContextFactory<TContext> factory,
        Action<TContext> action
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        factory.InternalExecuteInTransaction(
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
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="action">The action executed in the tracsaction.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    public static void ExecuteInTransaction<TContext>(
        this IDbContextFactory<TContext> factory,
        Action<TContext> action,
        IsolationLevel isolationLevel
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        factory.InternalExecuteInTransaction(
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
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="func">The function executed in the context.</param>
    /// <returns>
    /// The return value of <paramref name="func"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="func"/> are <see langword="null" />.
    /// </exception>
    public static TResult ExecuteInTransaction<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, TResult> func
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = func ?? throw new ArgumentNullException(nameof(func));

        return factory.InternalExecuteInTransaction(
            func,
            ctx => ctx.Database.BeginTransaction()
        );
    }

    /// <summary>
    /// Executes the action in the transaction.
    /// The transaction is executed in the context that is created by the factory.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    /// <param name="factory">The factory to create context.</param>
    /// <param name="func">The function executed in the context.</param>
    /// <param name="isolationLevel">The isolation level to use for the transaction.</param>
    /// <returns>
    /// The return value of <paramref name="func"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="func"/> are <see langword="null" />.
    /// </exception>
    public static TResult ExecuteInTransaction<TContext, TResult>(
        this IDbContextFactory<TContext> factory,
        Func<TContext, TResult> func,
        IsolationLevel isolationLevel
    )
        where TContext : DbContext
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));
        _ = func ?? throw new ArgumentNullException(nameof(func));

        return factory.InternalExecuteInTransaction(
            func,
            ctx => ctx.Database.BeginTransaction(isolationLevel)
        );
    }

    #endregion
}
