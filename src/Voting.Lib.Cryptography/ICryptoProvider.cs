// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Lib.Cryptography.Asymmetric;

namespace Voting.Lib.Cryptography;

/// <summary>
/// Interface for a provider of cryptographic operations.
/// </summary>
public interface ICryptoProvider
{
    /// <summary>
    /// Signs <paramref name="data"/> with the configured private key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A signature.</returns>
    Task<byte[]> CreateSignature(byte[] data, string keyId);

    /// <summary>
    /// Signs <paramref name="data"/> with the configured ECDSA SHA384 private key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A ECDSA SHA384 signature.</returns>
    Task<byte[]> CreateEcdsaSha384Signature(byte[] data, string keyId);

    /// <summary>
    /// Hashes <paramref name="data"/> with the configured HMAC secret key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A HMAC SHA256 signature.</returns>
    Task<byte[]> CreateHmacSha256(byte[] data, string keyId);

    /// <summary>
    /// Signs each element in <paramref name="bulkData"/> with the configured private key which is stored in the device.
    /// </summary>
    /// <param name="bulkData">The bulk data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The signatures of the bulk data.</returns>
    Task<IReadOnlyList<byte[]>> BulkCreateSignature(IEnumerable<byte[]> bulkData, string keyId);

    /// <summary>
    /// Signs each element in <paramref name="bulkData"/> with the configured ECDSA SHA384 private key which is stored in the device.
    /// </summary>
    /// <param name="bulkData">The bulk data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The ECDSA ECDSA SHA384 signatures of the bulk data.</returns>
    Task<IReadOnlyList<byte[]>> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, string keyId);

    /// <summary>
    /// Hashes each element in <paramref name="bulkData"/> with the configured HMAC secret key which is stored in the device.
    /// </summary>
    /// <param name="bulkData">The bulk data to sign.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The HMAC SHA256 signatures of the bulk data.</returns>
    Task<IReadOnlyList<byte[]>> BulkCreateHmacSha256(IEnumerable<byte[]> bulkData, string keyId);

    /// <summary>
    /// Encrypts <paramref name="plainText"/> per AES-GCM with the configured AES MAC private/secret key which is stored in the device.
    /// </summary>
    /// <param name="plainText">The plain text to encrypt.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The encrypted cipher represented as a concatenation of the tag, cipher text and nonce.</returns>
    Task<byte[]> EncryptAesGcm(byte[] plainText, string keyId);

    /// <summary>
    /// Encrypts each element in <paramref name="bulkPlainText"/> per AES-GCM with the configured AES MAC private/secret key which is stored in the device.
    /// </summary>
    /// <param name="bulkPlainText">The bulk plain text to encrypt.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The encrypted ciphers represented as a concatenation of the tag, cipher text and nonce.</returns>
    Task<IReadOnlyList<byte[]>> BulkEncryptAesGcm(IEnumerable<byte[]> bulkPlainText, string keyId);

    /// <summary>
    /// Decrypts <paramref name="cipherText"/> per AES-GCM with the configured AES MAC private/secret key which is stored in the device.
    /// </summary>
    /// <param name="cipherText">The cipher to decrypt represented as a concenation of the tag, cipher text and nonce.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The plain text.</returns>
    Task<byte[]> DecryptAesGcm(byte[] cipherText, string keyId);

    /// <summary>
    /// Verifies that <paramref name="signature"/> is created by the configured public key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A result whether the signature is valid.</returns>
    Task<bool> VerifySignature(byte[] data, byte[] signature, string keyId);

    /// <summary>
    /// Verifies that ECDSA SHA384 <paramref name="signature"/> is created by the configured public key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A result whether the signature is valid.</returns>
    Task<bool> VerifyEcdsaSha384Signature(byte[] data, byte[] signature, string keyId);

    /// <summary>
    /// Verifies that HMAC SHA256 <paramref name="hash"/> is created by the configured secret key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="hash">The signature.</param>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>A result whether the signature is valid.</returns>
    Task<bool> VerifyHmacSha256(byte[] data, byte[] hash, string keyId);

    /// <summary>
    /// Exports a public key of the crypto provider.
    /// The public key is cached for the instance lifetime of the implementation.
    /// </summary>
    /// <param name="keyId">The id of the key to use for this operation.</param>
    /// <returns>The exported public key.</returns>
    Task<EcdsaPublicKey> ExportEcdsaPublicKey(string keyId);

    /// <summary>
    /// Generates an AES secret key and stores it in the crypto provider.
    /// </summary>
    /// <param name="keyLabel">The key id.</param>
    /// <returns>The keyId generated.</returns>
    Task<string> GenerateAesSecretKey(string keyLabel);

    /// <summary>
    /// Generates an ECDSA SHA384 secret key and stores it in the crypto provider.
    /// </summary>
    /// <param name="keyLabel">The key id.</param>
    /// <returns>The keyId generated.</returns>
    Task<string> GenerateEcdsaSha384SecretKey(string keyLabel);

    /// <summary>
    /// Resolves the keyId of a AES secret key.
    /// </summary>
    /// <param name="keyLabel">The label of the key.</param>
    /// <returns>The keyId.</returns>
    Task<string> GetAesSecretKeyId(string keyLabel);

    /// <summary>
    /// Resolves the keyId of a ECDSA SHA384 secret key.
    /// </summary>
    /// <param name="keyLabel">The label of the key.</param>
    /// <returns>The keyId.</returns>
    Task<string> GetEcdsaSha384SecretKeyId(string keyLabel);

    /// <summary>
    /// Deletes a aes secret key in the crypto provider.
    /// </summary>
    /// <param name="keyId">The key id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAesSecretKey(string keyId);

    /// <summary>
    /// Deletes a ECDSA SHA384 secret key in the crypto provider.
    /// </summary>
    /// <param name="keyId">The key id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteEcdsaSha384Key(string keyId);

    /// <summary>
    /// Generates a mac secret key and stores it in the crypto provider.
    /// </summary>
    /// <param name="keyLabel">The key id.</param>
    /// <returns>The keyId generated.</returns>
    Task<string> GenerateMacSecretKey(string keyLabel);

    /// <summary>
    /// Resolves the keyId of a MAC secret key.
    /// </summary>
    /// <param name="keyLabel">The label of the key.</param>
    /// <returns>The keyId.</returns>
    Task<string> GetMacSecretKeyId(string keyLabel);

    /// <summary>
    /// Deletes a mac secret key in the crypto provider.
    /// </summary>
    /// <param name="keyId">The key id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteMacSecretKey(string keyId);

    /// <summary>
    /// Gets the health status of the crypto provider.
    /// </summary>
    /// <param name="keyId">The keyId to use for this operation or <c>null</c> if the key of the default configuration should be used.</param>
    /// <returns><c>true</c> if the crypto provider is healthy.</returns>
    Task<bool> IsHealthy(string? keyId = null);
}
