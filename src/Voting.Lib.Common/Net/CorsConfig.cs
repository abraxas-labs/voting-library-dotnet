// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Common.Net;

/// <summary>
/// Configuration options for CORS builder.
/// </summary>
public class CorsConfig
{
    /// <summary>
    /// Gets or sets the access-control-allow-headers.
    /// </summary>
    public List<string> AllowedHeaders { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the access-control-allow-methods.
    /// </summary>
    public List<string> AllowedMethods { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the access-control-allow-origin.
    /// </summary>
    public List<string> AllowedOrigins { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the access-control-expose-headers.
    /// </summary>
    public List<string> ExposedHeaders { get; set; } = new List<string>();
}
