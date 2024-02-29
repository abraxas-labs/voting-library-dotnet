// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Authorization;

/// <summary>
/// Provides the permissions per role.
/// </summary>
public interface IPermissionProvider
{
    /// <summary>
    /// Gets the permissions for a given list of roles.
    /// </summary>
    /// <param name="roles">The roles to get the permissions for.</param>
    /// <returns>The permissions of the role.</returns>
    public IReadOnlyCollection<string> GetPermissionsForRoles(IEnumerable<string> roles);
}
