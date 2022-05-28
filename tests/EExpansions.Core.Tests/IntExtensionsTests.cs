namespace EExpansions;

public class IntExtensionsTests
{
    [Theory]
    [InlineData(0, 1, 5)]
    [InlineData(1, 1, 5)]
    [InlineData(3, 1, 5)]
    [InlineData(5, 1, 5)]
    [InlineData(6, 1, 5)]
    [InlineData(1, -int.MaxValue, int.MaxValue)]
    public void InRange(int value, int start, int count)
    {
        Assert.Equal(
            Enumerable.Range(start, count).Any(i => i == value),
            value.InRange(start, count)
        );
    }

    [Theory]
    [InlineData(0, 1, -1)]
    [InlineData(0, int.MaxValue, int.MaxValue)]
    public void InRange_Exception(int value, int start, int count)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => value.InRange(start, count));
    }
}
