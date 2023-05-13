using System.ComponentModel.DataAnnotations;

namespace EExpansions.EntityFrameworkCore;

public sealed class TodoDbContext : EEDbContext<Guid?, User>
{
    public static Guid? UserId = null;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=localhost,11433;Database=EExpansions_EntityFrameworkCore_Tests;User id=sa;Password=Passw0rd!Passw0rd!;TrustServerCertificate=True");
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    public override Guid? GetUserId()
    {
        return UserId;
    }
}

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TodoItem : IEntityUpsertionRecordable<Guid?, User>, IEntitySoftDeletionRecordable<Guid?, User>
{
    [Key]
    public Guid Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public User? Creator { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
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
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
        };
        TodoDbContext.UserId = user.Id;
        context.Users.Add(user);
        context.SaveChanges();
    }
}

[CollectionDefinition(nameof(TodoDbContextCollectionFixture))]
public class TodoDbContextCollectionFixture : ICollectionFixture<TodoDbContextFixture>
{
    // There is nothing to do.
}
