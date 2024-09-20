// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Common.Configuration;

/// <summary>
/// Configuration options for http probes to 3rd party APIs.
/// </summary>
public class HttpProbesHealthCheckConfig
{
    /// <summary>
    /// Gets or sets the list of http probes to check.
    /// </summary>
    public List<HttpProbeConfig> Probes { get; set; } = new();

    /// <summary>
    /// Gets or sets the health check's default http request timeout for all configured probes.
    /// Overrides for specific probes could be configured on the probe itself.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets a value indicating whether the health check for all configured probes is enabled or not.
    /// </summary>
    public bool IsHealthCheckEnabled { get; set; } = true;
}
