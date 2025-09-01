// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Pkcs11.Configuration;

/// <summary>
/// The type of a key.
/// </summary>
internal enum KeyType
{
    /// <summary>
    /// A private key.
    /// </summary>
    PrivateKey,

    /// <summary>
    /// A public key.
    /// </summary>
    PublicKey,

    /// <summary>
    /// A secret key.
    /// </summary>
    SecretKey,
}
