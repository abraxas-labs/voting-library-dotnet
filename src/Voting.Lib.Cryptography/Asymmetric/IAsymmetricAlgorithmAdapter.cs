// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// Interface for an adapter of an asymmetric (also known as a public key) algorithm.
/// </summary>
/// <typeparam name="TPublicKey">Type of public key.</typeparam>
/// <typeparam name="TPrivateKey">Type of private key.</typeparam>
public interface IAsymmetricAlgorithmAdapter<TPublicKey, TPrivateKey>
    where TPublicKey : IPublicKey
    where TPrivateKey : IPrivateKey
{
    /// <summary>
    /// Creates a new <typeparamref name="TPrivateKey"/> with a random private and public key.
    /// </summary>
    /// <returns>A <typeparamref name="TPrivateKey"/>.</returns>
    TPrivateKey CreateRandomPrivateKey();

    /// <summary>
    /// Creates a new <typeparamref name="TPublicKey"/> by the provided public key.
    /// </summary>
    /// <param name="publicKey">Public key bytes in X509.SubjectPublicKeyInfo format.</param>
    /// <param name="keyId">The key id.</param>
    /// <returns>A <typeparamref name="TPublicKey"/>.</returns>
    TPublicKey CreatePublicKey(byte[] publicKey, string keyId);

    /// <summary>
    /// Signs <paramref name="data"/> with an asymmetric algorithm which uses the private key component of the <paramref name="key"/>.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="key">The private key.</param>
    /// <returns>A signature.</returns>
    byte[] CreateSignature(byte[] data, TPrivateKey key);

    /// <summary>
    /// Verifies that <paramref name="signature"/> is created by <paramref name="data"/> signed by <paramref name="key"/>.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="key">The public key.</param>
    /// <returns>A result whether the signature is valid.</returns>
    bool VerifySignature(byte[] data, byte[] signature, TPublicKey key);
}
