using Microsoft.EntityFrameworkCore;

namespace EExpansions.TestHelper;

public abstract class DbContextFixture<TContext> : IDisposable
    where TContext : DbContext, new()
{
    public DbContextFixture()
    {
        using var context = CreateDbContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Initialize(context);
    }

    public void Dispose()
    {
        using var context = CreateDbContext();

        context.Database.EnsureDeleted();
    }

    protected abstract void Initialize(TContext context);

    public TContext CreateDbContext() => new TContext();

    public IDbContextFactory<TContext> CreateDbContextFactory() => new MockDbContextFactory<TContext>();
}

public class MockDbContextFactory<TContext> : IDbContextFactory<TContext>
    where TContext : DbContext, new()
{
    public TContext CreateDbContext() => new TContext();
}
