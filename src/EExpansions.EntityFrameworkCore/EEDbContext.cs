namespace EExpansions.EntityFrameworkCore;

using Internal;

/// <summary>
/// The wrapper class of <see cref="DbContext"/> to activate
/// <see cref="IEntityCreationRecordable"/>,
/// <see cref="IEntityUpdationRecordable"/> and
/// <see cref="IEntitySoftDeletionRecordable"/>.
/// </summary>
[HasEEDbContextLogics]
public abstract class EEDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext" /> class.
    /// </summary>
    public EEDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEDbContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc cref="DbContext.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnCreating(entity, now);
    }

    /// <summary>
    /// Configures the entity on updation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnUpdating(entity, now);
    }

    /// <summary>
    /// Configures the entity on deletion.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now)
    {
        EEDbContextImplementations.OnDeleting(entity, now);
    }

    /// <summary>
    /// Configures the entity on restoring deleted entity.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    protected virtual void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        EEDbContextImplementations.OnRestoring(entity);
    }

    #region PrimitiveSaveChanges

    private protected int PrimitiveSaveChanges()
    {
        return base.SaveChanges();
    }

    private protected int PrimitiveSaveChanges(bool acceptAllChangesOnSuccess)
    {
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private protected Task<int> PrimitiveSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    private protected Task<int> PrimitiveSaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    #endregion

    /// <inheritdoc cref="DbContext.SaveChanges"/>
    public override int SaveChanges()
    {
        var now = DateTimeOffset.UtcNow;
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now);
        return PrimitiveSaveChanges();
    }

    /// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var now = DateTimeOffset.UtcNow;
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now);
        return PrimitiveSaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now);
        return PrimitiveSaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTimeOffset.UtcNow;
        EEDbContextImplementations.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now);
        return PrimitiveSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

/// <summary>
/// The wrapper class of <see cref="DbContext"/> to activate
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEDbContext"/>.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public abstract class EEDbContext<TUserForeignKey> : EEDbContext, IUserIdGettable<TUserForeignKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TUserForeignKey}" /> class.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will be called to configure the database (and other options) to be used for this context.
    /// </summary>
    public EEDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TUserForeignKey}" /> class using the specified options.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will still be called to allow further configuration of the options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEDbContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc cref="DbContext.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreating<TUserForeignKey>(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnCreating(entity, now);

        if (entity is IEntityCreationRecordable<TUserForeignKey> creatable)
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
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordable<TUserForeignKey> updatable)
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
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        base.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordable<TUserForeignKey> deletable)
        {
            EEDbContextImplementations.OnDeleting(deletable, id);
        }
    }

    /// <inheritdoc/>
    protected override void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        base.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordable<TUserForeignKey> deletable)
        {
            EEDbContextImplementations.OnRestoring(deletable);
        }
    }

    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    public abstract TUserForeignKey GetUserId();

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
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEDbContext{TUserForeignKey}"/>.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public abstract class EEDbContext<TUserForeignKey, TUser> : EEDbContext<TUserForeignKey>
    where TUser : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TUserForeignKey, TUser}" /> class.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will be called to configure the database (and other options) to be used for this context.
    /// </summary>
    public EEDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEDbContext{TUserForeignKey, TUser}" /> class using the specified options.
    /// The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
    /// method will still be called to allow further configuration of the options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
    /// </summary>
    public virtual DbSet<TUser> Users { get; set; } = default!;

    /// <inheritdoc cref="DbContext.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreating<TUserForeignKey, TUser>(modelBuilder);
    }
}
