// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Iam.SecondFactor.Configuration;
using Voting.Lib.Iam.SecondFactor.Services;
using Voting.Lib.Scheduler;

namespace Voting.Lib.Iam.SecondFactor.DependencyInjection;

/// <summary>
/// Service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the 2fa transaction provider services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration.</param>
    /// <typeparam name="TRepo">The type of the repository to use to store the configs.</typeparam>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSecondFactorTransactionProvider<TRepo>(
        this IServiceCollection services,
        SecondFactorTransactionConfig config)
        where TRepo : class, ISecondFactorTransactionRepository
    {
        services.TryAddSingleton(config);
        services.AddScheduledJob<CleanSecondFactorTransactionsJob>(config.CleanupJobInterval);
        services.TryAddScoped<ISecondFactorTransactionRepository, TRepo>();
        services.TryAddScoped<ISecondFactorTransactionService, SecondFactorTransactionService>();
        return services;
    }
}
