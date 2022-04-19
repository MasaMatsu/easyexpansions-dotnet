﻿namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// Defines a property for recording the date and time of record updation.
/// </summary>
public interface IEntityUpdationRecordable
{
    /// <summary>
    /// The date and time of record updation.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was updated and the updater.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IEntityUpdationRecordable<TKey> : IEntityUpdationRecordable
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The user ID of the updater.
    /// </summary>
    public TKey? UpdatedBy { get; set; }
}

/// <summary>
/// Defines properties to record the date and time the record was updated and the updater.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public interface IEntityUpdationRecordable<TKey, TUser> : IEntityUpdationRecordable<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class
{
    /// <summary>
    /// The user of the updater.
    /// </summary>
    public TUser? Updater { get; set; }
}