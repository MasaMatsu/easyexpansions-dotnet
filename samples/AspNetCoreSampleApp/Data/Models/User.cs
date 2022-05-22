using EExpansions.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSampleApp.Data.Models;

public class User : IdentityUser<Guid>, IEntityCreationRecordable<Guid, User>, IEntityUpdationRecordable<Guid, User>, IEntitySoftDeletionRecordable<Guid, User>
{
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public User? Creator { get; set; }

    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public User? Updater { get; set; }
    
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public User? Deleter { get; set; }
}
