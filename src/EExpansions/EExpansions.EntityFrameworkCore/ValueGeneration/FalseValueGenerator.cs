using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EExpansions.EntityFrameworkCore;

internal class FalseValueGenerator : ValueGenerator<bool>
{
    public override bool GeneratesTemporaryValues => false;

    public override bool Next(EntityEntry entry)
    {
        return false;
    }
}
