namespace EExpansions.AspNetCore.Identity;

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext"/> to activate
/// <see cref="IEntityCreationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntityUpdationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordableWithStringKey{TUser}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEIdentityDbContext{TUser}"/>.
/// </summary>
public abstract class EEIdentityDbContext : EEIdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }
}

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext{TUser}"/> to activate
/// <see cref="IEntityCreationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntityUpdationRecordableWithStringKey{TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordableWithStringKey{TUser}"/>
/// and their derived interfaces.
/// </summary>
/// <typeparam name="TUser">The type of the user objects.</typeparam>
public abstract class EEIdentityDbContext<TUser> : IdentityDbContext<TUser>
    where TUser : IdentityUser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser}" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser}" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        InternalDbContext.OnModelCreating(modelBuilder);
        InternalDbContext.OnModelCreatingWithStringKey(modelBuilder);
        InternalDbContext.OnModelCreatingWithStringKey<TUser>(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, string? id)
    {
        InternalDbContext.OnCreating(entity, now);

        if (entity is IEntityCreationRecordableWithStringKey creatable)
        {
            InternalDbContext.OnCreating(creatable, id);
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
        InternalDbContext.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordableWithStringKey updatable)
        {
            InternalDbContext.OnUpdating(updatable, id);
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
        InternalDbContext.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
        {
            InternalDbContext.OnDeleting(deletable, id);
        }
    }

    /// <summary>
    /// Configures the entity on restoring deleted entity.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    protected virtual void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        InternalDbContext.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordableWithStringKey deletable)
        {
            InternalDbContext.OnRestoring(deletable);
        }
    }

    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    protected abstract string? GetUserId();

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
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges();
    }

    /// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
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
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext{TUser, TRole, TKey}"/> to activate
/// <see cref="IEntitySoftDeletionRecordable{TKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TKey, TUser}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}"/>.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TRole">The type of role objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
public abstract class EEIdentityDbContext<TUser, TRole, TKey> : EEIdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey}" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey}" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }
}

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}"/> to activate
/// <see cref="IEntitySoftDeletionRecordable{TKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TKey, TUser}"/>
/// and their derived interfaces.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TRole">The type of role objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
/// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
/// <typeparam name="TUserRole">The type of the user role object.</typeparam>
/// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
/// <typeparam name="TRoleClaim">The type of the role claim object.</typeparam>
/// <typeparam name="TUserToken">The type of the user token object.</typeparam>
public abstract class EEIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : struct, IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserRole : IdentityUserRole<TKey>
    where TUserLogin : IdentityUserLogin<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserToken : IdentityUserToken<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc cref="IdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        InternalDbContext.OnModelCreating(modelBuilder);
        InternalDbContext.OnModelCreating<TKey>(modelBuilder);
        InternalDbContext.OnModelCreating<TKey, TUser>(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, TKey? id)
    {
        InternalDbContext.OnCreating(entity, now);

        if (entity is IEntityCreationRecordable<TKey> creatable)
        {
            InternalDbContext.OnCreating(creatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on updation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnUpdating(IEntityUpdationRecordable entity, DateTimeOffset now, TKey? id)
    {
        InternalDbContext.OnUpdating(entity, now);

        if (entity is IEntityUpdationRecordable<TKey> updatable)
        {
            InternalDbContext.OnUpdating(updatable, id);
        }
    }

    /// <summary>
    /// Configures the entity on deletion.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnDeleting(IEntitySoftDeletionRecordable entity, DateTimeOffset now, TKey? id)
    {
        InternalDbContext.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordable<TKey> deletable)
        {
            InternalDbContext.OnDeleting(deletable, id);
        }
    }

    /// <summary>
    /// Configures the entity on restoring deleted entity.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    protected virtual void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        InternalDbContext.OnRestoring(entity);

        if (entity is IEntitySoftDeletionRecordable<TKey> deletable)
        {
            InternalDbContext.OnRestoring(deletable);
        }
    }

    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    protected abstract TKey? GetUserId();

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
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges();
    }

    /// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var id = GetUserId();
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
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
        InternalDbContext.OnSaveChanges(this, OnCreating, OnUpdating, OnDeleting, OnRestoring, now, id);
        return PrimitiveSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
