// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Cryptography.Configuration;

namespace Voting.Lib.Cryptography.Asymmetric;

/// <summary>
/// Interface for an adapter of a PKCS#11 cryptographic device.
/// </summary>
public interface IPkcs11DeviceAdapter
{
    /// <summary>
    /// Signs <paramref name="data"/> with the configured private key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>A signature.</returns>
    byte[] CreateSignature(byte[] data, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Signs <paramref name="data"/> with the configured ECDSA SHA384 private key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>A ECDSA SHA384 signature.</returns>
    byte[] CreateEcdsaSha384Signature(byte[] data, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Signs each element in <paramref name="bulkData"/> with the configured private key which is stored in the device.
    /// </summary>
    /// <param name="bulkData">The bulk data to sign.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>The signatures of the bulk data.</returns>
    IReadOnlyList<byte[]> BulkCreateSignature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Signs each element in <paramref name="bulkData"/> with the configured ECDSA SHA384 private key which is stored in the device.
    /// </summary>
    /// <param name="bulkData">The bulk data to sign.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>The ECDSA ECDSA SHA384 signatures of the bulk data.</returns>
    IReadOnlyList<byte[]> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Encrypts <paramref name="plainText"/> with the configured AES MAC private/secret key which is stored in the device.
    /// </summary>
    /// <param name="plainText">The cipher to encrypt.</param>
    /// <param name="pkcs11Config">The configuration for encryption.</param>
    /// <returns>The encrypted cipher represented as a concenation of the tag, cipher text and nonce.</returns>
    byte[] EncryptAes(byte[] plainText, Pkcs11Config pkcs11Config);

    /// <summary>
    /// Decrypts <paramref name="cipherText"/> with the configured AES MAC private/secret key which is stored in the device.
    /// </summary>
    /// <param name="cipherText">The cipher to decrypt represented as a concenation of the tag, cipher text and nonce.</param>
    /// <param name="pkcs11Config">The configuration for decryption.</param>
    /// <returns>The plain text.</returns>
    byte[] DecryptAes(byte[] cipherText, Pkcs11Config pkcs11Config);

    /// <summary>
    /// Verifies that <paramref name="signature"/> is created by the configured public key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>A result whether the signature is valid.</returns>
    bool VerifySignature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Verifies that ECDSA SHA384 <paramref name="signature"/> is created by the configured public key which is stored in the device.
    /// </summary>
    /// <param name="data">The data to sign.</param>
    /// <param name="signature">The signature.</param>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>A result whether the signature is valid.</returns>
    bool VerifyEcdsaSha384Signature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Exports a public key of the hsm.
    /// The public key is cached for the instance lifetime of the implementation.
    /// </summary>
    /// <param name="pkcs11Config">The config to use.</param>
    /// <returns>The exported public key.</returns>
    EcdsaPublicKey ExportEcdsaPublicKey(Pkcs11Config? pkcs11Config = null);

    /// <summary>
    /// Gets the health status of the PKCS#11 device connection.
    /// </summary>
    /// <param name="pkcs11Config">The configuration for sign.</param>
    /// <returns>Returns whether a connection to the PKCS#11 device could be established and the login is successful.</returns>
    bool IsHealthy(Pkcs11Config? pkcs11Config = null);
}
