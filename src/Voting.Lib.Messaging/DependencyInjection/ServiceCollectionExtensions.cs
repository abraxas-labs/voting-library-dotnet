// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Voting.Lib.Messaging;
using Voting.Lib.Messaging.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions to add messaging support.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds messaging support to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The RabbitMQ configuration.</param>
    /// <param name="busConfiguratorAction">An action to configure the bus.</param>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddVotingLibMessaging(
        this IServiceCollection services,
        RabbitMqConfig config,
        Action<IServiceCollectionBusConfigurator> busConfiguratorAction)
    {
        services.AddLogging();
        services.AddSystemClock();
        services.AddMassTransit(o =>
        {
            busConfiguratorAction(o);

            o.SetKebabCaseEndpointNameFormatter();
            o.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config.HostName, config.VirtualHost, h =>
                {
                    h.UseCluster(c =>
                    {
                        foreach (var host in config.Hosts)
                        {
                            c.Node(host);
                        }
                    });
                    h.Username(config.Username);
                    h.Password(config.Password);
                });

                cfg.UseMessageRetry(r => r.Intervals(config.RetryIntervals.ToArray()));
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services
            .AddMassTransitHostedService()
            .AddScoped<MessageProducerBuffer>()
            .AddSingleton(typeof(MessageConsumerHub<,>))
            .AddSingleton(typeof(MessageConsumerHub<>))
            .AddSingleton<IMessagingHealth, MessagingHealth>();
    }
}
