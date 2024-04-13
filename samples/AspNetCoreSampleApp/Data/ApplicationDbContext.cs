using EExpansions.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreSampleApp.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSampleApp.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor
) : EEIdentityDbContext<User>(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;

    public override string? GetUserId()
    {
        var claim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
}
