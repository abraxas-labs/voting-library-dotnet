// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Ech.Ech0045.Converter;

namespace Voting.Lib.Ech.Ech0045.DependencyInjection;

/// <summary>
/// eCH-0045 service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds eCH-0045 services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddEch0045(this IServiceCollection services)
    {
        return services
            .AddSingleton<Ech0045Serializer>()
            .AddSingleton<Ech0045Deserializer>();
    }
}
