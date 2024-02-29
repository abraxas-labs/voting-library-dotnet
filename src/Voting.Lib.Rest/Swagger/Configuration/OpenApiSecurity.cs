// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Rest.Swagger.Configuration;

/// <summary>
/// The open api security defines different oidc parameters which allows to
/// make use of the integrated SwaggerUI authentication methods.
/// </summary>
public class OpenApiSecurity
{
    /// <summary>
    /// Gets or sets the OIDC client app identifier.
    /// </summary>
    public string OidcClientApp { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the OIDC authroization endpoint.
    /// </summary>
    public Uri? OidcAuthorizationEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the OIDC token endpoint.
    /// </summary>
    public Uri? OidcTokenEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the OIDC discovery endpoint.
    /// </summary>
    public Uri? OidcDiscoveryEndpoint { get; set; }
}
