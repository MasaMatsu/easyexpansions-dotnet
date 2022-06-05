using System;

namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// Defines a method to get user ID.
/// </summary>
/// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
public interface IUserIdGettable<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    TKey? GetUserId();
}

/// <summary>
/// Defines a method to get user ID.
/// </summary>
public interface IUserIdGettableWithStringKey
{
    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    string? GetUserId();
}
