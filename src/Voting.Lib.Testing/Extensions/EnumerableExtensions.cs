// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace System.Collections.Generic;

/// <summary>
/// Test extensions for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Wraps an <see cref="IEnumerable{T}"/> into an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <param name="enumerable">The enumerable.</param>
    /// <typeparam name="T">The items type.</typeparam>
    /// <returns>The wrapped <see cref="IAsyncEnumerable{T}"/>. This is not really async.</returns>
    public static async IAsyncEnumerable<T> AsAsync<T>(this IEnumerable<T> enumerable)
    {
        foreach (var item in enumerable)
        {
            yield return item;
        }

        await Task.CompletedTask;
    }
}
