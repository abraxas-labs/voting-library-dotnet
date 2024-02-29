// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Database.Models;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Extensions for <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryablePageableExtensions
{
    /// <summary>
    /// Returns a page of the provided query.
    /// </summary>
    /// <param name="queryable">The queryable.</param>
    /// <param name="currentPage">The 1-indexed page to return.</param>
    /// <param name="pageSize">The number of entries to return per page.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The entity type of the query.</typeparam>
    /// <returns>A task resolving to the fetched page.</returns>
    public static Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> queryable,
        int currentPage,
        int pageSize,
        CancellationToken ct = default)
    {
        if (currentPage < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(currentPage));
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

        return ToPageInternalAsync(queryable, currentPage, pageSize, ct);
    }

    /// <summary>
    /// Returns a page of the provided query.
    /// </summary>
    /// <param name="queryable">The queryable.</param>
    /// <param name="pageable">The page to load.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The entity type of the query.</typeparam>
    /// <returns>A task resolving to the fetched page.</returns>
    public static Task<Page<T>> ToPageAsync<T>(this IQueryable<T> queryable, Pageable pageable, CancellationToken ct = default)
        => ToPageAsync(queryable, pageable.Page, pageable.PageSize, ct);

    private static async Task<Page<T>> ToPageInternalAsync<T>(IQueryable<T> queryable, int currentPage, int pageSize, CancellationToken ct)
    {
        var skippedItemCount = (currentPage - 1) * pageSize;
        var items = await queryable
            .Skip(skippedItemCount)
            .Take(pageSize)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        // we only need to fetch the total items count if it isn't the last page and
        // it has not 0 items (ie. if we fetch the page N+1 we can't calculate the total of items)
        var count = pageSize == items.Count || items.Count == 0
            ? await queryable.CountAsync(ct).ConfigureAwait(false)
            : skippedItemCount + items.Count;

        return new Page<T>(items, count, currentPage, pageSize);
    }
}
