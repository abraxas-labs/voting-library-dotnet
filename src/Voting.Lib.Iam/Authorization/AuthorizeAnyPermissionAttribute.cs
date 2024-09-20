// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Voting.Lib.Iam.Authorization;

/// <summary>
/// An authorization attribute that requires at least one of the specified permissions.
/// </summary>
public class AuthorizeAnyPermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "AnyPermission:";
    private const char PermissionSeparator = '|';

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAnyPermissionAttribute"/> class.
    /// </summary>
    /// <param name="permissions">The permissions, of which at least one must be fulfilled.</param>
    public AuthorizeAnyPermissionAttribute(params string[] permissions)
    {
        if (permissions.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(permissions), "At least one permission is required");
        }

        if (permissions.Any(p => p.Contains(PermissionSeparator)))
        {
            throw new ArgumentException("Permission may not contain the permission separator", nameof(permissions));
        }

        Policy = PolicyPrefix + string.Join(PermissionSeparator, permissions);
    }

    internal static IAuthorizationRequirement? ExtractRequirementFromPolicyName(string policyName)
    {
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.Ordinal))
        {
            return null;
        }

        var permissionsString = policyName[PolicyPrefix.Length..];
        var permissions = permissionsString.Split(PermissionSeparator);
        return new AnyPermissionRequirement(permissions);
    }
}
