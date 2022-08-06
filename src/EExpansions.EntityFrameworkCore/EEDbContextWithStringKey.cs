namespace EExpansions.EntityFrameworkCore;

using Internal;

/// <summary>
/// The wrapper class of <see cref="DbContext"/> to activate
/// <see cref="IEntityCreationRecordableWithStringKey"/>,
/// <see cref="IEntityUpdationRecordableWithStringKey"/>,
/// <see cref="IEntitySoftDeletionRecordableWithStringKey"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEDbContext"/>.
/// </summary>
public abstract class EEDbContextWithStringKey : EEDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TKey}" /> class.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will be called to configure the database (and other options) to be used for this context.
    /// </summary>
    public EEDbContextWithStringKey() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TKey}" /> class using the specified options.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will still be called to allow further configuration of the options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEDbContextWithStringKey(DbContextOptions options) : base(options) { }

    /// <inheritdoc cref="DbContext.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreatingWithStringKey(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnCreating(entity, now);

        if (entity is IEntityCreationRecordableWithStringKey creatable)
        {
            EEDbContextImplementations.OnCreating(creatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on updation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordableWithStringKey updatable)
        {
            EEDbContextImplementations.OnUpdating(updatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on deletion.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now, string? id)
    {
        base.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
        {
            EEDbContextImplementations.OnDeleting(deletable, id);
        }
    }

    /// <inheritdoc/>
    protected override void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        base.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
        {
            EEDbContextImplementations.OnRestoring(deletable);
        }
    }

    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    protected abstract string? GetUserId();

    /// <inheritdoc cref="DbContext.SaveChanges"/>
    public override int SaveChanges()
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges();
    }

    /// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

/// <summary>
/// The wrapper class of <see cref="DbContext"/> to activate
/// <see cref="IEntityCreationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntityUpdationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordableWithStringKey{TUser}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEDbContextWithStringKey"/>.
/// </summary>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public abstract class EEDbContextWithStringKey<TUser> : EEDbContextWithStringKey
    where TUser : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContextWithStringKey{TUser}" /> class.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will be called to configure the database (and other options) to be used for this context.
    /// </summary>
    public EEDbContextWithStringKey() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContextWithStringKey{TUser}" /> class using the specified options.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will still be called to allow further configuration of the options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEDbContextWithStringKey(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
    /// </summary>
    public virtual DbSet<TUser> Users { get; set; } = default!;

    /// <inheritdoc cref="DbContext.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreatingWithStringKey<TUser>(modelBuilder);
    }
}
