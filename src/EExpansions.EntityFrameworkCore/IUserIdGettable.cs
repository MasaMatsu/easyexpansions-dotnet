namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// Defines a method to get user ID.
/// </summary>
/// <typeparam name="TUserForeignKey">The type of the key that is used for user ID.</typeparam>
public interface IUserIdGettable<TUserForeignKey>
{
    /// <summary>
    /// Returns the id of the editing user.
    /// </summary>
    /// <returns>The id of the editing user.</returns>
    TUserForeignKey GetUserId();
}
