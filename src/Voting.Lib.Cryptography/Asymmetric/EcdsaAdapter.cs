// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// Ecdsa adapter implementation of an asymmetric (also known as a public key) algorithm.
/// </summary>
public class EcdsaAdapter : IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>
{
    private readonly ECCurve _ellipticCurve = ECCurve.NamedCurves.nistP521;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    /// <inheritdoc />
    public EcdsaPrivateKey CreateRandomPrivateKey()
    {
        var ecdsa = ECDsa.Create(_ellipticCurve);
        return new EcdsaPrivateKey(ecdsa);
    }

    /// <inheritdoc />
    public EcdsaPublicKey CreatePublicKey(byte[] publicKey, string keyId)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return new EcdsaPublicKey(ecdsa, keyId);
    }

    /// <inheritdoc />
    public byte[] CreateSignature(byte[] data, EcdsaPrivateKey key)
    {
        if (data.Length == 0)
        {
            throw new CryptographyException("Data to sign may not be empty");
        }

        return key.AsymmetricAlgorithm.SignData(data, _hashAlgorithm);
    }

    /// <inheritdoc />
    public bool VerifySignature(byte[] data, byte[] signature, EcdsaPublicKey key)
    {
        if (data.Length == 0 || signature.Length == 0)
        {
            throw new CryptographyException("Data or signature may not be empty");
        }

        return key.AsymmetricAlgorithm.VerifyData(data, signature, _hashAlgorithm);
    }
}
