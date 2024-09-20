// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Voting.Lib.Rest.Swagger.Configuration;

namespace Voting.Lib.Rest.Swagger.Utils;

/// <summary>
/// Factory for creating Swagger/OpenAPI info.
/// </summary>
public static class SwaggerInfoFactory
{
    /// <summary>
    /// Create the info for the config and description.
    /// </summary>
    /// <param name="config">The OpenAPI configuration.</param>
    /// <param name="description">The API description.</param>
    /// <returns>Returns the OpenAPI info.</returns>
    public static OpenApiInfo CreateInfo(OpenApiDescription config, ApiVersionDescription? description = null)
    {
        var version = description == null ? config.ApiVersion : description.ApiVersion.ToString();

        var info = new OpenApiInfo()
        {
            Version = version,
            Title = $"{config.Title} v{version}",
            TermsOfService = config.TermsOfServiceUrl,
            Contact = new OpenApiContact()
            {
                Name = config.ContactName,
                Email = config.ContactEmail,
                Url = config.ContactUrl,
            },
            License = new OpenApiLicense
            {
                Name = config.LicenseName,
                Url = config.LicenseUrl,
            },
        };

        if (description != null && description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
