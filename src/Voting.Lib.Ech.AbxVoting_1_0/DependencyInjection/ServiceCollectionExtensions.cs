// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Ech.AbxVoting_1_0.Converter;

namespace Voting.Lib.Ech.AbxVoting_1_0.DependencyInjection;

/// <summary>
/// Abx voting service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds abx voting services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddAbxVoting(this IServiceCollection services)
    {
        return services
            .AddSingleton<AbxVotingDeserializer>();
    }
}
