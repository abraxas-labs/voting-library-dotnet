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
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Limited apps to {Apps} because of apps header", string.Join(',', appShortcuts));
        }

        _logger.LogDebug(SecurityLogging.SecurityEventId, "Get role token for apps {Apps}", string.Join(',', appShortcuts));
        var request = new RoleTokenRequestModel(subjectToken, appShortcuts);
        var config = await GetConfiguration().ConfigureAwait(false);

        var tokenEndpoint = GetRoleTokenEndpoint(config);
        using var response = await _httpClient.PostAsJsonAsync(tokenEndpoint, request, SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(SecurityLogging.SecurityEventId, "The download of the role-token returned status-code {Status}", response.StatusCode);
            return [];
        }

        var roleTokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (roleTokenResponse == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Role token response was null.");
            return [];
        }

        var token = roleTokenResponse.Token ?? throw new SecurityException("No token received");
        var roleTokenIdentity = await ValidateToken(subject, token).ConfigureAwait(false);
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

    /// <summary>
    /// Validates a JWT token by checking its signature, lifetime, issuer, and subject.
    /// </summary>
    /// <remarks>
    /// The validation behavior is intended to be equivalent to
    /// <see cref="Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler" /> in ASP.NET Core.
    ///
    /// <para>
    /// <b>Signing key refresh (<see href="https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/8.14.0/src/Microsoft.IdentityModel.Protocols/Configuration/ConfigurationManager.cs#L451-L468">
    /// IdentityModel v8.* non-blocking behavior</see>)</b>
    /// </para>
    /// <para>
    /// If validation fails with <see cref="SecurityTokenSignatureKeyNotFoundException" />, for example after an
    /// identity provider signing key rotation, this method calls
    /// <see cref="Microsoft.IdentityModel.Protocols.IConfigurationManager{T}.RequestRefresh" /> to trigger an
    /// asynchronous JWKS refresh. This manual refresh behaves equivalently to the internal refresh mechanism used by
    /// <see cref="Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler"/>.
    /// </para>
    /// <para>
    /// In IdentityModel v8.*, <see cref="Microsoft.IdentityModel.Protocols.IConfigurationManager{T}.RequestRefresh" />
    /// schedules a background Task in <c>RequestRefreshBackgroundThread()</c> to fetch updated signing keys from the
    /// JWKS endpoint. The call returns immediately and does not block the current validation attempt. Until the refresh
    /// completes, <see cref="Microsoft.IdentityModel.Protocols.IConfigurationManager{T}.GetConfigurationAsync(CancellationToken)" />
    /// continues to return the previously cached keys.
    /// </para>
    /// <para>
    /// As a result, the current call always fails when a key-not-found condition is encountered. Validation succeeds on a
    /// subsequent call once the background refresh has completed and the updated keys are available.
    /// </para>
    /// <para>
    /// <b>Note:</b> The very first call to
    /// <see cref="Microsoft.IdentityModel.Protocols.IConfigurationManager{T}.GetConfigurationAsync(CancellationToken)" />
    /// always blocks, because no cached configuration exists and the initial metadata and JWKS retrieval must complete
    /// before token validation can occur.
    /// </para>
    /// </remarks>
    /// <param name="subject">The expected <c>sub</c> (subject) claim value of the JWT token.</param>
    /// <param name="token">The raw JWT token to validate.</param>
    /// <returns>
    /// The validated <see cref="ClaimsIdentity" />, or <see langword="null" /> if validation fails.
    /// </returns>
    private async Task<ClaimsIdentity?> ValidateToken(
        string subject,
        string token)
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
                    Options is { RefreshOnIssuerKeyNotFound: true, ConfigurationManager: not null })
                {
                    _logger.LogWarning(SecurityLogging.SecurityEventId, "Role token key not found, request an asynchronous key refresh.");
                    Options.ConfigurationManager.RequestRefresh();
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
