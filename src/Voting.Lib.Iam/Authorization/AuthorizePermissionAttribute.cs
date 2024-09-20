// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.AspNetCore.Authorization;

namespace Voting.Lib.Iam.Authorization;

/// <summary>
/// An authorization attribute that requires a specific permission.
/// </summary>
public class AuthorizePermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "Permission.";

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizePermissionAttribute"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public AuthorizePermissionAttribute(string permission)
    {
        Policy = PolicyPrefix + permission;
    }

    internal static IAuthorizationRequirement? ExtractRequirementFromPolicyName(string policyName)
    {
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.Ordinal))
        {
            return null;
        }

        var permission = policyName[PolicyPrefix.Length..];
        return new PermissionRequirement(permission);
    }
}
