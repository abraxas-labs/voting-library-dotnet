// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Kms.ApiModels;
using Voting.Lib.Cryptography.Kms.Configuration;
using Voting.Lib.Cryptography.Kms.Exceptions;
using Voting.Lib.Cryptography.Kms.Extensions;

namespace Voting.Lib.Cryptography.Kms;

/// <summary>
/// A KMS (key management system) crypto provider implementation
/// for the THALES CipherTrust API.
/// </summary>
public class KmsCryptoProvider : ICryptoProvider
{
    private const int AesIvLength = 12; // AES-GCM standard IV size
    private const int AesTagLength = 16; // AES-GCM standard tag size

    private const string AlgorithmNameAes = "aes";
    private const string AlgorithmNameHmacSha256 = "hmac-sha256";
    private const string AlgorithmNameEc = "ec";
    private const string AlgorithmNameEcSign = "ecdsa";
    private const string AlgorithmNameSha384 = "sha-384";

    private const string ModeGcm = "gcm";
    private const string CurveEcP384 = "secp384r1";

    private const string MediaTypeOctetStream = "application/octet-stream";

    private readonly HttpClient _http;
    private readonly KmsConfig _config;
    private readonly ILogger<KmsCryptoProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="KmsCryptoProvider"/> class.
    /// </summary>
    /// <param name="http">The http provider.</param>
    /// <param name="config">The config to use.</param>
    /// <param name="logger">The logger.</param>
    public KmsCryptoProvider(
        HttpClient http,
        KmsConfig config,
        ILogger<KmsCryptoProvider> logger)
    {
        _http = http;
        _config = config;
        _logger = logger;
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<byte[]> CreateSignature(byte[] data, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<byte[]> CreateEcdsaSha384Signature(byte[] data, string keyId)
    {
        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeOctetStream);

        var url = QueryHelpers.AddQueryString("v1/crypto/sign", new Dictionary<string, string?>
        {
            { "keyName", keyId },
            { "signAlgo", AlgorithmNameEcSign },
            { "hashAlgo", AlgorithmNameSha384 },
        });

        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Content = content;

        using var resp = await _http.SendAsync(req);
        var signResp = await resp.ReadResponse<SignResponse>();
        return Convert.FromHexString(signResp.Data);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<byte[]> CreateHmacSha256(byte[] data, string keyId)
    {
        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeOctetStream);

        using var req = new HttpRequestMessage(HttpMethod.Post, $"v1/crypto/mac?keyName={keyId}");
        req.Content = content;

        using var resp = await _http.SendAsync(req);
        var macResp = await resp.ReadResponse<MacResponse>();
        return Convert.FromBase64String(macResp.Data);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<IReadOnlyList<byte[]>> BulkCreateSignature(IEnumerable<byte[]> bulkData, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<IReadOnlyList<byte[]>> BulkCreateEcdsaSha384Signature(IEnumerable<byte[]> bulkData, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<IReadOnlyList<byte[]>> BulkCreateHmacSha256(IEnumerable<byte[]> bulkData, string keyId)
    {
        var tasks = bulkData.Select(x => CreateHmacSha256(x, keyId));
        return await Task.WhenAll(tasks);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<byte[]> EncryptAesGcm(byte[] plainText, string keyId)
    {
        var req = new EncryptRequest(keyId, Convert.ToBase64String(plainText), ModeGcm);
        var resp = await _http.PostJson<EncryptRequest, EncryptResponse>("v1/crypto/encrypt", req);

        var cipherText = Convert.FromBase64String(resp.Ciphertext);
        var iv = Convert.FromBase64String(resp.Iv);
        var tag = Convert.FromBase64String(resp.Tag);

        if (iv.Length != AesIvLength)
        {
            throw new KmsException($"Unexpected IV length, expected = {AesIvLength}, actual = {iv.Length}");
        }

        if (tag.Length != AesTagLength)
        {
            throw new KmsException($"Unexpected tag length, expected = {AesTagLength}, actual = {tag.Length}");
        }

        var result = new byte[iv.Length + cipherText.Length + tag.Length];

        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(cipherText, 0, result, iv.Length, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, iv.Length + cipherText.Length, tag.Length);
        return result;
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<IReadOnlyList<byte[]>> BulkEncryptAesGcm(IEnumerable<byte[]> bulkPlainText, string keyId)
    {
        var tasks = bulkPlainText.Select(x => EncryptAesGcm(x, keyId));
        return await Task.WhenAll(tasks);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<byte[]> DecryptAesGcm(byte[] cipherText, string keyId)
    {
        var (iv, text, tag) = SplitAesCipherText(cipherText);
        var req = new DecryptRequest(
            keyId,
            Convert.ToBase64String(text),
            Convert.ToBase64String(tag),
            Convert.ToBase64String(iv),
            ModeGcm);
        var resp = await _http.PostJson<DecryptRequest, DecryptResponse>("v1/crypto/decrypt", req);
        return Convert.FromBase64String(resp.Plaintext);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<bool> VerifySignature(byte[] data, byte[] signature, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<bool> VerifyEcdsaSha384Signature(byte[] data, byte[] signature, string keyId)
    {
        using var content = new ByteArrayContent(data);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeOctetStream);

        var publicKeyName = $"{keyId}-pub";
        var signatureHexEncoded = Convert.ToHexString(signature);

        var url = QueryHelpers.AddQueryString("v1/crypto/signv", new Dictionary<string, string?>
        {
            { "keyName", publicKeyName },
            { "signAlgo", AlgorithmNameEcSign },
            { "hashAlgo", AlgorithmNameSha384 },
            { "signature", signatureHexEncoded },
        });

        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Content = content;

        using var resp = await _http.SendAsync(req);
        var verifyResponse = await resp.ReadResponse<VerifyResponse>();
        return verifyResponse.Verified;
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<bool> VerifyHmacSha256(byte[] data, byte[] hash, string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<EcdsaPublicKey> ExportEcdsaPublicKey(string keyId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<string> GenerateAesSecretKey(string keyLabel)
    {
        try
        {
            var key = await _http.PostJson<CreateKeyRequest, KeyModel>(
                "v1/vault/keys2",
                new CreateKeyRequest(
                    keyLabel,
                    KeyUsageMask.EncryptAndDecrypt,
                    AlgorithmNameAes,
                    _config.AesKeyLabels,
                    _config.AesKeySize));
            return key.Id;
        }
        catch (KmsException e) when (e.Code == KmsKeyAlreadyExistsException.KmsErrorCode)
        {
            throw new KmsKeyAlreadyExistsException(keyLabel, e.Message);
        }
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<string> GenerateEcdsaSha384SecretKey(string keyLabel)
    {
        try
        {
            var key = await _http.PostJson<CreateKeyRequest, KeyModel>(
                "v1/vault/keys2",
                new CreateKeyRequest(
                    keyLabel,
                    KeyUsageMask.SignAndVerify,
                    AlgorithmNameEc,
                    _config.EcdsaSha384KeyLabels,
                    CurveId: CurveEcP384));

            // sign calls use the name of the key instead of the id
            return key.Name;
        }
        catch (KmsException e) when (e.Code == KmsKeyAlreadyExistsException.KmsErrorCode)
        {
            throw new KmsKeyAlreadyExistsException(keyLabel, e.Message);
        }
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<string> GetAesSecretKeyId(string keyLabel)
    {
        var resp = await _http.GetJson<Page<KeyModel>>($"v1/vault/keys2?limit=1&name={WebUtility.UrlEncode(keyLabel)}");
        return resp.Resources?.FirstOrDefault()?.Id ?? throw new KmsException("Key not found");
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<string> GetEcdsaSha384SecretKeyId(string keyLabel)
    {
        // sign calls use the name of the key instead of the id
        return Task.FromResult(keyLabel);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task DeleteAesSecretKey(string keyId)
        => DeleteKey(keyId);

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task DeleteEcdsaSha384Key(string keyId)
        => DeleteKey(keyId);

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<string> GenerateMacSecretKey(string keyLabel)
    {
        try
        {
            var key = await _http.PostJson<CreateKeyRequest, KeyModel>(
                "v1/vault/keys2",
                new CreateKeyRequest(
                    keyLabel,
                    KeyUsageMask.Mac,
                    AlgorithmNameHmacSha256,
                    _config.MacKeyLabels));

            // hmac calls use the name of the key instead of the id
            return key.Name;
        }
        catch (KmsException e) when (e.Code == KmsKeyAlreadyExistsException.KmsErrorCode)
        {
            throw new KmsKeyAlreadyExistsException(keyLabel, e.Message);
        }
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task<string> GetMacSecretKeyId(string keyLabel)
    {
        // hmac calls use the name of the key instead of the id
        return Task.FromResult(keyLabel);
    }

    /// <inheritdoc cref="ICryptoProvider"/>
    public Task DeleteMacSecretKey(string keyId)
        => DeleteKey(keyId);

    /// <inheritdoc cref="ICryptoProvider"/>
    public async Task<bool> IsHealthy(string? keyId = null)
    {
        if (keyId != null)
        {
            throw new NotImplementedException();
        }

        try
        {
            await _http.GetJson<Page<KeyModel>>("v1/vault/keys2?limit=1");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not list kms keys, healthcheck failed.");
            return false;
        }
    }

    private static (byte[] Iv, byte[] Ciphertext, byte[] Tag) SplitAesCipherText(byte[] combined)
    {
        if (combined.Length < AesIvLength + AesTagLength)
        {
            throw new ArgumentException("Invalid cipherText length.");
        }

        var iv = new byte[AesIvLength];
        var tag = new byte[AesTagLength];
        var cipherText = new byte[combined.Length - AesIvLength - AesTagLength];

        Buffer.BlockCopy(combined, 0, iv, 0, AesIvLength);
        Buffer.BlockCopy(combined, AesIvLength, cipherText, 0, cipherText.Length);
        Buffer.BlockCopy(combined, AesIvLength + cipherText.Length, tag, 0, AesTagLength);

        return (iv, cipherText, tag);
    }

    private Task DeleteKey(string keyId) =>
        _http.DeleteAsync($"v1/vault/keys2/{WebUtility.UrlEncode(keyId)}");
}
