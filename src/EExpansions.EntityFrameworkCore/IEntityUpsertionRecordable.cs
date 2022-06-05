namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable"/> and <see cref="IEntityUpdationRecordable"/>.
/// There are no additional definitions.
/// </summary>
public interface IEntityUpsertionRecordable : IEntityCreationRecordable, IEntityUpdationRecordable
{ }

#region for struct

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TKey}"/> and <see cref="IEntityUpdationRecordable{TKey}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityUpsertionRecordable<TKey> : IEntityCreationRecordable<TKey>, IEntityUpdationRecordable<TKey>
    where TKey : struct, IEquatable<TKey>
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TKey, TUser}"/> and <see cref="IEntityUpdationRecordable{TKey, TUser}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityUpsertionRecordable<TKey, TUser> : IEntityCreationRecordable<TKey, TUser>, IEntityUpdationRecordable<TKey, TUser>
    where TKey : struct, IEquatable<TKey>
    where TUser : class
{ }

#endregion

#region for string

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordableWithStringKey"/> and <see cref="IEntityCreationRecordableWithStringKey"/>.
/// There are no additional definitions.
/// </summary>
public interface IEntityUpsertionRecordableWithStringKey : IEntityCreationRecordableWithStringKey, IEntityUpdationRecordableWithStringKey
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordableWithStringKey{TUser}"/> and <see cref="IEntityCreationRecordableWithStringKey{TUser}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityUpsertionRecordableWithStringKey<TUser> : IEntityCreationRecordableWithStringKey<TUser>, IEntityUpdationRecordableWithStringKey<TUser>
    where TUser : class
{ }

#endregion
