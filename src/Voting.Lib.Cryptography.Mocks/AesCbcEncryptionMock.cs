// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Voting.Lib.Cryptography.Mocks;

/// <summary>
/// Mock-Implementation of an adapter of Aes encryption.
/// </summary>
public static class AesCbcEncryptionMock
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("MySecretKey12345MySecretKey12345"); // the key to use for encryption
    private static readonly byte[] Iv = Encoding.UTF8.GetBytes("1234567890123456"); // the initialization vector to use for encryption

    /// <summary>
    /// Mock encrypt.
    /// </summary>
    /// <param name="clearBytes">The bytes to encrypt.</param>
    /// <returns>The ciphertext.</returns>
    public static byte[] Encrypt(byte[] clearBytes)
    {
        byte[] encryptedBytes = EncryptStringToBytes_Aes(clearBytes, Key, Iv);
        return encryptedBytes;
    }

    /// <summary>
    /// Mock decrypt.
    /// </summary>
    /// <param name="ciphertext">The ciphertext.</param>
    /// <returns>The plaintext.</returns>
    public static byte[] Decrypt(byte[] ciphertext)
    {
        var decryptedText = DecryptStringFromBytes_Aes(ciphertext, Key, Iv); // decrypt the bytes using AES
        return decryptedText;
    }

    private static byte[] EncryptStringToBytes_Aes(byte[] plainText, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException("plainText");
        }

        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }

        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("iv");
        }

        // Create an AesManaged object
        // with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        aesAlg.Padding = PaddingMode.None;

        // Create an encryptor to perform the stream transform.
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for encryption.
        using MemoryStream msEncrypt = new MemoryStream();
        using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        {
            // Write all data to the stream.
            // swEncrypt.Write(Encoding.ASCII.GetBytes(plainText));
            swEncrypt.BaseStream.Write(plainText, 0, plainText.Length);
        }

        var encrypted = msEncrypt.ToArray();

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    private static byte[] DecryptStringFromBytes_Aes(byte[] cipherBytes, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (cipherBytes == null || cipherBytes.Length <= 0)
        {
            throw new ArgumentNullException("cipherBytes");
        }

        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }

        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("iv");
        }

        // Create an AesManaged object
        // with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        aesAlg.Padding = PaddingMode.None;

        // Create a decryptor to perform the stream transform.
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using MemoryStream msDecrypt = new MemoryStream(cipherBytes);
        using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new StreamReader(csDecrypt);

        using var memoryStream = new MemoryStream();
        srDecrypt.BaseStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
