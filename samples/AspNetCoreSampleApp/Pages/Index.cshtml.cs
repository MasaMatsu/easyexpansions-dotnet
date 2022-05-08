using System.Security.Claims;
using AspNetCoreSampleApp.Data;
using AspNetCoreSampleApp.Data.Models;
using EExpansions;
using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Pages;

public class IndexModel : PageModel
{
    public const string SessionKeyName = "_Name";
    public const string SessionKeyAge = "_Age";

    private readonly IDbContextFactory<ApplicationDbContext> _factory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IDbContextFactory<ApplicationDbContext> factory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<IndexModel> logger
    )
    {
        _factory = factory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
        {
            HttpContext.Session.SetString(SessionKeyName, "The Doctor");
            HttpContext.Session.SetInt32(SessionKeyAge, 73);
        }
        var name = HttpContext.Session.GetString(SessionKeyName);
        var age = HttpContext.Session.GetInt32(SessionKeyAge).ToString();

        _logger.LogInformation("Session Name: {Name}", name);
        _logger.LogInformation("Session Age: {Age}", age);

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _factory.ExecuteAsync(async context =>
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
