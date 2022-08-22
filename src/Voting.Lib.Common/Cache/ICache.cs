// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;

namespace Voting.Lib.Common.Cache;

/// <summary>
/// A cache represents a data storage which that is used to store data temporarily.
/// </summary>
/// <typeparam name="T">Cache entry type.</typeparam>
public interface ICache<T>
{
    /// <summary>
    /// Gets the cache entry.
    /// </summary>
    /// <param name="key">The cache key to retrieve.</param>
    /// <returns>Returns the cached value or the default value if no entry exists.</returns>
    T? Get(string key);

    /// <summary>
    /// Inserts the key and value into the cache.
    /// Replaces an existing cache entry with the same key, should one exist.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="value">The value of the cache entry.</param>
    void Set(string key, T value);

    /// <summary>
    /// Gets the cached value if it exists. Otherwise, adds the entry to the cache.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The factory to create a new cache entry.</param>
    /// <returns>The cached or newly created value.</returns>
    Task<T> GetOrAdd(string key, Func<Task<T>> factory);
}
