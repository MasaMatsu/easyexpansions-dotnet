namespace EExpansions.EntityFrameworkCore;

[Collection(nameof(TodoDbContextCollectionFixture))]
public class DbContextExtensionsTests(TodoDbContextFixture fixture) : IDisposable
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task ExecuteInTransactionAsync()
    {
        var guid = Guid.NewGuid();
        using (var context = fixture.CreateDbContext())
        {
            var item = new TodoItem
            {
                Id = guid,
                Title = guid.ToString(),
            };
            await context.ExecuteInTransactionAsync(async (ctx, token) =>
            {
                await ctx.TodoItems.AddAsync(item, token);
                await ctx.SaveChangesAsync(false, token);
            });
        }

        using (var context = fixture.CreateDbContext())
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
