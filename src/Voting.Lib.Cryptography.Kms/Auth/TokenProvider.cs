// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Voting.Lib.Common;

namespace Voting.Lib.Cryptography.Kms.Auth;

/// <summary>
/// Provides and caches KMS auth tokens.
/// </summary>
public class TokenProvider : IAsyncDisposable
{
    private readonly AsyncLock _lock = new();
    private readonly TimeProvider _timeProvider;

    private readonly TokenClient _client;
    private TokenState? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenProvider"/> class.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="client">The client to fetch the tokens.</param>
    public TokenProvider(
        TimeProvider timeProvider,
        TokenClient client)
    {
        _timeProvider = timeProvider;
        _client = client;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _lock.DisposeAsync();
    }

    internal async Task<string> GetToken()
    {
        if (HasValidToken())
        {
            return _state.Token;
        }

        using var locker = await _lock.AcquireAsync();
        if (HasValidToken())
        {
            return _state.Token;
        }

        _state = await _client.RefreshOrFetchNewToken(_state);
        return _state.Token;
    }

    [MemberNotNullWhen(true, nameof(_state))]
    private bool HasValidToken()
        => _state != null && _state.Expiration > _timeProvider.GetUtcNow();
}
