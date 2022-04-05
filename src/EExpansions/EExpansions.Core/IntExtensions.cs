namespace EExpansions.Core;

public static class IntExtensions
{
    /// <summary>
    /// Indicates whether the specified integer is in the specified range.
    /// </summary>
    /// <param name="value">The interger to test.</param>
    /// <param name="start">The value of the first integer in the sequence.</param>
    /// <param name="count">The number of sequential integers to generate.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="value" /> parameter is in the range; otherwise, <see langword="false" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count" /> is less than 0.
    /// -or-
    /// <paramref name="start" /> + <paramref name="count" /> -1 is larger than <see cref="F:System.Int32.MaxValue" />.
    /// </exception>
    public static bool InRange(this int value, int start, int count)
    {
        var max = ((long)start) + count - 1;
        if (count < 0 || max > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return start <= value && value < start + count;
    }
}
