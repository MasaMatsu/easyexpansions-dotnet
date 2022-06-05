using EExpansions.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreSampleApp.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSampleApp.Data;

public class ApplicationDbContext : EEIdentityDbContext<User>
{
    private IHttpContextAccessor HttpContextAccessor { get; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;

    public override string? GetUserId()
    {
        var claim = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
}
