// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Voting.Lib.Rest.Swagger.Configuration;
using Voting.Lib.Rest.Swagger.DocumentFilters;
using Voting.Lib.Rest.Swagger.OperationFilters;
using Voting.Lib.Rest.Swagger.Utils;

namespace Voting.Lib.Rest.Swagger.DependencyInjection;

/// <summary>
/// Service collection extensions for adding Swagger.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string SwaggerConfigSection = "Swagger";

    /// <summary>
    /// Registers and configures the swagger generator services.
    /// Optionally adds authentication options.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="configuration">The Configuration.</param>
    /// <returns>The IServiceCollectionReference.</returns>
    public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SwaggerConfig>(configuration.GetSection(SwaggerConfigSection));
        var config = configuration.GetSection(SwaggerConfigSection).Get<SwaggerConfig>();

        return AddSwaggerGenerator(services, config);
    }

    /// <summary>
    /// Registers and configures the swagger generator services.
    /// Optionally adds authentication options.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="config">The swagger configuration.</param>
    /// <returns>The IServiceCollectionReference.</returns>
    public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services, SwaggerConfig config)
    {
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.OperationFilter<SwaggerDefaultValues>(); // Open issue: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            options.OperationFilter<SwaggerExtensionOperationFilter>();
            options.OperationFilter<ContextHeaderFilter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
            if (provider?.ApiVersionDescriptions.Count > 0)
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        SwaggerInfoFactory.CreateInfo(config.ApiDescription, description));
                }
            }
            else
            {
                options.SwaggerDoc(config.ApiDescription.ApiVersion, SwaggerInfoFactory.CreateInfo(config.ApiDescription));
            }

            // OIDC Authentication Extension
            if (config.EnableSwaggerUiOAuth2)
            {
                options.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = config.ApiSecurity.OidcAuthorizationEndpoint,
                                TokenUrl = config.ApiSecurity.OidcTokenEndpoint,
                                Scopes = new Dictionary<string, string>
                                {
                                    { "openid", "Access OpenId protocol" },
                                    { "profile", "Access profile" },
                                    { "email", "Access email address" },
                                },
                            },
                        },
                        OpenIdConnectUrl = config.ApiSecurity.OidcDiscoveryEndpoint,
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                            },
                            new[] { "openid", "profile", "email" }
                        },
                });
            }

            // JWT Bearer Token Authentication Extension
            if (config.EnableSwaggerUiBearerAuth)
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme (Sample: '{jwt-token}').",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "bearer",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            },
                            Array.Empty<string>()
                        },
                });
            }

            // Register api descriptions
            if (config.XmlDocumentationFiles != null)
            {
                foreach (var xmlFile in config.XmlDocumentationFiles)
                {
                    options.IncludeXmlComments(xmlFile, true);
                }
            }
        });

        return services;
    }
}
