// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Voting.Lib.Database.Testing.NQueryDetector;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Extensions methods for detecting problematic N+1 queries.
/// </summary>
public static class NQueryDetectorExtensions
{
    internal const string IgnoreTag = "-- " + IgnoreTagContent;
    private const string IgnoreTagContent = "ignore voting lib n query detector";

    private static readonly Lazy<NQueryDetectorInterceptor> GlobalNQueryDetectorInterceptor =
        new(() => new NQueryDetectorInterceptor());

    /// <summary>
    /// This query does not count against the n query counter limit.
    /// </summary>
    /// <param name="q">The queryable.</param>
    /// <typeparam name="T">The type of the queryable.</typeparam>
    /// <returns>The updated queryable.</returns>
    public static IQueryable<T> IgnoreNQueryDetector<T>(this IQueryable<T> q)
        => q.TagWith(IgnoreTagContent);

    /// <summary>
    /// Adds the n query detector to check how many times a query is run.
    /// </summary>
    /// <param name="builder">The options builder.</param>
    /// <param name="useGlobalInstance">
    /// If true a global n query detector instance is used.
    /// True should be provided if the detector should work across multiple DbContext instances.
    /// </param>
    /// <returns>The options builder instance.</returns>
    public static DbContextOptionsBuilder AddNQueryDetector(this DbContextOptionsBuilder builder, bool useGlobalInstance = true)
        => AddNQueryDetector(builder, useGlobalInstance ? GlobalNQueryDetectorInterceptor.Value : new NQueryDetectorInterceptor());

    /// <summary>
    /// Adds the n query detector to check how many times a query is run.
    /// </summary>
    /// <param name="builder">The options builder.</param>
    /// <param name="interceptor">The <see cref="NQueryDetectorInterceptor"/> instance.</param>
    /// <returns>The options builder instance.</returns>
    public static DbContextOptionsBuilder AddNQueryDetector(this DbContextOptionsBuilder builder, NQueryDetectorInterceptor interceptor)
    {
        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new NQueryDetectorEfCoreExtension(interceptor));
        return builder.AddInterceptors(interceptor);
    }

    /// <summary>
    /// Creates a new n query detector span.
    /// </summary>
    /// <param name="ctx">The database context on which the detector should act.</param>
    /// <param name="maxQueryCount">The maximum count of each query to be allowed.</param>
    /// <returns>The created span.</returns>
    public static NQueryDetectorSpan CreateNQueryDetectorSpan(this DbContext ctx, int maxQueryCount = 1)
        => ctx.GetService<NQueryDetectorInterceptor>().CreateSpan(maxQueryCount);
}
