// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Authorization;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Iam.Testing.AuthenticationScheme;

/// <summary>
/// Default mock implementation.
/// </summary>
/// <inheritdoc />
public class AuthenticationHandlerMock : SecureConnectHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationHandlerMock"/> class.
    /// </summary>
    /// <param name="options">The SecureConnect options.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="encoder">The URL encoder.</param>
    /// <param name="roleTokenHandler">The role token handler.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public AuthenticationHandlerMock(
        IOptionsMonitor<SecureConnectOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IRoleTokenHandler roleTokenHandler,
        IServiceProvider serviceProvider)
        : base(options, logger, encoder, roleTokenHandler, serviceProvider)
    {
    }

    /// <summary>
    /// If no authorize header is provided 401 is returned immediately.
    /// Maps the user id by the x-user header, throws 401 if not present.
    /// Maps the tenant by the x-tenant header.
    /// Maps the roles by the x-roles header (only if x-tenant has a value, skipped otherwise).
    /// </summary>
    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(SecureConnectTestDefaults.AuthHeader))
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail("401");
        }

        var identity = new ClaimsIdentity(SecureConnectDefaults.AuthenticationScheme);
        if (!Request.Headers.TryGetValue(SecureConnectTestDefaults.UserHeader, out var userId))
        {
            Response.StatusCode = 401;
            return AuthenticateResult.Fail($"no {SecureConnectTestDefaults.UserHeader} header present");
        }

        identity.AddClaim(
            new Claim(
                ClaimTypes.NameIdentifier,
                userId!));

        var hasTenantId = Request.Headers.TryGetValue(SecureConnectTestDefaults.TenantHeader, out var tenantId);
        if (!hasTenantId && !string.IsNullOrEmpty(Options.DefaultTenantId))
        {
            hasTenantId = true;
            tenantId = Options.DefaultTenantId;
        }

        List<string>? roles = null;
        if (hasTenantId && Request.Headers.TryGetValue(SecureConnectTestDefaults.RolesHeader, out var rolesHeader))
        {
            roles = rolesHeader.Where(x => !string.IsNullOrWhiteSpace(x)).ToList()!;
            identity.AddClaims(rolesHeader.Select(role => new Claim(ClaimTypes.Role, role!)));
        }

        var user = await UserService.GetUser(userId!, true) ?? new() { Loginid = userId! };
        var tenant = await TenantService.GetTenant(tenantId!, true) ?? new() { Id = tenantId! };

        // the usage of the AuthStore and IPermissionProvider is optional
        var permissions = ServiceProvider.GetService<IPermissionProvider>()?.GetPermissionsForRoles(roles ?? Enumerable.Empty<string>());
        ServiceProvider.GetService<IAuthStore>()?.SetValues("mock-token", user, tenant, roles, permissions);

        return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    SecureConnectDefaults.AuthenticationScheme));
    }

    /// <inheritdoc />
    protected override Task InitializeHandlerAsync() => Task.CompletedTask;

    /// <inheritdoc />
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        if (!Request.Headers.ContainsKey(SecureConnectTestDefaults.AuthHeader))
        {
            Response.StatusCode = 401;
        }

        return Task.CompletedTask;
    }
}
