using AspNetCoreSampleApp.Data;
using AspNetCoreSampleApp.Data.Models;
using EExpansions.AspNetCore.Caching;
using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Pages;

public class FetchDataModel : PageModel
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;
    private readonly IEntityCache _entityCache;

    public TodoItem? Item { get; set; } = null!;

    public FetchDataModel(
        IDbContextFactory<ApplicationDbContext> factory,
        IEntityCache entityCache
    )
    {
        _factory = factory;
        _entityCache = entityCache;
    }

    public async Task OnGetAsync()
    {
        var id = await _factory.ExecuteAsync(async c => (await c.TodoItems.FirstOrDefaultAsync())?.Id);
        Item = await _entityCache.GetAsync<TodoItem>(id);
        if (Item is not null)
        {
            Item.Creator = await _entityCache.GetAsync<User>(Item?.CreatedBy);
        }
    }
}
