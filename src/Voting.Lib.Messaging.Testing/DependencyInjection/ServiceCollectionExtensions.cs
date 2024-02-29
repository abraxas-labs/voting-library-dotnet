// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Messaging;
using Voting.Lib.Messaging.Testing.Mocks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for messaging mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Replace the messaging implementation with mocks.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="busConfiguratorAction">The configuration action.</param>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddVotingLibMessagingMocks(
        this IServiceCollection services,
        Action<IServiceCollectionBusConfigurator> busConfiguratorAction)
    {
        services.AddMassTransitInMemoryTestHarness(cfg =>
        {
            busConfiguratorAction(cfg);

            cfg.SetKebabCaseEndpointNameFormatter();
        });

        services
            .AddScoped<MessageProducerBuffer>()
            .AddSingleton(typeof(MessageConsumerHub<,>))
            .AddSingleton(typeof(MessageConsumerHub<>))
            .RemoveAll<IMessagingHealth>()
            .AddSingleton(new MessagingHealthMock(true))
            .AddSingleton<IMessagingHealth>(sp => sp.GetRequiredService<MessagingHealthMock>());

        return services;
    }
}
