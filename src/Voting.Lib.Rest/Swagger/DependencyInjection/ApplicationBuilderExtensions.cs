// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Voting.Lib.Rest.Swagger.Configuration;

namespace Voting.Lib.Rest.Swagger.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the swagger generator as part of the applications's middleware.
    /// </summary>
    /// <param name="app">The IApplicationBuilder.</param>
    /// <returns>Reference to IApplicationBuilder.</returns>
    public static IApplicationBuilder UseSwaggerGenerator(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetRequiredService<IOptions<SwaggerConfig>>().Value;

        if (!config.EnableSwaggerGenerator)
        {
            return app;
        }

        app.UseSwagger(c =>
        {
            c.RouteTemplate = config.SwaggerGeneratorRootTemplate;
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                ConfigureSwaggerServerUrls(swaggerDoc, httpReq, config));
        });

        return app;
    }

    /// <summary>
    /// Adds the SwaggerUI as part of the applications's middleware.
    /// </summary>
    /// <param name="app">The IApplicationBuilder.</param>
    /// <returns>The IApplicationBuilder reference.</returns>
    public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetRequiredService<IOptions<SwaggerConfig>>().Value;

        if (config.EnableSwaggerUi)
        {
            app.UseSwaggerUI(c =>
            {
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                if (provider?.ApiVersionDescriptions.Count > 0)
                {
                    // register all api versions in the swagger ui based on generic version descriptors
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.RoutePrefix = "swagger";
                        c.SwaggerEndpoint($"{description.GroupName}/swagger.json", config.ApiDescription.Title);
                    }
                }
                else
                {
                    c.RoutePrefix = "swagger";
                    c.SwaggerEndpoint($"{config.ApiDescription.ApiVersion}/swagger.json", config.ApiDescription.Title);
                }

                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>() { { "response_type", "code token" } });
                c.OAuthClientId(config.ApiSecurity.OidcClientApp);
                c.OAuthAppName(config.ApiDescription.Title);
            });
        }

        return app;
    }

    private static void ConfigureSwaggerServerUrls(OpenApiDocument swaggerDoc, HttpRequest httpReq, SwaggerConfig config)
    {
        if (config.SwaggerServerUrls == null)
        {
            return;
        }

        foreach (var server in config.SwaggerServerUrls)
        {
            var serverUrl = server.Replace("{scheme}", httpReq.Scheme, StringComparison.OrdinalIgnoreCase)
                .Replace("{host}", httpReq.Host.Value, StringComparison.OrdinalIgnoreCase);
            swaggerDoc.Servers.Add(new OpenApiServer { Url = serverUrl });
        }
    }
}
