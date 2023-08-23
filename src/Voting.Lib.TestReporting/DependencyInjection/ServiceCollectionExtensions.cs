// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.TestReporting.Services;

namespace Voting.Lib.TestReporting.DependencyInjection;

/// <summary>
/// Service collection extensions for test reporting registrations.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the services related to test reporting to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddTestReporting(this IServiceCollection services)
    {
        return services.AddSingleton<JUnitReporter>();
    }
}
