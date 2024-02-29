// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// Represents the user roles.
/// </summary>
public class UserRoles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoles"/> class.
    /// </summary>
    /// <param name="roles">The users roles collection.</param>
    public UserRoles(IReadOnlyCollection<string> roles)
    {
        Roles = roles;
    }

    /// <summary>
    /// Gets or sets the user roles collection.
    /// </summary>
    public IReadOnlyCollection<string> Roles { get; set; }
}
