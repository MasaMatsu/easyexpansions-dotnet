using System.ComponentModel.DataAnnotations;

namespace EExpansions;

public class EnumExtensionsTests
{
    private const string DisplayName = "TheDisplayName";

    private enum VerificationEnum
    {
        [Display(Name = DisplayName)]
        HaveDisplayName,

        HaveNoDisplayName,
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(VerificationEnum.HaveDisplayName, DisplayName)]
    [InlineData(VerificationEnum.HaveNoDisplayName, null)]
    public void GetDisplayName(Enum? enumValue, string? expected)
    {
        if (enumValue is null)
        {
            Assert.Throws<ArgumentNullException>(() => enumValue!.GetDisplayName());
            return;
        }
        Assert.Equal(expected, enumValue.GetDisplayName());
    }
}
