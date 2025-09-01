// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.TokenHandling.ServiceToken;

internal class ServiceTokenHandler : TokenHandler
{
    private readonly SecureConnectServiceAccountOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public ServiceTokenHandler(
        ILogger<ServiceTokenHandler> logger,
        SecureConnectServiceAccountOptions options,
        TimeProvider timeProvider,
        IHttpClientFactory httpClientFactory)
        : base(timeProvider, logger)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task<(string Token, DateTimeOffset TokenExpiry)> FetchToken(CancellationToken cancellationToken)
    {
        if (_options.ConfigurationManager == null)
        {
            throw new SecurityException(
                $"Could not get security token for service account {_options} because of missing ConfigurationManager on the JwtBearerOptions");
        }

        var config = await _options.ConfigurationManager.GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);

        Logger.LogInformation(SecurityLogging.SecurityEventId, "Requesting new service token");
        using var httpClient = _httpClientFactory.CreateClient(_options.ServiceTokenClientName);
        var requestStarted = TimeProvider.GetUtcNow();
        using var response = await httpClient.PostAsJsonAsync(config.TokenEndpoint, CreateServiceTokenRequestModel(), SecureConnectDefaults.JsonOptions, cancellationToken)
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new SecurityException(
                $"Could not get security token for service account {_options.UserName}. Returned status-code {response.StatusCode}");
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(SecureConnectDefaults.JsonOptions, cancellationToken).ConfigureAwait(false);
        if (tokenResponse == null)
        {
            throw new SecurityException(
                $"Could not deserialize access token response while fetching new token for {_options.UserName}");
        }

        var token = tokenResponse.AccessToken ?? throw new SecurityException("No token received");
        var expiry = requestStarted.AddSeconds(tokenResponse.ExpiresIn);
        var refreshAfter = expiry.Subtract(_options.RefreshBeforeExpiration);

        Logger.LogInformation(SecurityLogging.SecurityEventId, "Got new service token, valid until {Expiry} but we will refresh anytime after {ValidTo}", expiry, refreshAfter);
        return (token, refreshAfter);
    }

    /// <summary>
    /// Creates the <see cref="ServiceTokenRequestModel"/>,
    /// containing the grant_type, client_id, client_secret and scope from the <see cref="SecureConnectOptions"/>.
    /// </summary>
    /// <returns>The <see cref="ServiceTokenRequestModel"/>.</returns>
    private ServiceTokenRequestModel CreateServiceTokenRequestModel()
    {
        var requestModel = new ServiceTokenRequestModel(_options.UserName, _options.Password);
        if (_options.ClientIdScopes == null && _options.Scopes == null)
        {
            return requestModel;
        }

        var scopes = new List<string>(_options.Scopes ?? []);

        if (_options.ClientIdScopes != null)
        {
            scopes.AddRange(GetNamespacePrefixedScopeList(_options.ClientIdScopes));
        }

        requestModel.Scope = scopes;
        return requestModel;
    }

    /// <summary>
    /// Gets a validated list of scopes, where all scope entries are prefixed with the <see cref="SecureConnectDefaults.ScopeNamespacePrefix"/>.
    /// </summary>
    /// <param name="scopes">The scopes passed within the secure connect options.</param>
    /// <returns>A valid enumerable of scopes for the service token request.</returns>
    private IEnumerable<string> GetNamespacePrefixedScopeList(IEnumerable<string> scopes)
    {
        return scopes
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => SecureConnectDefaults.ScopeNamespacePrefix + x);
    }
}
