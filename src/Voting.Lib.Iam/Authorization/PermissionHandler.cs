// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Iam.Authorization;

internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAuth _auth;

    public PermissionHandler(IAuth auth)
    {
        _auth = auth;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (_auth.HasPermission(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
