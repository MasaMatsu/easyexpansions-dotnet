namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// Defines a property for recording the date and time of record deletion.
/// </summary>
public interface IEntitySoftDeletionRecordable
{
    /// <summary>
    /// <see langword="true"/> if the entity is deleted; otherwise <see langword="false"/>.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The date and time of record deletion.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was deleted and the deleter.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordable<TUserForeignKey> : IEntitySoftDeletionRecordable
{
    /// <summary>
    /// The user ID of the deleter.
    /// </summary>
    public TUserForeignKey DeletedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was deleted and the deleter.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntitySoftDeletionRecordable<TUserForeignKey, TUser> : IEntitySoftDeletionRecordable<TUserForeignKey>
    where TUser : class
{
    /// <summary>
    /// The user ID of the deleter.
    /// </summary>
    public TUser? Deleter { get; set; }
}

/// <summary>
/// The wrapper of <see cref="IEntitySoftDeletionRecordable"/>.
/// There are no additional definitions.
/// A entity that inherits this interface is never deleted.
/// </summary>
public interface IEntitySoftDeletionRecordableIgnoringHardDeletion : IEntitySoftDeletionRecordable
{ }

/// <summary>
/// The wrapper of <see cref="IEntitySoftDeletionRecordable{TUserForeignKey}"/>.
/// There are no additional definitions.
/// A entity that inherits this interface is never deleted.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordableIgnoringHardDeletion<TUserForeignKey> : IEntitySoftDeletionRecordable<TUserForeignKey>, IEntitySoftDeletionRecordableIgnoringHardDeletion
{ }

/// <summary>
/// The wrapper of <see cref="IEntitySoftDeletionRecordable{TUserForeignKey, TUser}"/>.
/// There are no additional definitions.
/// A entity that inherits this interface is never deleted.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntitySoftDeletionRecordableIgnoringHardDeletion<TUserForeignKey, TUser> : IEntitySoftDeletionRecordable<TUserForeignKey, TUser>, IEntitySoftDeletionRecordableIgnoringHardDeletion
    where TUser : class
{ }
