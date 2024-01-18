// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Authorization;

namespace Voting.Lib.Iam.Authorization;

/// <summary>
/// An authorization attribute that requires a specific permission.
/// </summary>
public class AuthorizePermissionAttribute : AuthorizeAttribute
{
    internal const string PolicyPrefix = "Permission.";

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizePermissionAttribute"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public AuthorizePermissionAttribute(string permission)
    {
        Policy = PolicyPrefix + permission;
    }
}
