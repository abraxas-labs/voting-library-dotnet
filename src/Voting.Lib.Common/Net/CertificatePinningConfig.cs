// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Common.Net;

/// <summary>
/// The configuration used for certificate pinning.
/// </summary>
public class CertificatePinningConfig
{
    /// <summary>
    /// Gets or sets a collection of certificate pins.
    /// </summary>
    public List<DomainCertificatePinning> Pins { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether pinning is required for all requests / authorities.
    /// </summary>
    public bool RequirePinningForAllAuthorities { get; set; } = true;
}
