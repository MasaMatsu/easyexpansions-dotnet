namespace EExpansions;

/// <summary>
/// Extensions
/// </summary>
public static class SemaphoreSlimExtensions
{
    /// <summary>
    /// Executes an asynchronously function after waiting for <see cref="SemaphoreSlim"/>.
    /// Finally releases the <see cref="SemaphoreSlim"/>.
    /// </summary>
    /// <param name="semaphore"><see cref="SemaphoreSlim"/> for exclusive control.</param>
    /// <param name="funcAsync">The asynchronously function to execute in lock.</param>
    /// <param name="timeout">
    /// A <see cref="TimeSpan" /> that represents the number of milliseconds to wait.
    /// <see cref="Timeout.InfiniteTimeSpan"/> is used if not specified.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="semaphore"/> or <paramref name="funcAsync"/> are <see langword="null" />.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// <see cref="SemaphoreSlim"/> timeout period has expired.
    /// </exception>
    public static async Task ExecuteInLockAsync(
        this SemaphoreSlim semaphore,
        Func<Task> funcAsync,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default
    )
    {
        _ = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        _ = funcAsync ?? throw new ArgumentNullException(nameof(funcAsync));

        cancellationToken.ThrowIfCancellationRequested();

        if (!await semaphore.WaitAsync(timeout ?? Timeout.InfiniteTimeSpan, cancellationToken))
        {
            throw new TimeoutException("The semaphore timeout period has expired.");
        }
        try
        {
            await funcAsync();
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Executes an action after waiting for <see cref="SemaphoreSlim"/>.
    /// Finally releases the <see cref="SemaphoreSlim"/>.
    /// </summary>
    /// <param name="semaphore"><see cref="SemaphoreSlim"/> for exclusive control.</param>
    /// <param name="action">The action to execute in lock.</param>
    /// <param name="timeout">
    /// A <see cref="TimeSpan" /> that represents the number of milliseconds to wait.
    /// <see cref="Timeout.InfiniteTimeSpan"/> is used if not specified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="semaphore"/> or <paramref name="action"/> are <see langword="null" />.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// <see cref="SemaphoreSlim"/> timeout period has expired.
    /// </exception>
    public static void ExecuteInLock(
        this SemaphoreSlim semaphore,
        Action action,
        TimeSpan? timeout = null
    )
    {
        _ = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        if (!semaphore.Wait(timeout ?? Timeout.InfiniteTimeSpan))
        {
            throw new TimeoutException("The semaphore timeout period has expired.");
        }
        try
        {
            action();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
