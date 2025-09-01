// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Voting.Lib.Common;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <summary>
/// Default secure connect implementation.
/// </summary>
internal class RoleTokenHandler : IRoleTokenHandler
{
    private const string RoleTokenSubjectEndpointPath = "/token/roles/subject";
    private readonly IOptionsMonitor<SecureConnectOptions> _options;
    private readonly ILogger<RoleTokenHandler> _logger;
    private readonly HttpClient _httpClient;

    public RoleTokenHandler(
        IOptionsMonitor<SecureConnectOptions> options,
        HttpClient httpClient,
        ILogger<RoleTokenHandler> logger)
    {
        _options = options;
        _logger = logger;
        _httpClient = httpClient;
    }

    private SecureConnectOptions Options => _options.Get(SecureConnectDefaults.AuthenticationScheme);

    /// <inheritdoc cref="IRoleTokenHandler"/>
    public async Task<IReadOnlyCollection<string>> GetRoles(string subjectToken, string subject, string tenantId, IEnumerable<string>? apps = null)
    {
        _logger.LogDebug(SecurityLogging.SecurityEventId, "Get roles for tenant {TenantId}.", tenantId);

        if ((Options.ConfigurationManager == null && Options.Configuration == null) || Options.Audience == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Failed getting roles because of missing Configuration and ConfigurationManager or Audience on the JwtBearerOptions");
            return [];
        }

        var hasApps = Options.RoleTokenApps?.Count > 0;
        var appShortcuts = hasApps ? Options.RoleTokenApps! : new List<string> { Options.Audience };
        if (Options.LimitRolesToAppHeaderApps)
        {
            if (apps == null)
            {
                _logger.LogWarning(SecurityLogging.SecurityEventId, "Roles limited to app header and no apps provided");
                return [];
            }

            appShortcuts = appShortcuts.Intersect(apps).ToList();
            _logger.LogInformation(SecurityLogging.SecurityEventId, "Limited apps to {Apps} because of apps header", string.Join(',', appShortcuts));
        }

        _logger.LogDebug(SecurityLogging.SecurityEventId, "Get role token for apps {Apps}", string.Join(',', appShortcuts));
        var request = new RoleTokenRequestModel(subjectToken, appShortcuts);
        var config = await GetConfiguration().ConfigureAwait(false);

        var tokenEndpoint = GetRoleTokenEndpoint(config);
        using var response = await _httpClient.PostAsJsonAsync(tokenEndpoint, request, SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "The download of the role-token returned status-code {Status}", response.StatusCode);
            return [];
        }

        var roleTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (roleTokenResponse == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Role token response was null.");
            return [];
        }

        var token = roleTokenResponse.Token ?? throw new SecurityException("No token received");
        var roleTokenIdentity = await ValidateToken(subject, token, true).ConfigureAwait(false);
        if (roleTokenIdentity == null)
        {
            // already logged.
            return [];
        }

        var roles = roleTokenIdentity.Claims.FirstOrDefault(c => c.Type == Options.RoleClaimName)?.Value;
        if (roles == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "No roles in role token found.");
            return [];
        }

        _logger.LogDebug(SecurityLogging.SecurityEventId, "Roles {Roles} in role token found.", roles);
        var permissions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string[]>>>(
                roles,
                SecureConnectDefaults.JsonOptions);
        if (permissions == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "No permissions in roles found.");
            return [];
        }

        return appShortcuts
            .SelectMany(app => ExtractRoles(permissions, app, tenantId, hasApps))
            .ToList();
    }

    private async Task<ClaimsIdentity?> ValidateToken(
        string subject,
        string token,
        bool refreshOnKeyNotFound)
    {
        try
        {
            // validation code behaviour should equal to Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler.
            var config = await GetConfiguration().ConfigureAwait(false);
            var validationParameters = Options.TokenValidationParameters.Clone();

            var issuers = new[] { config.Issuer };
            validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuers) ?? issuers;
            validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(config.SigningKeys) ?? config.SigningKeys;

            TokenHandler tokenHandler = Options.TokenHandlers.FirstOrDefault() ?? throw new SecurityTokenValidationException("Could not find a token validator from the provided token handlers.");
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            if (!tokenValidationResult.IsValid)
            {
                if (tokenValidationResult.Exception is SecurityTokenSignatureKeyNotFoundException &&
                    refreshOnKeyNotFound &&
                    Options is { RefreshOnIssuerKeyNotFound: true, ConfigurationManager: not null })
                {
                    _logger.LogError(SecurityLogging.SecurityEventId, "Role token key not found, retrying with refreshed keys.");
                    Options.ConfigurationManager.RequestRefresh();
                    return await ValidateToken(subject, token, false);
                }

                throw new SecurityTokenValidationException("Role token validation failed.", tokenValidationResult.Exception);
            }

            if (!subject.Equals((tokenValidationResult.SecurityToken as JsonWebToken)?.Subject))
            {
                throw new SecurityTokenValidationException("Subject of role token is not valid.");
            }

            return tokenValidationResult.ClaimsIdentity;
        }
        catch (Exception e)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, e, "Role token validation failed.");
            return null;
        }
    }

    private async Task<OpenIdConnectConfiguration> GetConfiguration()
    {
        // either Configuration or ConfigurationManager is not null since this is already validated
        return Options.Configuration ?? await Options.ConfigurationManager!.GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
    }

    private IEnumerable<string> ExtractRoles(
        IReadOnlyDictionary<string, Dictionary<string, string[]>> permissions,
        string app,
        string tenantId,
        bool addAppPrefix)
    {
        if (!permissions.TryGetValue(app, out var appPermissions))
        {
            return [];
        }

        if (!appPermissions.TryGetValue(tenantId, out var roles))
        {
            return [];
        }

        return addAppPrefix
            ? roles.Select(r => app + Options.RoleAppsSeparator + r)
            : roles;
    }

    /// <summary>
    /// Returns the endpoint used to exchange a subject token for a role token.
    /// Currently, only <c>/token/roles/subject</c> is used, since all current use cases resolve
    /// the roles of the subject.
    /// An alternate endpoint, <c>/token/roles/actor</c>, exists to resolve the roles
    /// of the actor (if present) in the subject token, but is not currently required
    /// by any use case.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>The endpoint to resolve the role token.</returns>
    private string GetRoleTokenEndpoint(OpenIdConnectConfiguration config)
        => config.Issuer.TrimEnd('/') + RoleTokenSubjectEndpointPath;
}
