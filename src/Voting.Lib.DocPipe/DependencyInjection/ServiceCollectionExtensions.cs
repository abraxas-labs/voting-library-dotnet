// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.DocPipe;
using Voting.Lib.DocPipe.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for DocPipe.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the DocPipe services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The DocPipe configuration.</param>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddDocPipe(this IServiceCollection services, DocPipeConfig config)
    {
        services.TryAddSingleton(config);

        services.AddHttpClient<IDocPipeService, DocPipeService>(httpClient =>
        {
            httpClient.Timeout = config.Timeout ?? Timeout.InfiniteTimeSpan;
            httpClient.BaseAddress = config.BaseAddress ?? throw new ValidationException("DocPipe base address is required");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(DocPipeConfig.DocPipeAuthenticationScheme, config.Token);
        });
        services.TryAddTransient<IDocPipeUrlBuilder, DocPipeUrlBuilder>();

        return services;
    }
}
