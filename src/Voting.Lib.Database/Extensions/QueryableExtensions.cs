// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace System.Linq;

/// <summary>
/// Extension methods for <see cref="IQueryable"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Calls <see cref="EntityFrameworkQueryableExtensions.ToListAsync{TSource}"/> and <see cref="Enumerable.ToHashSet{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>.
    /// </summary>
    /// <param name="queryable">The queryable.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the queryable.</typeparam>
    /// <returns>The resulting hash set.</returns>
    public static async Task<HashSet<T>> ToHashSetAsync<T>(this IQueryable<T> queryable, CancellationToken ct = default)
    {
        var l = await queryable.ToListAsync(ct).ConfigureAwait(false);
        return l.ToHashSet();
    }
}
