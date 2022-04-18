namespace EExpansions;

public static class EnumerableExtensions
{
    #region Asynchronously extensions

    private static async Task<IEnumerable<(TSource source, bool isTarget)>> GetAllResultAsync<TSource>(
        IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, int, Task<bool>> predicate
    )
    {
        var tasks =
            source
            .Select(async (s, i) => (
                source: s,
                isTarget: await predicate(s, i)
            ));
        return
            synchronously
            ? await tasks.WhenAllSync()
            : await tasks.WhenAll();
    }

    private static async Task<IEnumerable<TSource>> TakeWhileResultAsync<TSource>(
        IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate,
        int count
    )
    {
        var tasks =
            source
            .Select(async s => (
                source: s,
                isTarget: await predicate(s)
            ));

        var results = new List<TSource>();
        foreach (var task in tasks)
        {
            var result = await task;
            if (result.isTarget)
            {
                results.Add(result.source);
                if (results.Count >= count)
                {
                    break;
                }
            }
        }
        return results;
    }

    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> that contains the elements to apply the predicate to.</param>
    /// <param name="synchronously">If it is <see langword="true"/>, predicates are processed one at a time synchronously.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <see langword="true" /> if every element of the source sequence passes the test in the specified predicate,
    /// or if the sequence is empty; otherwise, <see langword="false" />.
    /// </returns>
    public static async Task<bool> AllAsync<TSource>(
        this IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, synchronously, (s, _) => predicate(s)))
            .All(r => r.isTarget);
    }

    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> whose elements to apply the predicate to.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <see langword="true" /> if the source sequence is not empty and at least one of its elements passes the test in the specified predicate; otherwise, <see langword="false" />.
    /// </returns>
    public static async Task<bool> AnyAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 1))
            .Any();
    }

    /// <summary>
    /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> that contains elements to be tested and counted.</param>
    /// <param name="synchronously">If it is <see langword="true"/>, predicates are processed one at a time synchronously.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number that represents how many elements in the sequence satisfy the condition in the predicate function.
    /// </returns>
    public static async Task<int> CountAsync<TSource>(
        this IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, synchronously, (s, _) => predicate(s)))
            .Count(r => r.isTarget);
    }

    /// <summary>
    /// Returns an <see cref="long" /> that represents how many elements in a sequence satisfy a condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> that contains the elements to be counted.</param>
    /// <param name="synchronously">If it is <see langword="true"/>, predicates are processed one at a time synchronously.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number that represents how many elements in the sequence satisfy the condition in the predicate function.
    /// </returns>
    public static async Task<long> LongCountAsync<TSource>(
        this IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, synchronously, (s, _) => predicate(s)))
            .LongCount(r => r.isTarget);
    }

    /// <summary>
    /// Returns the first element in a sequence that satisfies a specified condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No element satisfies the condition in <paramref name="predicate" />.
    /// -or-
    /// The source sequence is empty.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the first element in the sequence that passes the test in the specified predicate function.
    /// </returns>
    public static async Task<TSource> FirstAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        var results = await TakeWhileResultAsync(source, predicate, 1);
        if (results.Any())
        {
            return results.First();
        }
        throw new InvalidOperationException("No element satisfies the condition.");
    }

    /// <summary>
    /// Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <see langword="default" />(<typeparamref name="TSource"/>) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />;
    /// otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.
    /// </returns>
    public static async Task<TSource?> FirstOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 1))
            .FirstOrDefault();
    }

    /// <summary>
    /// Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <paramref name="defaultValue" /> if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />;
    /// otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.
    /// </returns>
    public static async Task<TSource> FirstOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate,
        TSource defaultValue
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 1))
            .FirstOrDefault(defaultValue);
    }

    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No element satisfies the condition in <paramref name="predicate" />.
    /// -or-  
    /// The source sequence is empty.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the last element in the sequence that passes the test in the specified predicate function.
    /// </returns>
    public static async Task<TSource> LastAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        var results = await TakeWhileResultAsync(source.Reverse(), predicate, 1);
        if (results.Any())
        {
            return results.First();
        }
        throw new InvalidOperationException("No element satisfies the condition.");
    }

    /// <summary>
    /// Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <see langword="default" />(<typeparamref name="TSource"/>) if the sequence is empty or if no elements pass the test in the predicate function;
    /// otherwise, the last element that passes the test in the predicate function.
    /// </returns>
    public static async Task<TSource?> LastOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source.Reverse(), predicate, 1))
            .FirstOrDefault();
    }

    /// <summary>
    /// Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <paramref name="defaultValue" /> if the sequence is empty or if no elements pass the test in the predicate function;
    /// otherwise, the last element that passes the test in the predicate function.
    /// </returns>
    public static async Task<TSource> LastOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate,
        TSource defaultValue
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source.Reverse(), predicate, 1))
            .LastOrDefault(defaultValue);
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
    /// <param name="predicate">An asynchronously function to test an element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No element satisfies the condition in <paramref name="predicate" />.  
    /// -or-
    /// More than one element satisfies the condition in <paramref name="predicate" />.  
    /// -or-
    /// The source sequence is empty.</exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the input sequence that satisfies a condition.
    /// </returns>
    public static async Task<TSource> SingleAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 2))
            .Single();
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists;
    /// this method throws an exception if more than one element satisfies the condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
    /// <param name="predicate">An asynchronously function to test an element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// More than one element satisfies the condition in <paramref name="predicate" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the input sequence that satisfies the condition, or <see langword="default" />(<typeparamref name="TSource"/>) if no such element is found.
    /// </returns>
    public static async Task<TSource?> SingleOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 2))
            .SingleOrDefault();
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists;
    /// this method throws an exception if more than one element satisfies the condition.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
    /// <param name="predicate">An asynchronously function to test an element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// More than one element satisfies the condition in <paramref name="predicate" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the input sequence that satisfies the condition, or <paramref name="defaultValue" /> if no such element is found.
    /// </returns>
    public static async Task<TSource> SingleOrDefaultAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate,
        TSource defaultValue
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await TakeWhileResultAsync(source, predicate, 2))
            .SingleOrDefault(defaultValue);
    }

    /// <summary>
    /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return elements from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.
    /// </returns>
    public static async Task<IEnumerable<TSource>> SkipWhileAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, true, (s, _) => predicate(s)))
            .SkipWhile(r => r.isTarget)
            .Select(r => r.source);
    }

    /// <summary>
    /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// The element's index is used in the logic of the predicate function.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return elements from.</param>
    /// <param name="predicate">An asynchronously function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.
    /// </returns>
    public static async Task<IEnumerable<TSource>> SkipWhileAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, int, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, true, predicate))
            .SkipWhile(r => r.isTarget)
            .Select(r => r.source);
    }

    /// <summary>
    /// Returns elements from a sequence as long as a specified condition is true.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">A sequence to return elements from.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}" /> that contains the elements from the input sequence that occur before the element at which the test no longer passes.
    /// </returns>
    public static async Task<IEnumerable<TSource>> TakeWhileAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, true, (s, _) => predicate(s)))
            .TakeWhile(r => r.isTarget)
            .Select(r => r.source);
    }

    /// <summary>
    /// Returns elements from a sequence as long as a specified condition is true. The element's index is used in the logic of the predicate function.
    /// The condition is tested by asynchronously <paramref name="predicate"/>.
    /// </summary>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="predicate">An asynchronously function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}" /> that contains elements from the input sequence that occur before the element at which the test no longer passes.
    /// </returns>
    public static async Task<IEnumerable<TSource>> TakeWhileAsync<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, int, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, true, predicate))
            .TakeWhile(r => r.isTarget)
            .Select(r => r.source);
    }

    /// <summary>
    /// Filters a sequence of values based on an predicate that is an asynchronously function.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to filter.</param>
    /// <param name="synchronously">If it is <see langword="true"/>, predicates are processed one at a time synchronously.</param>
    /// <param name="predicate">An asynchronously function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="IEnumerable{T}" /> that contains
    /// <see cref="Task{TResult}"/> elements from the input sequence that satisfy the condition.
    /// </returns>
    public static async Task<IEnumerable<TSource>> WhereAsync<TSource>(
        this IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, synchronously, (s, _) => predicate(s)))
            .Where(r => r.isTarget)
            .Select(r => r.source);
    }

    /// <summary>
    /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to filter.</param>
    /// <param name="synchronously">If it is <see langword="true"/>, predicates are processed one at a time synchronously.</param>
    /// <param name="predicate">An asynchronously function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.
    /// </exception>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}" /> that contains elements from the input sequence that satisfy the condition.
    /// </returns>
    public static async Task<IEnumerable<TSource>> WhereAsync<TSource>(
        this IEnumerable<TSource> source,
        bool synchronously,
        Func<TSource, int, Task<bool>> predicate
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return
            (await GetAllResultAsync(source, synchronously, predicate))
            .Where(r => r.isTarget)
            .Select(r => r.source);
    }

    #endregion
}
