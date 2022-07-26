// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Voting.Lib.Common;

namespace Voting.Lib.Testing.Mocks;

/// <summary>
/// Service collection extensions for testing/mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// replaces all instances of TService with a singleton instance of TMock.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="TService">The type of the service to be removed.</typeparam>
    /// <typeparam name="TMock">The type of the mock.</typeparam>
    /// <returns>The updated service collection instance.</returns>
    public static IServiceCollection AddMock<TService, TMock>(this IServiceCollection services)
        where TService : class
        where TMock : class, TService
    {
        services.RemoveAll<TService>();
        services.TryAddSingleton<TMock>();
        services.AddSingleton<TService>(sp => sp.GetRequiredService<TMock>());
        return services;
    }

    /// <summary>
    /// Removes all hosted services from the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection RemoveHostedServices(this IServiceCollection services)
        => services.RemoveAll<IHostedService>();

    /// <summary>
    /// Replaces the SystemClock with a fixed timestamp clock.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddMockedClock(this IServiceCollection services)
        => services.RemoveAll<ISystemClock>()
            .AddMock<ISystemClock, MockedClock>()
            .AddMock<IClock, MockedClock>();
}
