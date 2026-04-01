// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Mocks.Configuration;

/// <summary>
/// Configuration for the crypto mock.
/// </summary>
public class CryptoMockConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether the public and private key id should be derived from the provided key id segments.
    /// This value is useful to replicate HSM behavior.
    /// </summary>
    public bool DeriveKeyPairFromKeyIdSegments { get; set; } = true;
}
