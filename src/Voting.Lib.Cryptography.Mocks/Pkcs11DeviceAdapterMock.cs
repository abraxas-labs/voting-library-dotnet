// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Configuration;

namespace Voting.Lib.Cryptography.Mocks;

/// <summary>
/// Implementation of an adapter of a PKCS#11 cryptographic device.
/// </summary>
public class Pkcs11DeviceAdapterMock : IPkcs11DeviceAdapter
{
    private const string Pkcs8PrivateKey = "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIB5AzPr3zQuJ8s4tf+ayKq0UCyP0Npik8DvM4PePDu4nCWRAbCFm/SO9cP/dJhp4EypibpgT/HcPOMeSSVbXe2FayhgYkDgYYABAAOBqB7U2x8yCtJ1/ckvfVHazq2EKCOGC/veG14qVc6jXzd6FtLg/jD5+SF44+h3sars9rEAw4bWAogoXZyB5ouTQEQy3H0Te2mW9Is57rwybKUS2dAO21glBkqYE0aRPwswEBqWzSzZtB/lj60rdZUKRwBbJoFrUtDe27FypmsH623KQ==";

    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    private byte[] Pkcs8PrivateKeyBytes => Convert.FromBase64String(Pkcs8PrivateKey);

    /// <inheritdoc />
    public byte[] CreateSignature(byte[] data, Pkcs11Config? pkcs11Config = null)
    {
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(Pkcs8PrivateKeyBytes, out _);
        return ecdsa.SignData(data, _hashAlgorithm);
    }

    /// <inheritdoc />
    public byte[] CreateEcdsaSha384Signature(byte[] data, Pkcs11Config? pkcs11Config = null)
    {
        return this.CreateSignature(data, pkcs11Config);
    }

    /// <inheritdoc />
    public IEnumerable<byte[]> BulkCreateSignature(ICollection<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(Pkcs8PrivateKeyBytes, out _);

        foreach (var data in bulkData)
        {
            yield return ecdsa.SignData(data, _hashAlgorithm);
        }
    }

    /// <inheritdoc />
    public IEnumerable<byte[]> BulkCreateEcdsaSha384Signature(ICollection<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        return this.BulkCreateSignature(bulkData, pkcs11Config);
    }

    /// <inheritdoc />
    public byte[] EncryptAesMac(byte[] cipher, Pkcs11Config pkcs11Config)
    {
        return AesCbcEncryptionMock.Encrypt(cipher);
    }

    /// <inheritdoc />
    public byte[] DecryptAesMac(byte[] encryptedCipher, Pkcs11Config pkcs11Config)
    {
        return AesCbcEncryptionMock.Decrypt(encryptedCipher);
    }

    /// <inheritdoc />
    public bool VerifySignature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
    {
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(Pkcs8PrivateKeyBytes, out _);
        return ecdsa.VerifyData(data, signature, _hashAlgorithm);
    }

    /// <inheritdoc />
    public bool VerifyEcdsaSha384Signature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
    {
        return VerifySignature(data, signature, pkcs11Config);
    }

    /// <inheritdoc />
    public bool IsHealthy(Pkcs11Config? pkcs11Config = null) => true;
}
