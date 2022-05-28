using System.ComponentModel.DataAnnotations;

namespace EExpansions.EntityFrameworkCore;

public sealed class TodoDbContext : EEDbContext<Guid, User>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=localhost,11443;Database=Todo;User id=sa;Password=Passw0rd!Passw0rd!");
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    protected override Guid? GetUserId()
    {
        return Guid.NewGuid();
    }
}

public class User
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class TodoItem : IEntityCreationRecordable<Guid, User>, IEntityUpdationRecordable<Guid, User>, IEntitySoftDeletionRecordable<Guid, User>
{
    [Key]
    public Guid Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;
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

public class TodoDbContextFixture : TestHelper.DbContextFixture<TodoDbContext>
{
    protected override void Initialize(TodoDbContext context)
    {
        throw new NotImplementedException();
    }
}

[Collection(nameof(TodoDbContextCollectionFixture))]
public class TodoDbContextCollectionFixture : ICollectionFixture<TodoDbContextFixture>
{
    // There is nothing to do.
}
