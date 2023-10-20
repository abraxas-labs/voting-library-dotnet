// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net;
using System.Net.Http;

namespace Voting.Lib.Common.Configuration;

/// <summary>
/// Configuration options for http probes to 3rd party APIs.
/// </summary>
public class HttpProbeConfig
{
    /// <summary>
    /// Gets or sets the URI scheme, i.e. 'https'. Default is <see cref="Uri.UriSchemeHttps"/>.
    /// </summary>
    public string Scheme { get; set; } = Uri.UriSchemeHttps;

    /// <summary>
    /// Gets or sets the host which may include the port if required, i.e. 'example.host.ch:1111'.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the request path, i.e. '/healthz'. Default is root '/'.
    /// </summary>
    public string? Path { get; set; } = "/";

    /// <summary>
    /// Gets or sets the http method for the request. Default is 'HEAD'.
    /// </summary>
    public string Method { get; set; } = "HEAD";

    /// <summary>
    /// Gets the <see cref="System.Net.Http.HttpMethod"/> from <see cref="Method"/>,
    /// where allowed values are <see cref="HttpMethod.Get"/> or <see cref="HttpMethod.Head"/>.
    /// </summary>
    public HttpMethod HttpMethod => Method switch
    {
        "GET" => HttpMethod.Get,
        "HEAD" => HttpMethod.Head,
        _ => HttpMethod.Head,
    };

    /// <summary>
    /// Gets the https probe's request <see cref="Uri"/> created from properties:
    /// <list type="bullet">
    ///     <item><see cref="Scheme"/></item>
    ///     <item><see cref="Host"/></item>
    ///     <item><see cref="Path"/></item>
    /// </list>
    /// </summary>
    /// <exception cref="UriFormatException">Throws if Uri creation fails.</exception>
    public Uri? RequestUri =>
        Uri.TryCreate(
            $"{Scheme}://{Host}/{Path?.TrimStart('/')}",
            UriKind.Absolute,
            out var authorityUri)
            ? authorityUri
            : null;

    /// <summary>
    /// Gets or sets the expected response status code. It will only be validated if <see cref="IsResponseStatusCheckEnabled"/> is set to true.
    /// Default is <see cref="HttpStatusCode.OK"/>.
    /// </summary>
    public HttpStatusCode ExpectedResponseStatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// Gets or sets the expected response content. It will only be validated if <see cref="IsResponseContentCheckEnabled"/> is set to true.
    /// </summary>
    public string ExpectedResponseContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the http request timeout for the probe. If nothing defined, the default value for all probes should be used.
    /// </summary>
    public TimeSpan? RequestTimeout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the health check for this specific probe is enabled or not. Default is 'true'.
    /// </summary>
    public bool IsHealthCheckEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the response status code should be validated or not. Default is 'false'.
    /// </summary>
    public bool IsResponseStatusCheckEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the response content should be validated or not. Default is 'false'.
    /// </summary>
    public bool IsResponseContentCheckEnabled { get; set; }
}
