// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Database.Migrations;
using Voting.Lib.Database.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for database support.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all Database Services to the ServiceCollection.
    /// By Default EF Core Services for relational databases, such as <see cref="DbRepository{TDbContext,TKey,TEntity}"/> are added.
    /// </summary>
    /// <typeparam name="TContext">The db context type.</typeparam>
    /// <param name="services">The ServiceCollection to be modified.</param>
    /// <returns>The ServiceCollection Instance.</returns>
    public static IServiceCollection AddVotingLibDatabase<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.TryAddEnumerable(new ServiceDescriptor(typeof(IDatabaseMigrator), typeof(DatabaseMigrator<TContext>), ServiceLifetime.Singleton));
        return services.AddEfCoreRepositories();
    }

    /// <summary>
    /// Adds ef core repositories to the ServiceCollection.
    /// </summary>
    /// <param name="services">The Servicecollection to be modified.</param>
    /// <param name="lifetime">The service lifetime to be used.</param>
    /// <returns>The ServiceCollection Instance.</returns>
    public static IServiceCollection AddEfCoreRepositories(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        services.TryAdd(new ServiceDescriptor(
            typeof(IDbRepository<,,>),
            typeof(DbRepository<,,>),
            lifetime));
        services.TryAdd(new ServiceDescriptor(
            typeof(IDbRepository<,>),
            typeof(DbRepository<,>),
            lifetime));
        return services;
    }
}
