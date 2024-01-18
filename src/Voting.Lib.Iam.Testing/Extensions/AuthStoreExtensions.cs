// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Store;

/// <summary>
/// Extensions for <see cref="IAuthStore"/>.
/// </summary>
public static class AuthStoreExtensions
{
    /// <summary>
    /// Set the current authentication values. Only call this method once per request/scope.
    /// </summary>
    /// <param name="authStore">The auth store.</param>
    /// <param name="subjectAccessToken">The access token of the user.</param>
    /// <param name="userId">The current user ID.</param>
    /// <param name="tenantId">The current tenant ID.</param>
    /// <param name="roles">The roles of the current user.</param>
    /// <param name="permissions">The permissions of the current user.</param>
    public static void SetValues(this IAuthStore authStore, string subjectAccessToken, string userId, string tenantId, IEnumerable<string>? roles, IEnumerable<string>? permissions = null)
    {
        authStore.SetValues(subjectAccessToken, new() { Loginid = userId }, new() { Id = tenantId }, roles, permissions);
    }
}
