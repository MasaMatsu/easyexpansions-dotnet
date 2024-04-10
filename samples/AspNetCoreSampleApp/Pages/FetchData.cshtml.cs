using AspNetCoreSampleApp.Data;
using AspNetCoreSampleApp.Data.Models;
using EExpansions.AspNetCore.Caching;
using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Pages;

public class FetchDataModel(
    IDbContextFactory<ApplicationDbContext> factory,
    IEntityCache entityCache
) : PageModel
{
    public TodoItem? Item { get; set; } = null!;

    public async Task OnGetAsync()
    {
        var id = await factory.ExecuteAsync(async c => (await c.TodoItems.FirstOrDefaultAsync())?.Id);
        Item = await entityCache.GetAsync<TodoItem>(id);
        if (Item is not null)
        {
            Item.Creator = await entityCache.GetAsync<User>(Item?.CreatedBy);
        }
    }
}
