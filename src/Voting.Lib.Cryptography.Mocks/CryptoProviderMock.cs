// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.Mocks;

/// <summary>
/// Mock implementation of a <see cref="ICryptoProvider"/>.
/// </summary>
public class CryptoProviderMock : ICryptoProvider
{
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    /// <summary>
    /// Gets or sets the public key id segment.
    /// To derive the private key, the mock replaces this with <see cref="PrivateKeyIdSegment"/>.
    /// </summary>
    public static string PublicKeyIdSegment { get; set; } = "_PUBLIC_";

    /// <summary>
    /// Gets or sets the private key id segment.
    /// To derive the private key from the public key,
    /// the mock replaces <see cref="PublicKeyIdSegment"/> with <see cref="PrivateKeyIdSegment"/>.
    /// </summary>
    public static string PrivateKeyIdSegment { get; set; } = "_PRIVATE_";

    /// <inheritdoc />
    public Task<byte[]> CreateSignature(byte[] data, string keyId)
    {
        using var key = BuildEcdsaPrivateKey(keyId);
        return Task.FromResult(key.SignData(data, HashAlgorithm));
    }

    /// <inheritdoc />
    public Task<byte[]> CreateEcdsaSha384Signature(byte[] data, string keyId)
        => CreateSignature(data, keyId);

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateSignature(IEnumerable<byte[]> bulkData, string keyId)
    {
        using var key = BuildEcdsaPrivateKey(keyId);
        return Task.FromResult<IReadOnlyList<byte[]>>(bulkData.Select(d => key.SignData(d, HashAlgorithm)).ToList());
    }

    /// <inheritdoc />
    public Task<byte[]> CreateHmacSha256(byte[] data, string keyId)
    {
        using var key = BuildHmacSha256SecretKey(keyId);
        return Task.FromResult(key.ComputeHash(data));
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, string keyId)
        => BulkCreateSignature(bulkData, keyId);

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateHmacSha256(IEnumerable<byte[]> bulkData, string keyId)
    {
        using var key = BuildHmacSha256SecretKey(keyId);
        return Task.FromResult<IReadOnlyList<byte[]>>(bulkData.Select(key.ComputeHash).ToList());
    }

    /// <inheritdoc />
    public Task<byte[]> EncryptAesGcm(byte[] plainText, string keyId)
        => Task.FromResult(AesGcmEncryptionMock.Encrypt(plainText, keyId));

    /// <inheritdoc />
    public Task<byte[]> DecryptAesGcm(byte[] cipherText, string keyId)
        => Task.FromResult(AesGcmEncryptionMock.Decrypt(cipherText, keyId));

    /// <inheritdoc />
    public async Task<bool> VerifySignature(byte[] data, byte[] signature, string keyId)
    {
        using var publicKey = await ExportEcdsaPublicKey(keyId);
        return publicKey.AsymmetricAlgorithm.VerifyData(data, signature, HashAlgorithm);
    }

    /// <inheritdoc />
    public Task<bool> VerifyEcdsaSha384Signature(byte[] data, byte[] signature, string keyId)
        => VerifySignature(data, signature, keyId);

    /// <inheritdoc />
    public async Task<bool> VerifyHmacSha256(byte[] data, byte[] hash, string keyId)
    {
        var expectedHash = await CreateHmacSha256(data, keyId);
        return hash.SequenceEqual(expectedHash);
    }

    /// <inheritdoc />
    public Task<EcdsaPublicKey> ExportEcdsaPublicKey(string keyId)
    {
        var privateKeyId = keyId.Replace(PublicKeyIdSegment, PrivateKeyIdSegment, StringComparison.Ordinal);
        using var privateKey = BuildEcdsaPrivateKey(privateKeyId);
        var publicKeyParameters = privateKey.ExportParameters(false);
        return Task.FromResult(new EcdsaPublicKey(ECDsa.Create(publicKeyParameters)));
    }

    /// <inheritdoc />
    public Task<string> GenerateAesSecretKey(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task<string> GetAesSecretKeyId(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task DeleteAesSecretKey(string keyId) => Task.CompletedTask;

    /// <inheritdoc />
    public Task<string> GenerateMacSecretKey(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task<string> GetMacSecretKeyId(string keyLabel) => Task.FromResult(keyLabel);

    /// <inheritdoc />
    public Task DeleteMacSecretKey(string keyId) => Task.CompletedTask;

    /// <inheritdoc />
    public Task<bool> IsHealthy(string? keyId = null) => Task.FromResult(true);

    private ECDsa BuildEcdsaPrivateKey(string keyLabel)
    {
        if (!keyLabel.Contains(PrivateKeyIdSegment, StringComparison.Ordinal))
        {
            throw new CryptographyException($"A mocked private key id / label always needs to contain {PrivateKeyIdSegment}");
        }

        // derive a simple ecdsa from the label
        // this is probably not secure,
        // but it is deterministic per label
        // and also different for each label
        // which is what we need for in our mocked scenarios / tests.
        var parameters = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            D = SHA256.HashData(Encoding.UTF8.GetBytes(keyLabel)),
        };
        var derivedEcdsa = ECDsa.Create();
        derivedEcdsa.ImportParameters(parameters);
        return derivedEcdsa;
    }

    private HMACSHA256 BuildHmacSha256SecretKey(string keyLabel)
    {
        // derive a simple ecdsa from the label
        // this is probably not secure,
        // but it is deterministic per label
        // and also different for each label
        // which is what we need for in our mocked scenarios / tests.
        var key = SHA256.HashData(Encoding.UTF8.GetBytes(keyLabel));
        return new HMACSHA256(key);
    }
}
