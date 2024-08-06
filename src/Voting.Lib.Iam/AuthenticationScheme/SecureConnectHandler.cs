// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Voting.Lib.Common;
using Voting.Lib.Common.Cache;
using Voting.Lib.Iam.Authorization;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <summary>
/// <see cref="AuthenticationHandler{TOptions}"/> for SecureConnect.
/// </summary>
public class SecureConnectHandler : AuthenticationHandler<SecureConnectOptions>
{
    private const string AuthTimeClaimName = "auth_time";
    private readonly ICache<User> _userCache;
    private readonly ICache<Tenant> _tenantCache;
    private readonly ICache<UserRoles> _rolesCache;

    private string? _tenant;
    private string? _subjectToken;
    private IReadOnlyCollection<string>? _apps;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureConnectHandler"/> class.
    /// </summary>
    /// <param name="options">The SecureConnect options.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="encoder">The URL encoder.</param>
    /// <param name="roleTokenHandler">The SecureConnect role token handler.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public SecureConnectHandler(
        IOptionsMonitor<SecureConnectOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IRoleTokenHandler roleTokenHandler,
        IServiceProvider serviceProvider)
        : base(
            options,
            logger,
            encoder)
    {
        RoleTokenHandler = roleTokenHandler;
        ServiceProvider = serviceProvider;
        JwtBearerHandler = new JwtBearerHandler(
            options,
            logger,
            encoder);

        UserService = serviceProvider.GetRequiredService<IUserService>();
        TenantService = serviceProvider.GetRequiredService<ITenantService>();
        _userCache = serviceProvider.GetRequiredService<ICache<User>>();
        _tenantCache = serviceProvider.GetRequiredService<ICache<Tenant>>();
        _rolesCache = serviceProvider.GetRequiredService<ICache<UserRoles>>();
    }

    internal SecureConnectHandler(
        IOptionsMonitor<SecureConnectOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IRoleTokenHandler roleTokenHandler,
        JwtBearerHandler bearerHandler,
        IServiceProvider serviceProvider)
        : base(
            options,
            logger,
            encoder)
    {
        RoleTokenHandler = roleTokenHandler;
        ServiceProvider = serviceProvider;
        JwtBearerHandler = bearerHandler;

        UserService = serviceProvider.GetRequiredService<IUserService>();
        TenantService = serviceProvider.GetRequiredService<ITenantService>();
        _userCache = serviceProvider.GetRequiredService<ICache<User>>();
        _tenantCache = serviceProvider.GetRequiredService<ICache<Tenant>>();
        _rolesCache = serviceProvider.GetRequiredService<ICache<UserRoles>>();
    }

    /// <summary>
    /// Gets the JWT bearer handler.
    /// </summary>
    protected JwtBearerHandler JwtBearerHandler { get; }

    /// <summary>
    /// Gets the role token handler.
    /// </summary>
    protected IRoleTokenHandler RoleTokenHandler { get; }

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the user service.
    /// </summary>
    protected IUserService UserService { get; }

    /// <summary>
    /// Gets the tenant service.
    /// </summary>
    protected ITenantService TenantService { get; }

    private string? Tenant =>
        _tenant ??= Context.Request.Headers.FirstOrDefault(header => string.Equals(header.Key, Options.TenantHeaderName, StringComparison.OrdinalIgnoreCase))
            .Value
            .FirstOrDefault()
            ?? Options.DefaultTenantId;

    private IReadOnlyCollection<string> Apps =>
        _apps ??= Context.Request.Headers.FirstOrDefault(header => string.Equals(header.Key, Options.AppHeaderName, StringComparison.OrdinalIgnoreCase)).Value;

    private string SubjectToken => _subjectToken ??=
        Context.Request.Headers
            .FirstOrDefault(header => header.Key == HeaderNames.Authorization)
            .Value
            .FirstOrDefault()
            ?.Replace(SecureConnectDefaults.BearerScheme, string.Empty, StringComparison.InvariantCulture)
            .Trim()
        ?? string.Empty;

    /// <summary>
    /// Handle the authentication for the scheme.
    /// After the JwtBearerHandler ran through, the role token is fetched and parsed.
    /// If an <see cref="IAuthStore"/> implementation is available and the user has roles assigned, the values are set.
    /// </summary>
    /// <returns>Task with the authentication result.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Logger.LogTrace(SecurityLogging.SecurityEventId, "HandleAuthenticate");
        var result = await JwtBearerHandler.AuthenticateAsync().ConfigureAwait(false);
        Logger.LogDebug(SecurityLogging.SecurityEventId, "JWT Bearer (sub authenticate) returned '{result.Succeeded}'", result.Succeeded);

        if (!result.Succeeded)
        {
            Logger.LogDebug(SecurityLogging.SecurityEventId, "Authentication was not successful");
            return result;
        }

        if (!Options.FetchRoleToken
            || string.IsNullOrWhiteSpace(Tenant)
            || result.Principal?.Identity is not ClaimsIdentity roleIdentity)
        {
            return result;
        }

        var sub = roleIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (sub == null)
        {
            return AuthenticateResult.Fail("No sub present in token");
        }

        Logger.LogDebug(SecurityLogging.SecurityEventId, "Try get role token from cache");
        var key = BuildRolesCacheKey(roleIdentity.Claims);
        var userRoles = _rolesCache.Get(key);
        if (userRoles == null)
        {
            Logger.LogDebug(SecurityLogging.SecurityEventId, "Fetching role token for user from IAM");
            var roles = await RoleTokenHandler.GetRoles(SubjectToken, Tenant, Apps).ConfigureAwait(false);
            userRoles = new UserRoles(roles);
            if (roles.Count > 0)
            {
                _rolesCache.Set(key, userRoles);
            }
        }

        if (Options.AnyRoleRequired && userRoles.Roles.Count == 0)
        {
            return AuthenticateResult.Fail("No roles present in role token");
        }

        roleIdentity.AddClaims(userRoles.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // The usage of the AuthStore is optional
        var authStore = ServiceProvider.GetService<IAuthStore>();

        if (authStore == null)
        {
            return result;
        }

        var user = await _userCache.GetOrAdd(sub, async () =>
            await UserService.GetUser(sub, true).ConfigureAwait(false) ?? new() { Loginid = sub }).ConfigureAwait(false);

        var tenant = await _tenantCache.GetOrAdd(Tenant, async () =>
            await TenantService.GetTenant(Tenant, true).ConfigureAwait(false) ?? new() { Id = Tenant }).ConfigureAwait(false);

        // The usage of the IPermissionProvider is optional
        var permissionProvider = ServiceProvider.GetService<IPermissionProvider>();
        var permissions = permissionProvider?.GetPermissionsForRoles(userRoles.Roles);

        authStore.SetValues(SubjectToken, user, tenant, userRoles.Roles, permissions);

        return result;
    }

    /// <summary>
    /// Initialize the handler and the jwt handler.
    /// </summary>
    /// <returns>Task when finished.</returns>
    protected override async Task InitializeHandlerAsync()
    {
        Logger.LogTrace(SecurityLogging.SecurityEventId, "InitializeHandler");
        await JwtBearerHandler.InitializeAsync(Scheme, Context).ConfigureAwait(false);
        await base.InitializeHandlerAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Handle the challenge through jwt handler.
    /// </summary>
    /// <param name="properties">Properties.</param>
    /// <returns>Task for the challenge.</returns>
    protected override Task HandleChallengeAsync(AuthenticationProperties properties) =>
        JwtBearerHandler.ChallengeAsync(properties);

    /// <summary>
    /// Builds the roles cache key from subject token, tenant and apps.
    /// </summary>
    /// <returns>The cache key.</returns>
    private string BuildRolesCacheKey(IEnumerable<Claim> claims)
    {
        // include the subject (=the user) in the cache key
        var sub = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        // include the auth_time in the cache key
        // the auth time is the time when the user has authenticated
        // this is untouched when rolling the access tokens by using refresh flows
        // therefore the roles are cached during refresh flows, but the cache is invalidated
        // when the user authenticates again (explicitly logs out and logs in again).
        var authTime = claims.FirstOrDefault(x => x.Type == AuthTimeClaimName)?.Value;
        return HashUtil.GetSHA256Hash(string.Join(
            '-',
            sub,
            authTime,
            Tenant ?? string.Empty,
            string.Join('-', Apps)));
    }
}
