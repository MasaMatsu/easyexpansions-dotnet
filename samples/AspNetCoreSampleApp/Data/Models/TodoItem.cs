using System.ComponentModel.DataAnnotations;
using EExpansions.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Data.Models;

public class TodoItem : IEntityCreationRecordable<Guid, User>, IEntityUpdationRecordable<Guid, User>, IEntitySoftDeletionRecordable<Guid, User>
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Title { get; set; } = string.Empty;

    public User? Creator { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public User? Updater { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User? Deleter { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
