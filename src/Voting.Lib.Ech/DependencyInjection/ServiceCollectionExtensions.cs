// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Ech;
using Voting.Lib.Ech.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for eCH.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds voting lib ech helpers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The ech config.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddVotingLibEch(this IServiceCollection services, EchConfig config)
        => services
            .AddSingleton(config)
            .AddSingleton<IEchMessageIdProvider, DefaultEchMessageIdProvider>()
            .AddSingleton<DeliveryHeaderProvider>();
}
