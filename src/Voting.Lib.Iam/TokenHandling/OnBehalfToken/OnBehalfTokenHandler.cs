// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Voting.Lib.Common;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Voting.Lib.Iam.TokenHandling.OnBehalfToken;

internal class OnBehalfTokenHandler : TokenHandler
{
    private readonly SecureConnectOnBehalfOptions _options;
    private readonly SecureConnectServiceAccountOptions _serviceAccountOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public OnBehalfTokenHandler(
        ILogger<OnBehalfTokenHandler> logger,
        SecureConnectOnBehalfOptions options,
        SecureConnectServiceAccountOptions serviceAccountOptions,
        TimeProvider timeProvider,
        IHttpContextAccessor httpContextAccessor,
        HttpClient httpClient)
        : base(timeProvider, logger)
    {
        _options = options;
        _httpContextAccessor = httpContextAccessor;
        _serviceAccountOptions = serviceAccountOptions;
        _httpClient = httpClient;
    }

    protected override async Task<(string Token, DateTimeOffset TokenExpiry)> FetchToken(CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new SecurityException("Could not get security token without http context");
        }

        var subjectToken = GetSubjectToken();
        Logger.LogInformation(SecurityLogging.SecurityEventId, "Requesting new on behalf token");

        var tokenEndpoint = await GetTokenEndpoint(cancellationToken);
        var requestStarted = TimeProvider.GetUtcNow();
        using var response = await _httpClient.PostAsJsonAsync(tokenEndpoint, new OnBehalfTokenRequestModel(subjectToken, _options.Resource), SecureConnectDefaults.JsonOptions, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new SecurityException(
                $"Could not get ob token. Returned status-code {response.StatusCode}");
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(SecureConnectDefaults.JsonOptions, cancellationToken).ConfigureAwait(false);
        if (tokenResponse == null)
        {
            throw new SecurityException(
                "Could not deserialize access token response while fetching new ob token");
        }

        var token = tokenResponse.AccessToken ?? throw new SecurityException("No token received");
        var expiry = requestStarted.AddSeconds(tokenResponse.ExpiresIn);
        var refreshAfter = expiry.Subtract(_options.RefreshBeforeExpiration);

        Logger.LogInformation(SecurityLogging.SecurityEventId, "Got new ob token, valid until {Expiry} but we will refresh anytime after {ValidTo}", expiry, refreshAfter);
        return (token, refreshAfter);
    }

    private async Task<string> GetTokenEndpoint(CancellationToken cancellationToken)
    {
        // the configuration is cached by the configuration manager.
        var config = await _serviceAccountOptions.ConfigurationManager.GetConfigurationAsync(cancellationToken);
        return config.TokenEndpoint;
    }

    private string GetSubjectToken()
    {
        var subjectToken = _httpContextAccessor.HttpContext?
            .Request.Headers
            .FirstOrDefault(header => header.Key == HeaderNames.Authorization)
            .Value
            .FirstOrDefault()
            ?.Trim();

        if (subjectToken == null)
        {
            throw new SecurityException("Could not get security token without authorization header");
        }

        if (subjectToken.StartsWith(SecureConnectDefaults.BearerScheme, StringComparison.OrdinalIgnoreCase))
        {
            subjectToken = subjectToken[SecureConnectDefaults.BearerScheme.Length..].Trim();
        }

        return subjectToken;
    }
}
