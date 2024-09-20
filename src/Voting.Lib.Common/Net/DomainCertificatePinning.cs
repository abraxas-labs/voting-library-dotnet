// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Voting.Lib.Common.Configuration;

namespace Voting.Lib.Common.Net;

/// <summary>
/// Represents a configured domain certificate pinning.
/// </summary>
public class DomainCertificatePinning
{
    /// <summary>
    /// Gets list to which domain authorities this pin configuration applies.
    /// </summary>
    public HashSet<string> Authorities { get; } = new();

    /// <summary>
    /// Gets a set of public keys which are valid for the specified <see cref="Authorities"/>.
    /// </summary>
    public HashSet<string> PublicKeys { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets or sets a set of required public key sets which need to be found in the certificate chain.
    /// Of each set at least one public key must be present in the chain.
    /// </summary>
    public HashSet<PublicKeySet>? ChainPublicKeys { get; set; }

    /// <summary>
    /// Gets or sets the health check configuration for the pinned authorities.
    /// </summary>
    public HttpProbeConfig HealthCheck { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the configured <see cref="Authorities"/> are allow-listed,
    /// even when they don't have any pins configured.
    /// </summary>
    public bool AllowWithoutAnyPins { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all certificates are accepted for the configured <see cref="Authorities"/>.
    /// </summary>
    public bool DangerouslyAcceptAnyCertificate { get; set; }

    internal bool HasPublicKeys => PublicKeys is { Count: > 0 };

    internal bool HasChainPublicKeys => ChainPublicKeys is { Count: > 0 };

    internal IEnumerable<HttpProbeConfig> HealthCheckHttpProbes => Authorities.Select(authority => new HttpProbeConfig
    {
        Scheme = Uri.UriSchemeHttps,
        Host = authority,
        Path = HealthCheck.Path,
        Method = HealthCheck.Method,
        ExpectedResponseStatusCode = HealthCheck.ExpectedResponseStatusCode,
        ExpectedResponseContent = HealthCheck.ExpectedResponseContent,
        IsHealthCheckEnabled = HealthCheck.IsHealthCheckEnabled,
        IsResponseStatusCheckEnabled = HealthCheck.IsResponseStatusCheckEnabled,
        IsResponseContentCheckEnabled = HealthCheck.IsResponseContentCheckEnabled,
    });
}
