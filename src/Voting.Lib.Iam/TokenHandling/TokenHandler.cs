// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// A base token handler implementation.
/// </summary>
internal abstract class TokenHandler : ITokenHandler, IAsyncDisposable
{
    private readonly AsyncLock _lock = new();

    private DateTimeOffset? _tokenValidTo;
    private string? _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenHandler"/> class.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="logger">The logger.</param>
    protected TokenHandler(
        TimeProvider timeProvider,
        ILogger<TokenHandler> logger)
    {
        TimeProvider = timeProvider;
        Logger = logger;
    }

    protected TimeProvider TimeProvider { get; }

    protected ILogger<TokenHandler> Logger { get; }

    public async Task<string> GetToken(CancellationToken cancellationToken)
    {
        if (HasValidToken())
        {
            Logger.LogTrace(SecurityLogging.SecurityEventId, "Returning token from cache");
            return _token;
        }

        using var locker = await _lock.AcquireAsync(cancellationToken).ConfigureAwait(false);
        if (HasValidToken())
        {
            Logger.LogTrace(SecurityLogging.SecurityEventId, "Returning token from cache after acquiring lock");
            return _token;
        }

        (_token, _tokenValidTo) = await FetchToken(cancellationToken);
        return _token;
    }

    public ValueTask DisposeAsync()
        => _lock.DisposeAsync();

    protected abstract Task<(string Token, DateTimeOffset TokenExpiry)> FetchToken(CancellationToken cancellationToken);

    [MemberNotNullWhen(true, nameof(_token))]
    private bool HasValidToken()
        => _token != null && _tokenValidTo > TimeProvider.GetUtcNow();
}
