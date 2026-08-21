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
using Voting.Lib.Cryptography.Mocks.Configuration;

namespace Voting.Lib.Cryptography.Mocks;

/// <summary>
/// Mock implementation of a <see cref="ICryptoProvider"/>.
/// </summary>
public class CryptoProviderMock : ICryptoProvider
{
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    private readonly CryptoMockConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="CryptoProviderMock"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public CryptoProviderMock(CryptoMockConfig config)
    {
        _config = config;
    }

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

    /// <summary>
    /// Gets or sets a value indicating whether all operations of this mock should throw an exception (in this case <see cref="CryptographyException"/>).
    /// Useful to simulate a crypto provider outage in tests.
    /// </summary>
    public bool IsOperationFailureEnabled { get; set; }

    /// <summary>
    /// Resets the mutable state of this mock (e.g. <see cref="IsOperationFailureEnabled"/>) back to its defaults.
    /// This mock is typically registered as a singleton, so if your test host/factory is shared across multiple tests,
    /// call this between tests to avoid state leaking between them.
    /// </summary>
    public void Reset()
    {
        IsOperationFailureEnabled = false;
    }

    /// <inheritdoc />
    public Task<byte[]> CreateSignature(byte[] data, string keyId)
    {
        EnsureOperationNotFailing();
        using var key = BuildEcdsaPrivateKey(keyId);
        return Task.FromResult(key.SignData(data, HashAlgorithm));
    }

    /// <inheritdoc />
    public Task<byte[]> CreateEcdsaSha384Signature(byte[] data, string keyId)
    {
        EnsureOperationNotFailing();
        return CreateSignature(data, keyId);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateSignature(IEnumerable<byte[]> bulkData, string keyId)
    {
        EnsureOperationNotFailing();
        using var key = BuildEcdsaPrivateKey(keyId);
        return Task.FromResult<IReadOnlyList<byte[]>>(bulkData.Select(d => key.SignData(d, HashAlgorithm)).ToList());
    }

    /// <inheritdoc />
    public Task<byte[]> CreateHmacSha256(byte[] data, string keyId)
    {
        EnsureOperationNotFailing();
        using var key = BuildHmacSha256SecretKey(keyId);
        return Task.FromResult(key.ComputeHash(data));
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, string keyId)
    {
        EnsureOperationNotFailing();
        return BulkCreateSignature(bulkData, keyId);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkCreateHmacSha256(IEnumerable<byte[]> bulkData, string keyId)
    {
        EnsureOperationNotFailing();
        using var key = BuildHmacSha256SecretKey(keyId);
        return Task.FromResult<IReadOnlyList<byte[]>>(bulkData.Select(key.ComputeHash).ToList());
    }

    /// <inheritdoc />
    public Task<byte[]> EncryptAesGcm(byte[] plainText, string keyId)
    {
        EnsureOperationNotFailing();
        return Task.FromResult(AesGcmEncryptionMock.Encrypt(plainText, keyId));
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<byte[]>> BulkEncryptAesGcm(IEnumerable<byte[]> bulkPlainText, string keyId)
    {
        EnsureOperationNotFailing();
        return Task.FromResult<IReadOnlyList<byte[]>>(bulkPlainText.Select(plainText => AesGcmEncryptionMock.Encrypt(plainText, keyId)).ToList());
    }

    /// <inheritdoc />
    public Task<byte[]> DecryptAesGcm(byte[] cipherText, string keyId)
    {
        EnsureOperationNotFailing();
        return Task.FromResult(AesGcmEncryptionMock.Decrypt(cipherText, keyId));
    }

    /// <inheritdoc />
    public async Task<bool> VerifySignature(byte[] data, byte[] signature, string keyId)
    {
        EnsureOperationNotFailing();
        using var publicKey = await ExportEcdsaPublicKey(keyId);
        return publicKey.AsymmetricAlgorithm.VerifyData(data, signature, HashAlgorithm);
    }

    /// <inheritdoc />
    public Task<bool> VerifyEcdsaSha384Signature(byte[] data, byte[] signature, string keyId)
    {
        EnsureOperationNotFailing();
        return VerifySignature(data, signature, keyId);
    }

    /// <inheritdoc />
    public async Task<bool> VerifyHmacSha256(byte[] data, byte[] hash, string keyId)
    {
        EnsureOperationNotFailing();
        var expectedHash = await CreateHmacSha256(data, keyId);
        return hash.SequenceEqual(expectedHash);
    }

    /// <inheritdoc />
    public Task<EcdsaPublicKey> ExportEcdsaPublicKey(string keyId)
    {
        EnsureOperationNotFailing();
        var privateKeyId = _config.DeriveKeyPairFromKeyIdSegments
            ? keyId.Replace(PublicKeyIdSegment, PrivateKeyIdSegment, StringComparison.Ordinal)
            : keyId;

        using var privateKey = BuildEcdsaPrivateKey(privateKeyId);
        var publicKeyParameters = privateKey.ExportParameters(false);
        return Task.FromResult(new EcdsaPublicKey(ECDsa.Create(publicKeyParameters)));
    }

    /// <inheritdoc />
    public Task<string> GenerateAesSecretKey(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task<string> GetAesSecretKeyId(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task DeleteAesSecretKey(string keyId)
        => DeleteSecretKey();

    /// <inheritdoc />
    public Task<string> GenerateMacSecretKey(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task<string> GetMacSecretKeyId(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task DeleteMacSecretKey(string keyId)
        => DeleteSecretKey();

    /// <inheritdoc />
    public Task<string> GenerateEcdsaSha384SecretKey(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task<string> GetEcdsaSha384SecretKeyId(string keyLabel)
        => ResolveSecretKey(keyLabel);

    /// <inheritdoc />
    public Task DeleteEcdsaSha384Key(string keyId)
        => DeleteSecretKey();

    /// <inheritdoc />
    public Task<bool> IsHealthy(string? keyId = null)
    {
        EnsureOperationNotFailing();
        return Task.FromResult(true);
    }

    private void EnsureOperationNotFailing()
    {
        if (IsOperationFailureEnabled)
        {
            throw new CryptographyException("Simulated crypto provider operation failure.");
        }
    }

    private Task<string> ResolveSecretKey(string keyLabel)
    {
        EnsureOperationNotFailing();
        return Task.FromResult(keyLabel);
    }

    private Task DeleteSecretKey()
    {
        EnsureOperationNotFailing();
        return Task.CompletedTask;
    }

    private ECDsa BuildEcdsaPrivateKey(string keyLabel)
    {
        if (_config.DeriveKeyPairFromKeyIdSegments && !keyLabel.Contains(PrivateKeyIdSegment, StringComparison.Ordinal))
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
