namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// Defines a property for recording the date and time of record creation.
/// </summary>
public interface IEntityCreationRecordable
{
    /// <summary>
    /// The date and time of record creation.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityCreationRecordable<TUserForeignKey> : IEntityCreationRecordable
{
    /// <summary>
    /// The user ID of the creator.
    /// </summary>
    TUserForeignKey CreatedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityCreationRecordable<TUserForeignKey, TUser> : IEntityCreationRecordable<TUserForeignKey>
    where TUser : class
{
    /// <summary>
    /// The user of the creator.
    /// </summary>
    TUser? Creator { get; set; }
}
