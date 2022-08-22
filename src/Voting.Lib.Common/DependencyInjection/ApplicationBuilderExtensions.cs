// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Voting.Lib.Common.Net;

namespace Voting.Lib.Common.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds CORS middleware from <see cref="CorsMiddlewareExtensions"/>
    /// using the configuration from the registered <see cref="CorsConfig"/> options
    /// where the corresponding appsettings config section will bind to.
    /// </summary>
    /// <param name="app">The IApplicationBuilder.</param>
    /// <returns>Reference to IApplicationBuilder.</returns>
    public static IApplicationBuilder UseCorsFromConfig(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetRequiredService<IOptions<CorsConfig>>().Value;

        app.UseCors(builder => builder
                .WithOrigins(config.AllowedOrigins.ToArray())
                .WithMethods(config.AllowedMethods.ToArray())
                .WithHeaders(config.AllowedHeaders.ToArray())
                .WithExposedHeaders(config.ExposedHeaders.ToArray()));

        return app;
    }
}
