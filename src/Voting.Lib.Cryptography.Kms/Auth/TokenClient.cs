// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Cryptography.Kms.Configuration;
using Voting.Lib.Cryptography.Kms.Extensions;

namespace Voting.Lib.Cryptography.Kms.Auth;

/// <summary>
/// A client to fetch KMS tokens.
/// Unfortunately, KMS doesn't use standard OAuth2 or OIDC, this is a custom implementation
/// to match the implementation of the KMS.
/// </summary>
public class TokenClient
{
    // kms uses snake_case for auth endpoints, but camelCase for all other endpoints...
    private static readonly JsonSerializerOptions KmsAuthJsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly HttpClient _http;
    private readonly TimeProvider _timeProvider;
    private readonly KmsConfig _config;
    private readonly ILogger<TokenClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenClient"/> class.
    /// </summary>
    /// <param name="http">A http client.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="config">The config.</param>
    /// <param name="logger">The logger.</param>
    public TokenClient(
        HttpClient http,
        TimeProvider timeProvider,
        KmsConfig config,
        ILogger<TokenClient> logger)
    {
        _http = http;
        _timeProvider = timeProvider;
        _config = config;
        _logger = logger;
    }

    internal async Task<TokenState> RefreshOrFetchNewToken(TokenState? state)
    {
        if (state == null)
        {
            return await FetchNewToken();
        }

        return await RefreshToken(state) ?? await FetchNewToken();
    }

    private Task<TokenState> FetchNewToken()
    {
        _logger.LogDebug("Fetching new KMS token");
        return FetchToken(new FetchTokenRequest(_config.Username, _config.Password));
    }

    private async Task<TokenState?> RefreshToken(TokenState state)
    {
        try
        {
            if (state.RefreshToken == null)
            {
                return null;
            }

            _logger.LogDebug("Refreshing KMS token");
            var req = new RefreshTokenRequest(state.RefreshToken);
            var resp = await FetchToken(req);
            if (resp.RefreshToken == null)
            {
                resp = resp with { RefreshToken = state.RefreshToken };
            }

            return resp;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error refreshing token");
            return null;
        }
    }

    private async Task<TokenState> FetchToken<T>(T req)
    {
        var callStarted = _timeProvider.GetUtcNow();
        var resp = await _http.PostJson<T, TokenResponse>("v1/auth/tokens", req, KmsAuthJsonOptions);
        _logger.LogDebug("KMS token fetched");
        return new TokenState(
            resp.Jwt,
            callStarted + TimeSpan.FromSeconds(resp.Duration) - _config.TokenExpiryLeadTime,
            resp.RefreshToken);
    }

    private record RefreshTokenRequest(string RefreshToken, string GrantType = "refresh_token");

    private record FetchTokenRequest(string Username, string Password, string GrantType = "password");

    private record TokenResponse(string Jwt, string? RefreshToken, int Duration);
}
