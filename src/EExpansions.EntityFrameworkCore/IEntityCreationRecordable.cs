﻿namespace EExpansions.EntityFrameworkCore;

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

#region for struct

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityCreationRecordable<TKey> : IEntityCreationRecordable
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// The user ID of the creator.
    /// </summary>
    TKey? CreatedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityCreationRecordable<TKey, TUser> : IEntityCreationRecordable<TKey>
    where TKey : struct, IEquatable<TKey>
    where TUser : class
{
    /// <summary>
    /// The user of the creator.
    /// </summary>
    TUser? Creator { get; set; }
}

#endregion

#region for string

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
public interface IEntityCreationRecordableWithStringKey : IEntityCreationRecordable
{
    /// <summary>
    /// The user ID of the creator.
    /// </summary>
    string? CreatedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was created and the creator.
/// </summary>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityCreationRecordableWithStringKey<TUser> : IEntityCreationRecordableWithStringKey
    where TUser : class
{
    /// <summary>
    /// The user of the creator.
    /// </summary>
    TUser? Creator { get; set; }
}

#endregion