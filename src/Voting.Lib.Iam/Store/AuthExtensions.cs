// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Voting.Lib.Iam.Exceptions;

namespace Voting.Lib.Iam.Store;

/// <summary>
/// Auth related extensions.
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    /// Checks whether a user has a given role.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="role">The role to look for.</param>
    /// <returns>True if the user has this role in the auth store.</returns>
    public static bool HasRole(this IAuth auth, string role)
        => auth.Roles.Contains(role);

    /// <summary>
    /// Checks whether a user has any of the given role.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="roles">The roles to look for.</param>
    /// <returns>True if the user has any of the given role in the auth store.</returns>
    public static bool HasAnyRole(this IAuth auth, params string[] roles)
        => auth.Roles.Intersect(roles).Any();

    /// <summary>
    /// Checks whether a user has a given role. Throws otherwise.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="role">The role to look for.</param>
    /// <exception cref="ForbiddenException">if the user doesn't have the role.</exception>
    public static void EnsureRole(this IAuth auth, string role)
    {
        if (!auth.HasRole(role))
        {
            throw new ForbiddenException();
        }
    }

    /// <summary>
    /// Checks whether a user has any of the given roles. Throws otherwise.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="roles">The roles to look for.</param>
    /// <exception cref="ForbiddenException">if the user doesn't have any of roles.</exception>
    public static void EnsureAnyRole(this IAuth auth, params string[] roles)
    {
        if (!auth.HasAnyRole(roles))
        {
            throw new ForbiddenException();
        }
    }

    /// <summary>
    /// Checks whether a user has a given permission.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="permission">The permission to look for.</param>
    /// <returns>True if the user has this permission in the auth store.</returns>
    public static bool HasPermission(this IAuth auth, string permission)
        => auth.Permissions.Contains(permission);

    /// <summary>
    /// Checks whether a user has any of the given permissions.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="permissions">The permissions to look for.</param>
    /// <returns>True if the user has any of the given permissions in the auth store.</returns>
    public static bool HasAnyPermission(this IAuth auth, params string[] permissions)
        => auth.Permissions.Intersect(permissions).Any();

    /// <summary>
    /// Checks whether a user has a given permission. Throws otherwise.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="permission">The permission to look for.</param>
    /// <exception cref="ForbiddenException">if the user doesn't have the specified permission.</exception>
    public static void EnsurePermission(this IAuth auth, string permission)
    {
        if (!auth.HasPermission(permission))
        {
            throw new ForbiddenException();
        }
    }
}
