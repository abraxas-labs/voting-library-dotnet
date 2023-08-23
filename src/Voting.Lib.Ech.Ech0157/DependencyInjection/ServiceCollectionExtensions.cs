// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Ech.Ech0157.Converter;

namespace Voting.Lib.Ech.Ech0157.DependencyInjection;

/// <summary>
/// eCH-0157 service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds eCH-0157 services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddEch0157(this IServiceCollection services)
    {
        return services
            .AddSingleton<Ech0157Deserializer>();
    }
}
