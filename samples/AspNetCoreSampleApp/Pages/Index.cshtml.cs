using System.Security.Claims;
using AspNetCoreSampleApp.Data;
using AspNetCoreSampleApp.Data.Models;
using EExpansions;
using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Pages;

public class IndexModel(
    IDbContextFactory<ApplicationDbContext> factory,
    IHttpContextAccessor httpContextAccessor,
    ILogger<IndexModel> logger
) : PageModel
{
    public const string SessionKeyName = "_Name";
    public const string SessionKeyAge = "_Age";

    public async Task OnGetAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
        {
            HttpContext.Session.SetString(SessionKeyName, "The Doctor");
            HttpContext.Session.SetInt32(SessionKeyAge, 73);
        }
        var name = HttpContext.Session.GetString(SessionKeyName);
        var age = HttpContext.Session.GetInt32(SessionKeyAge).ToString();

        logger.LogInformation("Session Name: {Name}", name);
        logger.LogInformation("Session Age: {Age}", age);

        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await factory.ExecuteAsync(async context =>
        {
            var exists = await context.TodoItems.AnyAsync();
            if (!exists && !userId.IsNullOrEmpty())
            {
                context.TodoItems.Add(new TodoItem
                {
                    Title = "Buy a kitchen paper.",
                });
                await context.SaveChangesAsync();
            }
        });
    }
}
