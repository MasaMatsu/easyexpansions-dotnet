using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EExpansions.EntityFrameworkCore.ValueGeneration;

internal class UtcNowValueGenerator : ValueGenerator<DateTimeOffset>
{
    /// <inheritdoc/>
    public override bool GeneratesTemporaryValues => false;

    /// <inheritdoc/>
    public override DateTimeOffset Next(EntityEntry entry)
    {
        return DateTimeOffset.UtcNow;
    }
}
