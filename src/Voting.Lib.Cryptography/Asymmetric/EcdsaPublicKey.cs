// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// An ECDSA public key.
/// </summary>
public class EcdsaPublicKey : IPublicKey
{
    private const int IdBytesCount = 16;

    /// <summary>
    /// Initializes a new instance of the <see cref="EcdsaPublicKey"/> class.
    /// </summary>
    /// <param name="ecdsa">The system ECDSA implementation.</param>
    /// <param name="id">The public key ID. Will be generated if not provided.</param>
    internal EcdsaPublicKey(ECDsa ecdsa, string? id = null)
    {
        AsymmetricAlgorithm = ecdsa;
        PublicKey = ecdsa.ExportSubjectPublicKeyInfo();

        Id = id ?? Convert.ToBase64String(RandomNumberGenerator.GetBytes(IdBytesCount));
    }

    /// <summary>
    /// Gets the public key ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the raw public key bytes.
    /// </summary>
    public byte[] PublicKey { get; }

    internal ECDsa AsymmetricAlgorithm { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    /// <param name="disposing">Whether native resources should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            AsymmetricAlgorithm.Dispose();
        }
    }
}
