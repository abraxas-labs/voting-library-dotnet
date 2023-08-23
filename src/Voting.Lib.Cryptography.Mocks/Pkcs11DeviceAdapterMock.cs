// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
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

    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;
    private static readonly byte[] Pkcs8PrivateKeyBytes = Convert.FromBase64String(Pkcs8PrivateKey);

    /// <inheritdoc />
    public byte[] CreateSignature(byte[] data, Pkcs11Config? pkcs11Config = null)
    {
        using var key = BuildPrivateKey();
        return key.SignData(data, HashAlgorithm);
    }

    /// <inheritdoc />
    public byte[] CreateEcdsaSha384Signature(byte[] data, Pkcs11Config? pkcs11Config = null)
        => CreateSignature(data, pkcs11Config);

    /// <inheritdoc />
    public IReadOnlyList<byte[]> BulkCreateSignature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
    {
        using var key = BuildPrivateKey();
        return bulkData.Select(d => key.SignData(d, HashAlgorithm)).ToList();
    }

    /// <inheritdoc />
    public IReadOnlyList<byte[]> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, Pkcs11Config? pkcs11Config = null)
        => BulkCreateSignature(bulkData, pkcs11Config);

    /// <inheritdoc />
    public byte[] EncryptAes(byte[] plainText, Pkcs11Config pkcs11Config)
        => AesGcmEncryptionMock.Encrypt(plainText);

    /// <inheritdoc />
    public byte[] DecryptAes(byte[] cipherText, Pkcs11Config pkcs11Config)
        => AesGcmEncryptionMock.Decrypt(cipherText);

    /// <inheritdoc />
    public bool VerifySignature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
    {
        using var publicKey = ExportEcdsaPublicKey(pkcs11Config);
        return publicKey.AsymmetricAlgorithm.VerifyData(data, signature, HashAlgorithm);
    }

    /// <inheritdoc />
    public bool VerifyEcdsaSha384Signature(byte[] data, byte[] signature, Pkcs11Config? pkcs11Config = null)
        => VerifySignature(data, signature, pkcs11Config);

    /// <inheritdoc />
    public EcdsaPublicKey ExportEcdsaPublicKey(Pkcs11Config? pkcs11Config = null)
    {
        using var privateKey = BuildPrivateKey();
        var publicKeyParameters = privateKey.ExportParameters(false);
        return new EcdsaPublicKey(ECDsa.Create(publicKeyParameters));
    }

    /// <inheritdoc />
    public bool IsHealthy(Pkcs11Config? pkcs11Config = null) => true;

    private ECDsa BuildPrivateKey()
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(Pkcs8PrivateKeyBytes, out _);
        return ecdsa;
    }
}
