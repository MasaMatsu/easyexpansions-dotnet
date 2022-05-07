using EExpansions.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreSampleApp.Data.Models;
using System.Security.Claims;

namespace AspNetCoreSampleApp.Data;

public class ApplicationDbContext : EEIdentityDbContext<User>
{
    private IHttpContextAccessor HttpContextAccessor { get; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    protected override string? GetUserId()
    {
        var claim = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return Convert.ChangeType(claim, typeof(string)) as string;
    }
}
