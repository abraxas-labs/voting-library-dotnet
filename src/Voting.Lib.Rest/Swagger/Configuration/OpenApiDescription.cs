// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Rest.Swagger.Configuration;

/// <summary>
/// Description for an OpenAPI.
/// </summary>
public class OpenApiDescription
{
    /// <summary>
    /// Gets or sets the API version. If the ApiExplorer is used instead, this value is ignored.
    /// </summary>
    public string ApiVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the terms of service url.
    /// </summary>
    public Uri? TermsOfServiceUrl { get; set; }

    /// <summary>
    /// Gets or sets the contact name.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact email.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact url.
    /// </summary>
    public Uri? ContactUrl { get; set; }

    /// <summary>
    /// Gets or sets the license name.
    /// </summary>
    public string LicenseName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the license url.
    /// </summary>
    public Uri? LicenseUrl { get; set; }
}
