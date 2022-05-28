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
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordable<TKey> : IEntitySoftDeletionRecordable
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// The user ID of the deleter.
    /// </summary>
    public TKey? DeletedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was deleted and the deleter.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordable<TKey, TUser> : IEntitySoftDeletionRecordable<TKey>
    where TKey : struct, IEquatable<TKey>
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
/// The wrapper of <see cref="IEntitySoftDeletionRecordable{TKey}"/>.
/// There are no additional definitions.
/// A entity that inherits this interface is never deleted.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordableIgnoringHardDeletion<TKey> : IEntitySoftDeletionRecordable<TKey>, IEntitySoftDeletionRecordableIgnoringHardDeletion
    where TKey : struct, IEquatable<TKey>
{ }

/// <summary>
/// The wrapper of <see cref="IEntitySoftDeletionRecordable{TKey, TUser}"/>.
/// There are no additional definitions.
/// A entity that inherits this interface is never deleted.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntitySoftDeletionRecordableIgnoringHardDeletion<TKey, TUser> : IEntitySoftDeletionRecordable<TKey, TUser>, IEntitySoftDeletionRecordableIgnoringHardDeletion
    where TKey : struct, IEquatable<TKey>
    where TUser : class
{ }
