// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Voting.Lib.Common.Cache;

/// <inheritdoc />
public class Cache<T> : ICache<T>
{
    private readonly string _typeId = Guid.NewGuid().ToString();
    private readonly IMemoryCache _memoryCache;
    private readonly CacheOptions<T> _cacheOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cache{T}"/> class.
    /// </summary>
    /// <param name="memoryCache">The memory cache to use as underlying cache.</param>
    /// <param name="cacheOptions">The options for this cache.</param>
    public Cache(IMemoryCache memoryCache, CacheOptions<T> cacheOptions)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions;
    }

    /// <inheritdoc />
    public T? Get(string key)
    {
        return _memoryCache.Get<T>(BuildInternalKey(key));
    }

    /// <inheritdoc />
    public void Set(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        ApplyCacheOptions(cacheEntryOptions, key, value);
        _memoryCache.Set(BuildInternalKey(key), value, cacheEntryOptions);
    }

    /// <inheritdoc />
    public Task<T> GetOrAdd(string key, Func<Task<T>> factory)
    {
        return _memoryCache.GetOrCreateAsync(BuildInternalKey(key), cacheEntry =>
        {
            ApplyCacheOptions(cacheEntry);
            return factory();
        })!;
    }

    private void ApplyCacheOptions(ICacheEntry cacheEntry)
    {
        cacheEntry.Size = _cacheOptions.CalculateSize((string)cacheEntry.Key, (T)cacheEntry.Value!);
        cacheEntry.SlidingExpiration = _cacheOptions.SlidingExpiration;
        cacheEntry.AbsoluteExpirationRelativeToNow = _cacheOptions.AbsoluteExpirationRelativeToNow;
    }

    private void ApplyCacheOptions(MemoryCacheEntryOptions cacheEntryOptions, string key, T value)
    {
        cacheEntryOptions.Size = _cacheOptions.CalculateSize(key, value);
        cacheEntryOptions.SlidingExpiration = _cacheOptions.SlidingExpiration;
        cacheEntryOptions.AbsoluteExpirationRelativeToNow = _cacheOptions.AbsoluteExpirationRelativeToNow;
    }

    private string BuildInternalKey(string key) => _typeId + "-" + key;
}
