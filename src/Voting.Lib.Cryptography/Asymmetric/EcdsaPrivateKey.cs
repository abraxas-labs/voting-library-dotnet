// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// An ECDSA private key.
/// </summary>
public class EcdsaPrivateKey : EcdsaPublicKey, IPrivateKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EcdsaPrivateKey"/> class.
    /// </summary>
    /// <param name="ecdsa">The system ECDSA implementation.</param>
    /// <param name="id">The key id. If it is not provided, a random value is generated.</param>
    internal EcdsaPrivateKey(ECDsa ecdsa, string? id = null)
        : base(ecdsa, id)
    {
        PrivateKey = ecdsa.ExportPkcs8PrivateKey();
    }

    /// <summary>
    /// Gets the raw private key bytes.
    /// </summary>
    public byte[] PrivateKey { get; }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CryptographicOperations.ZeroMemory(PrivateKey);
        }

        base.Dispose(disposing);
    }
}
