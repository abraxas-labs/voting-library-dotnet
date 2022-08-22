// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Prometheus;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for prometheus integration.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a service which forwards <see cref="Counter{T}"/>, <see cref="ObservableCounter{T}"/> and <see cref="ObservableGauge{T}"/>
    /// metrics to prometheus-net.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddVotingLibPrometheusAdapter(this IServiceCollection services)
        => services.AddVotingLibPrometheusAdapter(new());

    /// <summary>
    /// Adds a service which forwards <see cref="Counter{T}"/>, <see cref="ObservableCounter{T}"/> and <see cref="ObservableGauge{T}"/>
    /// metrics to prometheus-net.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The config of the adapter.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddVotingLibPrometheusAdapter(this IServiceCollection services, PrometheusMeterAdapterConfig config)
    {
        services.TryAddSingleton(config);
        return services.AddHostedService<PrometheusMeterAdapter>();
    }
}
