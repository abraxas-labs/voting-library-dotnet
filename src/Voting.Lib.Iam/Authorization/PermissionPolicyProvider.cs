// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.AuthenticationScheme;

namespace Voting.Lib.Iam.Authorization;

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public PermissionPolicyProvider(
        IOptions<AuthorizationOptions> options)
    {
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var requirement = ExtractAuthRequirement(policyName);
        if (requirement == null)
        {
            return _defaultPolicyProvider.GetPolicyAsync(policyName);
        }

        // Implemented according to https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider
        var policy = new AuthorizationPolicyBuilder(SecureConnectDefaults.AuthenticationScheme);
        policy.AddRequirements(requirement);
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

    private IAuthorizationRequirement? ExtractAuthRequirement(string policyName)
    {
        return AuthorizePermissionAttribute.ExtractRequirementFromPolicyName(policyName)
            ?? AuthorizeAnyPermissionAttribute.ExtractRequirementFromPolicyName(policyName);
    }
}
