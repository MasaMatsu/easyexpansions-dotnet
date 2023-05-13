namespace EExpansions.AspNetCore.Identity;

#pragma warning disable EF1001

using EntityFrameworkCore.Internal;

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext"/> to activate
/// <see cref="IEntityCreationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>
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
/// <see cref="IEntityCreationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>
/// and their derived interfaces.
/// </summary>
/// <typeparam name="TUser">The type of the user objects.</typeparam>
[HasEEDbContextLogics]
public abstract class EEIdentityDbContext<TUser> : EEIdentityDbContext<TUser, IdentityRole<string>, string, string?>
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
}

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext{TUser, TRole, TKey}"/> to activate
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>
/// and their derived interfaces.
/// This class inherits <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserForeignKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}"/>.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TRole">The type of role objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
/// <typeparam name="TUserForeignKey">The type of the foreign key for users.</typeparam>
public abstract class EEIdentityDbContext<TUser, TRole, TKey, TUserForeignKey> : EEIdentityDbContext<TUser, TRole, TKey, TUserForeignKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserForeignKey}" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserForeignKey}" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }
}

/// <summary>
/// The wrapper class of <see cref="IdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}"/> to activate
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>,
/// <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>
/// and their derived interfaces.
/// </summary>
/// <typeparam name="TUser">The type of user objects.</typeparam>
/// <typeparam name="TRole">The type of role objects.</typeparam>
/// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
/// <typeparam name="TUserForeignKey">The type of the foreign key for users.</typeparam>
/// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
/// <typeparam name="TUserRole">The type of the user role object.</typeparam>
/// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
/// <typeparam name="TRoleClaim">The type of the role claim object.</typeparam>
/// <typeparam name="TUserToken">The type of the user token object.</typeparam>
[HasEEDbContextLogics]
public abstract class EEIdentityDbContext<TUser, TRole, TKey, TUserForeignKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IUserIdGettable<TUserForeignKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>
    where TUserRole : IdentityUserRole<TKey>
    where TUserLogin : IdentityUserLogin<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserToken : IdentityUserToken<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserForeignKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}" /> class.
    /// </summary>
    protected EEIdentityDbContext() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EEIdentityDbContext{TUser, TRole, TKey, TUserForeignKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}" /> class using the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public EEIdentityDbContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc cref="IdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}.OnModelCreating(ModelBuilder)"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreating(modelBuilder);
        EEDbContextImplementations.OnModelCreating<TUserForeignKey>(modelBuilder);
        EEDbContextImplementations.OnModelCreating<TUserForeignKey, TUser>(modelBuilder);
    }

    /// <summary>
    /// Configures the entity on creation.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <param name="now"><see cref="DateTimeOffset.UtcNow"/>.</param>
    /// <param name="id">The user id.</param>
    protected virtual void OnCreating(IEntityCreationRecordable entity, DateTimeOffset now, TUserForeignKey id)
    {
        EEDbContextImplementations.OnCreating(entity, now);

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
        EEDbContextImplementations.OnUpdating(entity, now);

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
        EEDbContextImplementations.OnDeleting(entity, now);

        if (entity is IEntitySoftDeletionRecordable<TUserForeignKey> deletable)
        {
            EEDbContextImplementations.OnDeleting(deletable, id);
        }
    }

    /// <summary>
    /// Configures the entity on restoring deleted entity.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    protected virtual void OnRestoring(IEntitySoftDeletionRecordable entity)
    {
        EEDbContextImplementations.OnRestoring(entity);

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

    /// <inheritdoc cref="DbContext.SaveChanges()"/>
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
