// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.ServiceTokenHandling;

internal class ServiceTokenHandler : IServiceTokenHandler, IAsyncDisposable
{
    private readonly ILogger<ServiceTokenHandler> _logger;
    private readonly SecureConnectServiceAccountOptions _options;
    private readonly TimeProvider _timeProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly AsyncLock _lock = new();
    private DateTimeOffset? _serviceTokenValidTo;
    private string? _serviceToken;

    public ServiceTokenHandler(
        ILogger<ServiceTokenHandler> logger,
        SecureConnectServiceAccountOptions options,
        TimeProvider timeProvider,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _options = options;
        _timeProvider = timeProvider;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetServiceToken()
    {
        if (HasValidToken())
        {
            _logger.LogTrace(SecurityLogging.SecurityEventId, "Returning service token from cache");
            return _serviceToken;
        }

        using var locker = await _lock.AcquireAsync();
        if (HasValidToken())
        {
            _logger.LogTrace(SecurityLogging.SecurityEventId, "Returning service token from cache after acquiring lock");
            return _serviceToken;
        }

        if (_options.ConfigurationManager == null)
        {
            throw new SecurityException(
                $"Could not get security token for service account {_options} because of missing ConfigurationManager on the JwtBearerOptions");
        }

        var config = await _options.ConfigurationManager.GetConfigurationAsync(default).ConfigureAwait(false);

        _logger.LogInformation(SecurityLogging.SecurityEventId, "Requesting new service token");
        using var httpClient = _httpClientFactory.CreateClient(_options.ServiceTokenClientName!);
        using var response = await httpClient.PostAsJsonAsync(config.TokenEndpoint, CreateServiceTokenRequestModel())
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new SecurityException(
                $"Could not get security token for service account {_options.UserName}. Returned status-code {response.StatusCode}");
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(SecureConnectDefaults.JsonOptions).ConfigureAwait(false);
        if (tokenResponse == null)
        {
            throw new SecurityException(
                $"Could not deserialize access token response while fetching new token for {_options.UserName}");
        }

        _serviceToken = tokenResponse.AccessToken;

        var expiry = _timeProvider.GetUtcNow().AddSeconds(tokenResponse.ExpiresIn);
        _serviceTokenValidTo = expiry.Subtract(_options.RefreshBeforeExpiration);

        _logger.LogInformation(SecurityLogging.SecurityEventId, "Got new service token, valid until {Expiry} but we will refresh anytime after {ValidTo}", expiry, _serviceTokenValidTo);
        return _serviceToken;
    }

    public ValueTask DisposeAsync()
        => _lock.DisposeAsync();

    [MemberNotNullWhen(true, nameof(_serviceToken))]
    private bool HasValidToken()
        => _serviceToken != null && _serviceTokenValidTo > _timeProvider.GetUtcNow();

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

        var scopes = new List<string>(_options.Scopes ?? Enumerable.Empty<string>());

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
