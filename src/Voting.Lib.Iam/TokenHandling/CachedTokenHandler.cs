// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Common.Cache;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// Base class for token handlers which need to cache fetched access tokens until they are about to expire.
/// </summary>
internal abstract class CachedTokenHandler : ITokenHandler, IDisposable
{
    private readonly ICache<TokenWithExpiration> _cache;
    private readonly KeyedAsyncLock<string> _refreshLock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedTokenHandler"/> class.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cache">The cache used to store fetched tokens by <see cref="GetCacheKey"/>.</param>
    protected CachedTokenHandler(
        TimeProvider timeProvider,
        ILogger<CachedTokenHandler> logger,
        ICache<TokenWithExpiration> cache)
    {
        TimeProvider = timeProvider;
        Logger = logger;
        _cache = cache;
    }

    protected TimeProvider TimeProvider { get; }

    protected ILogger<CachedTokenHandler> Logger { get; }

    /// <summary>
    /// Returns a valid token for the current request, fetching a new one if the cached token is missing or about to expire.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The token.</returns>
    public async Task<string> GetToken(CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey();

        var cached = _cache.Get(cacheKey);
        if (cached != null && cached.ValidTo > TimeProvider.GetUtcNow())
        {
            return cached.Token;
        }

        using var locker = await _refreshLock.AcquireAsync(cacheKey, cancellationToken).ConfigureAwait(false);

        // check inside the lock
        cached = _cache.Get(cacheKey);
        if (cached != null && cached.ValidTo > TimeProvider.GetUtcNow())
        {
            return cached.Token;
        }

        var newToken = await FetchNewToken(cancellationToken).ConfigureAwait(false);
        _cache.Set(cacheKey, newToken);
        return newToken.Token;
    }

    /// <inheritdoc />
    public void Dispose() => _refreshLock.Dispose();

    /// <summary>
    /// Fetches a new token from the identity provider.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The fetched token together with the timestamp at which it should be refreshed.</returns>
    protected abstract Task<(string Token, DateTimeOffset TokenExpiry)> FetchToken(CancellationToken cancellationToken);

    /// <summary>
    /// Returns the cache key under which the token for the current request should be stored.
    /// </summary>
    /// <returns>The cache key.</returns>
    protected abstract string GetCacheKey();

    private async Task<TokenWithExpiration> FetchNewToken(CancellationToken cancellationToken)
    {
        var (token, validTo) = await FetchToken(cancellationToken).ConfigureAwait(false);
        return new TokenWithExpiration(token, validTo);
    }
}
