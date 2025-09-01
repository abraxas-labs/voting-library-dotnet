// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Voting.Lib.Database.Postgres.Locking;

/// <summary>
/// Extension methods for queryables for locking.
/// </summary>
public static class QueryLockExtensions
{
    /// <summary>
    /// Adds an annotation to intercept the query and add `FOR UPDATE SKIP LOCKED`.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <typeparam name="T">The type of the query.</typeparam>
    /// <returns>The tagged queryable.</returns>
    public static IQueryable<T> ForUpdateSkipLocked<T>(this IQueryable<T> query)
        => query.TagWith(NpgsqlLockCommandInterceptor.LockForUpdateSkipLockedAnnotation);

    /// <summary>
    /// Adds an annotation to intercept the query and add `FOR UPDATE`.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <typeparam name="T">The type of the query.</typeparam>
    /// <returns>The tagged queryable.</returns>
    public static IQueryable<T> ForUpdate<T>(this IQueryable<T> query)
        => query.TagWith(NpgsqlLockCommandInterceptor.LockForUpdateAnnotation);
}
