// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;
using Moq;
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
    /// replaces all instances of TService with the provided singleton instance of TMock.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="mock">The mock instance.</param>
    /// <typeparam name="TService">The type of the service to be removed.</typeparam>
    /// <typeparam name="TMock">The type of the mock.</typeparam>
    /// <returns>The updated service collection instance.</returns>
    public static IServiceCollection AddMock<TService, TMock>(this IServiceCollection services, TMock mock)
        where TService : class
        where TMock : class, TService
    {
        services.RemoveAll<TService>();
        services.TryAddSingleton(mock);
        services.AddSingleton<TService>(sp => sp.GetRequiredService<TMock>());
        return services;
    }

    /// <summary>
    /// Replaces all instances of TService with a singleton instance of a mock.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">The configuration method to setup the mock.</param>
    /// <typeparam name="TService">The type of the service to be replaced.</typeparam>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMock<TService>(this IServiceCollection services, Action<Mock<TService>> configure)
        where TService : class
    {
        services.RemoveAll<TService>();

        var mock = new Mock<TService>();
        configure(mock);
        services.AddSingleton(mock);
        services.AddSingleton(mock.Object);
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
    /// Replaces the clock and system time provider with a <see cref="FakeTimeProvider"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddMockedClock(this IServiceCollection services)
    {
        return services
            .AddMock<IClock, MockedClock>()
            .AddMock<TimeProvider, FakeTimeProvider>(MockedClock.CreateFakeTimeProvider());
    }

    /// <summary>
    /// Replaces the system time provider with a <see cref="FakeTimeProvider"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddMockedTimeProvider(this IServiceCollection services)
    {
        return services
            .AddMock<TimeProvider, FakeTimeProvider>(MockedClock.CreateFakeTimeProvider());
    }
}
