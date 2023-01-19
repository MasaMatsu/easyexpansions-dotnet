namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable"/> and <see cref="IEntityUpdationRecordable"/>.
/// There are no additional definitions.
/// </summary>
public interface IEntityUpsertionRecordable : IEntityCreationRecordable, IEntityUpdationRecordable
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TUserForeignKey}"/> and <see cref="IEntityUpdationRecordable{TUserForeignKey}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityUpsertionRecordable<TUserForeignKey> : IEntityCreationRecordable<TUserForeignKey>, IEntityUpdationRecordable<TUserForeignKey>
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TUserForeignKey, TUser}"/> and <see cref="IEntityUpdationRecordable{TUserForeignKey, TUser}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityUpsertionRecordable<TUserForeignKey, TUser> : IEntityCreationRecordable<TUserForeignKey, TUser>, IEntityUpdationRecordable<TUserForeignKey, TUser>
    where TUser : class
{ }
