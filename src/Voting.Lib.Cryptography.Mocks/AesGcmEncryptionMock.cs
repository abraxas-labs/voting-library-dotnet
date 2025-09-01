// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using Voting.Lib.Common;

namespace Voting.Lib.Cryptography.Mocks;

/// <summary>
/// Mock implementation for AES GCM cryptographic operations.
/// </summary>
public static class AesGcmEncryptionMock
{
    /// <summary>
    /// The tag size for AES-GCM.
    /// </summary>
    public static readonly byte TagSize = 16;

    /// <summary>
    /// The nonce size for AES-GCM.
    /// </summary>
    public static readonly byte NonceSize = 12;

    private static readonly ArrayPool<byte> ByteArrayPool = ArrayPool<byte>.Shared;

    /// <summary>
    /// Mocked implementation for AES GCM encryption.
    /// </summary>
    /// <param name="plainText">The plain text bytes to encrypt.</param>
    /// <param name="keyId">The key id to use for this operation.</param>
    /// <returns>The ciphertext.</returns>
    public static byte[] Encrypt(ReadOnlySpan<byte> plainText, string keyId)
    {
        // Check arguments.
        if (plainText == null || plainText.Length == 0)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        using var aes = new AesGcm(BuildMockKey(keyId), TagSize);

        // Prepare cryptographic parameters
        var cipherTextRent = ByteArrayPool.Rent(plainText.Length);
        var cipherText = cipherTextRent.AsSpan(..plainText.Length);
        Span<byte> tag = stackalloc byte[TagSize];
        Span<byte> nonce = stackalloc byte[NonceSize];
        RandomNumberGenerator.Fill(nonce);

        aes.Encrypt(nonce, plainText, cipherText, tag);

        var cipherData = new ByteConverter(TagSize + cipherText.Length + nonce.Length)
            .Append(tag)
            .Append(cipherText)
            .Append(nonce)
            .GetBytes();

        ByteArrayPool.Return(cipherTextRent);
        return cipherData;
    }

    /// <summary>
    /// Mocked implementation for AES GCM decryption.
    /// </summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <param name="keyId">The key id to use for this operation.</param>
    /// <returns>The plaintext.</returns>
    public static byte[] Decrypt(ReadOnlySpan<byte> cipherText, string keyId)
    {
        // Check arguments.
        if (cipherText == default || cipherText.Length == 0)
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        using var aes = new AesGcm(BuildMockKey(keyId), TagSize);
        var tag = cipherText[..TagSize];
        var cipher = cipherText[TagSize..^NonceSize];
        var nonce = cipherText[^NonceSize..];

        var plainText = new byte[cipher.Length];
        aes.Decrypt(nonce, cipher, tag, plainText);

        return plainText;
    }

    private static byte[] BuildMockKey(string keyId)
    {
        // derive a simple key from the label
        // this is probably not secure,
        // but it is deterministic per keyId
        // and also different for each keyId
        // which is what we need for in our mocked scenarios / tests.
        return SHA256.HashData(Encoding.UTF8.GetBytes(keyId));
    }
}
