// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Common.Net;

/// <summary>
/// Represents a set of public keys.
/// </summary>
public class PublicKeySet
{
    /// <summary>
    /// Gets a set of public keys of which one must be present on the certificate.
    /// </summary>
    public HashSet<string> PublicKeys { get; } = new(StringComparer.OrdinalIgnoreCase);
}
