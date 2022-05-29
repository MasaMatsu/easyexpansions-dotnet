using EExpansions.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreSampleApp.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSampleApp.Data;

public class ApplicationDbContext : EEIdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private IHttpContextAccessor HttpContextAccessor { get; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;

    protected override Guid? GetUserId()
    {
        var claim = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return
            Guid.TryParse(claim?.Value, out var id)
            ? id
            : null;
    }
}
