// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.DokConnector.Configuration;
using Voting.Lib.DokConnector.Service;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service collection extensions for the DOK connector.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="EaiDokConnector"/> as <see cref="IDokConnector"/>.
    /// Make sure to add a secure connect handler on the <see cref="IHttpClientBuilder"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The config to use.</param>
    /// <returns>The http client builder.</returns>
    public static IHttpClientBuilder AddEaiDokConnector(this IServiceCollection services, DokConnectorConfig config)
    {
        services.TryAddSingleton(config);
        return services.AddHttpClient<IDokConnector, EaiDokConnector>(x => x.BaseAddress = config.Endpoint);
    }
}
