namespace EExpansions.EntityFrameworkCore;

[Collection(nameof(TodoDbContextCollectionFixture))]
public class DbContextExtensionsTests : IDisposable
{
    private readonly TodoDbContextFixture _fixture;

    public DbContextExtensionsTests(TodoDbContextFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
    }

    [Fact]
    public async Task ExecuteInTransactionAsync()
    {
        var guid = Guid.NewGuid();
        using (var context = _fixture.CreateDbContext())
        {
            var item = new TodoItem
            {
                Id = guid,
                Title = guid.ToString(),
            };
            await context.ExecuteInTransactionAsync(async (ctx, _) =>
            {
                await ctx.TodoItems.AddAsync(item);
                await ctx.SaveChangesAsync(false);
            });
        }

        using (var context = _fixture.CreateDbContext())
        {
            var savedItem =
                context.TodoItems
                .AsNoTracking()
                .FirstOrDefault(ti => ti.Id == guid);
            Assert.NotNull(savedItem);
            Assert.Equal(guid, savedItem?.Id);
        }
    }
}
