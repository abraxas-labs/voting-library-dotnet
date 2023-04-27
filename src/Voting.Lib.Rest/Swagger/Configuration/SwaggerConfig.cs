// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Rest.Swagger.Configuration;

/// <summary>
/// This configuration allows to customize the swagger generator and ui behavior.
/// </summary>
public class SwaggerConfig
{
    /// <summary>
    /// Gets or sets the API description.
    /// </summary>
    public OpenApiDescription ApiDescription { get; set; } = new();

    /// <summary>
    /// Gets or sets the API security configuration.
    /// </summary>
    public OpenApiSecurity ApiSecurity { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether to enable or disable the swagger.json generation.
    /// </summary>
    public bool EnableSwaggerGenerator { get; set; } = false;

    /// <summary>
    /// Gets or sets the route template where the generated swagger json-file can be accessed from.
    /// The placeholder {documentName} is automatically replaced by the configured api version.
    /// </summary>
    public string SwaggerGeneratorRootTemplate { get; set; } = "swagger/{documentName}/swagger.json";

    /// <summary>
    /// Gets or sets a list of XML documentation files which are included as part of the
    /// generated swagger documentation.
    /// </summary>
    public IEnumerable<string>? XmlDocumentationFiles { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to enable or disable the SwaggerUI.
    /// </summary>
    public bool EnableSwaggerUi { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to enable or disable oauth2 implicite authentication on SwaggerUI.
    /// </summary>
    public bool EnableSwaggerUiOAuth2 { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to enable or disable bearer authentication on SwaggerUI.
    /// </summary>
    public bool EnableSwaggerUiBearerAuth { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to enable or disable injection of context based headers.
    /// </summary>
    public bool EnableContextHeaderInjection { get; set; } = true;

    /// <summary>
    /// Gets or sets the default app context value (x-app).
    /// </summary>
    public string DefaultAppContextValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default tenant context value (x-tenant).
    /// </summary>
    public string DefaultTenantContextValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the swagger api servers a user may select from either localhost or another hosting environment.
    /// Multiple values can be defined if different hostings must be supported by the SwaggerUI (e.g. Port-Forwarding).
    /// </summary>
    public IEnumerable<string>? SwaggerServerUrls { get; set; }
}
