namespace EExpansions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("A")]
    public void IsNullOrEmpty(string value)
    {
        Assert.Equal(string.IsNullOrEmpty(value), value.IsNullOrEmpty());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("　")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData(null)]
    [InlineData("A")]
    public void IsNullOrWhitespace(string value)
    {
        Assert.Equal(string.IsNullOrWhiteSpace(value), value.IsNullOrWhitespace());
    }
}
