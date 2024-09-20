// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Authorization;

namespace Voting.Lib.Iam.Authorization;

internal class AnyPermissionRequirement : IAuthorizationRequirement
{
    public AnyPermissionRequirement(string[] permissions)
    {
        Permissions = permissions;
    }

    public string[] Permissions { get; }
}
