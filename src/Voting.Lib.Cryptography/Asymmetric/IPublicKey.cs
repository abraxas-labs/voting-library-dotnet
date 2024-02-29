// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// A cryptographic public key.
/// </summary>
public interface IPublicKey : IDisposable
{
    /// <summary>
    /// Gets the key ID which is a computed hash of the <see cref="PublicKey"/>.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the public key in X509.SubjectPublicKeyInfo format.
    /// </summary>
    byte[] PublicKey { get; }
}
