﻿namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable"/> and <see cref="IEntityUpdationRecordable"/>.
/// There are no additional definitions.
/// </summary>
public interface IEntityUpsertionRecordable : IEntityCreationRecordable, IEntityUpdationRecordable
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TKey}"/> and <see cref="IEntityUpdationRecordable{TKey}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityUpsertionRecordable<TKey> : IEntityCreationRecordable<TKey>, IEntityUpdationRecordable<TKey>
    where TKey : IEquatable<TKey>
{ }

/// <summary>
/// The wrapper of <see cref="IEntityCreationRecordable{TKey, TUser}"/> and <see cref="IEntityUpdationRecordable{TKey, TUser}"/>.
/// There are no additional definitions.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityUpsertionRecordable<TKey, TUser> : IEntityCreationRecordable<TKey, TUser>, IEntityUpdationRecordable<TKey, TUser>
    where TKey : IEquatable<TKey>
    where TUser : class
{ }