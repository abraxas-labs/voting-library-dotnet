// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.AspNetCore.HeaderPropagation;
using Voting.Lib.Rest;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all abraxas headers to the header propagation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">An optional configure action to set additional options on <see cref="HeaderPropagationOptions"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAbraxasHeaderPropagation(this IServiceCollection services, Action<HeaderPropagationOptions>? configure = null)
    {
        return services.AddHeaderPropagation(opts =>
        {
            foreach (var headerName in AbraxasHeaderNames.All)
            {
                opts.Headers.Add(headerName);
            }

            configure?.Invoke(opts);
        });
    }
}
