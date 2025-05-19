// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;

namespace Voting.Lib.Database.Postgres.Locking;

/// <summary>
/// EF Core builder extensions to add locking support.
/// </summary>
public static class EfCoreBuilderLockExtensions
{
    /// <summary>
    /// Adds the lock interceptor.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The updated builder.</returns>
    public static DbContextOptionsBuilder AddLockInterceptors(this DbContextOptionsBuilder builder)
    {
        return builder.AddInterceptors(new NpgsqlLockCommandInterceptor());
    }

    /// <summary>
    /// Adds the lock interceptor.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <typeparam name="T">The data context.</typeparam>
    /// <returns>The updated builder.</returns>
    public static DbContextOptionsBuilder<T> AddLockInterceptors<T>(this DbContextOptionsBuilder<T> builder)
        where T : DbContext
    {
        return builder.AddInterceptors(new NpgsqlLockCommandInterceptor());
    }
}
