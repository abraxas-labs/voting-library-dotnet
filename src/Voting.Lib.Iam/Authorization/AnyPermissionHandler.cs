// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Iam.Authorization;

internal class AnyPermissionHandler : AuthorizationHandler<AnyPermissionRequirement>
{
    private readonly IAuth _auth;

    public AnyPermissionHandler(IAuth auth)
    {
        _auth = auth;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyPermissionRequirement requirement)
    {
        if (_auth.HasAnyPermission(requirement.Permissions))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
