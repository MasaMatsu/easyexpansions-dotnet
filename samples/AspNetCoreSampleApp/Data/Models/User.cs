using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSampleApp.Data.Models;

public class User : IdentityUser, IEntityUpsertionRecordable<string?, User>, IEntitySoftDeletionRecordable<string?, User>
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public User? Creator { get; set; }

    public string? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public User? Updater { get; set; }
    
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public User? Deleter { get; set; }
}
