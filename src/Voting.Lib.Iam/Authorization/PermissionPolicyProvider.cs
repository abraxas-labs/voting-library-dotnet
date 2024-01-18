// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Voting.Lib.Iam.AuthenticationScheme;

namespace Voting.Lib.Iam.Authorization;

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(AuthorizePermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        // Implemented according to https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider
        var permission = policyName[AuthorizePermissionAttribute.PolicyPrefix.Length..];
        var policy = new AuthorizationPolicyBuilder(SecureConnectDefaults.AuthenticationScheme);
        policy.AddRequirements(new PermissionRequirement(permission));
        return Task.FromResult(policy.Build())!;
    }

    /// <inheritdoc />
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        var defaultPolicy = new AuthorizationPolicyBuilder(SecureConnectDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        return Task.FromResult(defaultPolicy);
    }

    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        var fallbackPolicy = new AuthorizationPolicyBuilder(SecureConnectDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        return Task.FromResult(fallbackPolicy)!;
    }
}
