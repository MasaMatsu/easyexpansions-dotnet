﻿using System.ComponentModel.DataAnnotations;
using EExpansions.EntityFrameworkCore;

namespace AspNetCoreSampleApp.Data.Models;

public class TodoItem : IEntityUpsertionRecordable<string?, User>, IEntitySoftDeletionRecordable<string?, User>
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Title { get; set; } = string.Empty;

    public User? Creator { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public User? Updater { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public User? Deleter { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
