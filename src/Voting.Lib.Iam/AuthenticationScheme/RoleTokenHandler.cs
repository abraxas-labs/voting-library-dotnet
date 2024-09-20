// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
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
    public async Task<IReadOnlyCollection<string>> GetRoles(string subjectToken, string tenantId, IEnumerable<string>? apps = null)
    {
        _logger.LogDebug(SecurityLogging.SecurityEventId, "Get roles for tenant {TenantId}.", tenantId);

        if ((Options.ConfigurationManager == null && Options.Configuration == null) || Options.Audience == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Failed getting roles because of missing Configuration and ConfigurationManager or Audience on the JwtBearerOptions");
            return Array.Empty<string>();
        }

        var hasApps = Options.RoleTokenApps?.Count > 0;
        var appShortcuts = hasApps ? Options.RoleTokenApps! : new List<string> { Options.Audience };
        if (Options.LimitRolesToAppHeaderApps)
        {
            if (apps == null)
            {
                _logger.LogWarning(SecurityLogging.SecurityEventId, "Roles limited to app header and no apps provided");
                return Array.Empty<string>();
            }

            appShortcuts = appShortcuts.Intersect(apps).ToList();
            _logger.LogInformation(SecurityLogging.SecurityEventId, "Limited apps to {Apps} because of apps header", string.Join(',', appShortcuts));
        }

        _logger.LogDebug(SecurityLogging.SecurityEventId, "Get role token for apps {Apps}", string.Join(',', appShortcuts));
        var request = new RoleTokenRequestModel(subjectToken, appShortcuts);
        var config = await GetConfiguration().ConfigureAwait(false);
        using var response = await _httpClient.PostAsJsonAsync(config.TokenEndpoint, request, SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "The download of the role-token returned status-code {Status}", response.StatusCode);
            return Array.Empty<string>();
        }

        var roleTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (roleTokenResponse == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Role token response was null.");
            return Array.Empty<string>();
        }

        var roleTokenIdentity = await ValidateToken(roleTokenResponse.AccessToken, true, subjectToken).ConfigureAwait(false);
        if (roleTokenIdentity == null)
        {
            // already logged.
            return Array.Empty<string>();
        }

        var roles = roleTokenIdentity.Claims.FirstOrDefault(c => c.Type == Options.RoleClaimName)?.Value;
        if (roles == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "No roles in role token found.");
            return Array.Empty<string>();
        }

        _logger.LogDebug(SecurityLogging.SecurityEventId, "Roles {Roles} in role token found.", roles);
        var permissions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string[]>>>(
                roles,
                SecureConnectDefaults.JsonOptions);
        if (permissions == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "No permissions in roles found.");
            return Array.Empty<string>();
        }

        return appShortcuts
            .SelectMany(app => ExtractRoles(permissions, app, tenantId, hasApps))
            .ToList();
    }

    private async Task<ClaimsIdentity?> ValidateToken(string token, bool refreshOnKeyNotFound, string subjectToken)
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
                    refreshOnKeyNotFound && Options.RefreshOnIssuerKeyNotFound &&
                    Options.ConfigurationManager != null)
                {
                    _logger.LogError(SecurityLogging.SecurityEventId, "Role token key not found, retrying with refreshed keys.");
                    Options.ConfigurationManager.RequestRefresh();
                    return await ValidateToken(token, false, subjectToken);
                }

                throw new SecurityTokenValidationException("Role token validation failed.", tokenValidationResult.Exception);
            }

            var subject = ExtractSubject(subjectToken);

            if (subject?.Equals((tokenValidationResult.SecurityToken as JsonWebToken)?.Subject) != true)
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
        return Options.Configuration ?? await Options.ConfigurationManager!.GetConfigurationAsync(default).ConfigureAwait(false);
    }

    private IEnumerable<string> ExtractRoles(
        IReadOnlyDictionary<string, Dictionary<string, string[]>> permissions,
        string app,
        string tenantId,
        bool addAppPrefix)
    {
        if (!permissions.TryGetValue(app, out var appPermissions))
        {
            return Enumerable.Empty<string>();
        }

        if (!appPermissions.TryGetValue(tenantId, out var roles))
        {
            return Enumerable.Empty<string>();
        }

        return addAppPrefix
            ? roles.Select(r => app + Options.RoleAppsSeparator + r)
            : roles;
    }

    private string ExtractSubject(string subjectToken)
    {
        var jwtSubjectToken = new JwtSecurityTokenHandler().ReadJwtToken(subjectToken);
        var subjectTokenType = jwtSubjectToken.Claims.FirstOrDefault(e => e.Type.Equals(SecureConnectTokenClaimTypes.TokenType))?.Value;
        if (string.IsNullOrWhiteSpace(subjectTokenType))
        {
            throw new SecurityTokenValidationException("Subject token has no token type defined");
        }

        string? subject = null;
        if (subjectTokenType == SecureConnectTokenTypes.AccessToken)
        {
            subject = jwtSubjectToken.Subject;
        }
        else if (subjectTokenType == SecureConnectTokenTypes.OnBehalfOfToken)
        {
            var actor = jwtSubjectToken.Claims.FirstOrDefault(t => t.Type == SecureConnectTokenClaimTypes.Actor)?.Value;
            subject = actor == null ? null : JsonDocument.Parse(actor).RootElement.GetProperty("sub").GetString();
        }
        else
        {
            throw new SecurityTokenValidationException("Unknown subject token type");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new SecurityTokenValidationException("Subject claim for subject token is not set");
        }

        return subject;
    }
}
