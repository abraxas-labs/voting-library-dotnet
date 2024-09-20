// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using MassTransit.Testing;

namespace MassTransit.ExtensionsDependencyInjectionIntegration;

/// <summary>
/// Service collection extensions for <see cref="IServiceCollectionBusConfigurator"/>.
/// </summary>
public static class ServiceCollectionBusConfiguratorExtensions
{
    /// <summary>
    /// Add consumer and consumer test harness for the configurator.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    public static void AddConsumerAndConsumerTestHarness<TConsumer>(
        this IServiceCollectionBusConfigurator configurator)
        where TConsumer : class, IConsumer
    {
        configurator.AddConsumer<TConsumer>();
        configurator.AddConsumerTestHarness<TConsumer>();
    }
}
