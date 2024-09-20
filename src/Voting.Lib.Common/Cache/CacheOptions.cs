// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common.Cache;

/// <summary>
/// Represents cache options.
/// </summary>
/// <typeparam name="T">The type of the cache value.</typeparam>
public class CacheOptions<T>
{
    /// <summary>
    /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Gets or sets an absolute expiration time, relative to now.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Gets or sets a function to calculate the size of a given entry.
    /// </summary>
    public Func<string, T, long> CalculateSize { get; set; } = (_, _) => 1;
}
