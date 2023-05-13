namespace EExpansions;

/// <summary>
/// Extensions
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task" /> objects in an enumerable collection have completed.
    /// <para>This method is a wrapper of <see cref="Task.WhenAll(IEnumerable{Task})"/>.</para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="tasks" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the completion of all of the supplied tasks.
    /// </returns>
    public static Task WhenAll(this IEnumerable<Task> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks);
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task{TResult}" /> objects in an enumerable collection have completed.
    /// <para>This method is a wrapper of <see cref="Task.WhenAll{TResult}(IEnumerable{Task{TResult}})"/>.</para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="ArgumentNullException">The <paramref name="tasks" /> is <see langword="null" />.</exception>
    /// <returns>
    /// A task that represents the completion of all of the supplied tasks.
    /// </returns>
    public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks);
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task" /> objects in an enumerable collection have completed.
    /// <para>
    /// This method awaits tasks sequentiality and synchronously.
    /// For example, it is useful in data contexts that only allow synchronous access.
    /// </para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="tasks" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the completion of all of the supplied tasks.
    /// </returns>
    public static async Task WhenAllSync(this IEnumerable<Task> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        foreach (var task in tasks)
        {
            await task;
        }
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task{TResult}" /> objects in an enumerable collection have completed.
    /// <para>
    /// This method awaits tasks sequentiality and synchronously.
    /// For example, it is useful in data contexts that only allow synchronous access.
    /// </para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="ArgumentNullException">The <paramref name="tasks" /> is <see langword="null" />.</exception>
    /// <returns>
    /// A task that represents the completion of all of the supplied tasks.
    /// </returns>
    public static async Task<IEnumerable<TResult>> WhenAllSync<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        var results = new List<TResult>();
        foreach (var task in tasks)
        {
            results.Add(await task);
        }
        return results;
    }

    /// <summary>
    /// Creates a task that will complete when any of the supplied tasks have completed.
    /// <para>This method is a wrapper of <see cref="Task.WhenAny(IEnumerable{Task})"/>.</para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="tasks" /> argument was <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the completion of one of the supplied tasks.
    /// The return task's Result is the task that completed.
    /// </returns>
    public static Task<Task> WhenAny(this IEnumerable<Task> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAny(tasks);
    }

    /// <summary>
    /// Creates a task that will complete when any of the supplied tasks have completed.
    /// <para>This method is a wrapper of <see cref="Task.WhenAny{TResult}(IEnumerable{Task{TResult}})"/>.</para>
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="tasks" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the completion of one of the supplied tasks.
    /// The return task's Result is the task that completed.
    /// </returns>
    public static Task<Task<TResult>> WhenAny<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAny(tasks);
    }
}
