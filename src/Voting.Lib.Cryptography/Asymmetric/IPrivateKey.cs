// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// A cryptographic private key.
/// </summary>
public interface IPrivateKey : IPublicKey
{
    /// <summary>
    /// Gets the private key in PKCS#8 format.
    /// </summary>
    byte[] PrivateKey { get; }
}
