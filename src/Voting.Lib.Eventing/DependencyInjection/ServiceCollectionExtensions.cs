// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Reflection;
using Voting.Lib.Eventing.Configuration;
using Voting.Lib.Eventing.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for eventing related things.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds eventing services to the ServiceCollection.
    /// Aggregates, AggregateSeeders and EventProcessors can be added via assembly scanning on the returned <see cref="IEventingServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration of the event store.</param>
    /// <param name="protoAssemblies">Assemblies which should be scanned for proto events.</param>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance to add more eventing services.</returns>
    public static IEventingServiceCollection AddVotingLibEventing(
        this IServiceCollection services,
        EventStoreConfig config,
        params Assembly[] protoAssemblies)
        => new EventingServiceCollection(services, config, protoAssemblies);
}
