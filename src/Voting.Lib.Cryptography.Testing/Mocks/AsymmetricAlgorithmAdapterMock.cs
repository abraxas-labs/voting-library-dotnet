// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;
using Voting.Lib.Cryptography.Asymmetric;

namespace Voting.Lib.Cryptography.Testing.Mocks;

/// <summary>
/// A asymmetric algorithm adapter mock for testing purposes.
/// </summary>
public class AsymmetricAlgorithmAdapterMock : IAsymmetricAlgorithmAdapter<EcdsaPublicKey, EcdsaPrivateKey>
{
    private const int KeyIdMockStartIndex = 47;
    private const int KeyIdMockLength = 24;

    private readonly HashAlgorithmName _hashAlgorithm;
    private readonly string[] _pkcs8PrivateKeyBase64Store =
    {
        "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIBJ6bIsxdpJARUdCgNZWDtYisSSN7UKvBFPxxTPhZfesFsnDO4fNNE5ux99wnQwR3wIqdHR2e8l/xtRBgfhUmUBEehgYkDgYYABAH7PeX89h5QiRJtN2dRsnmB5zkc+REd6o7CXjWo0SkkOELNFLrTe4LHYDn+6VGJXqonfSCiZFndQRFASeoqyPrqrQDmOPDr3VUL1TWuA1OdzSLszZEjqP7JL1gpls45n17c7VZ8fCBoN2kSmHCr3+Iop/d0fCXYV7FPoYBFFFlB4jIY8g==",
        "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIBKYc4H9RTDVYmmvJbWNYIC3wJVMnRqf1lxzZIpULerzFRYDGhtBE8etI947Z7WQGrNMHqc9670DobuTh2HuGIMUShgYkDgYYABACHwt9vtGzkJ1dMqvwtZ5deezgQLzdqjiVkZUokiem/DYAzRaMhuSv2Gj6S4vx/RLBSRJaz3vzB7XG2WCn9YcEl6QEGXMlBiErF1RUcfcZEwwnnZ4Q/3USmt1tWGUZtsdtoOG4YGPc2HW9p/Eqqj51uCFL7t+pN5Ee5MLX3oQMntP9hNg==",
        "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIBIQLdkl8P27rZTH3b4HhNE1D6qFiiWtHm27G1iiap3qRTpPx24SR28deRKqn58JEIxhKKnH9gU9OGKhzgaRgCYTShgYkDgYYABAEO+5lAge7ADqNySpuVa1lMX/CmN/rBa/p4momyzDM5Bpc0oA2P/kWQUCX83cteZ2KKoXNNZUhPfNeCltqU03uI8gGcFy2+WZkZRIbGsmWO4unA3Gy6mzklp6vnnKHf7jlD2fu44MfNSIxyaVjs85Mi2f1yi4PHccTWRvjKFYDx7aMaUw==",
        "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIAAJyr4qcghR3Fg46PAYfPAdZinYISnusV+ah1xoq+pg+gkPrpPlkUUpNYe/RAIErl5S3LNiaOjUeKBNJsmL+eXTihgYkDgYYABAETxWtp5wRbh7AZZCtr3lmot4NQU508rkPTN3Bz9qdEf8uuGXF8zTYgSxS+A/rq52fVRZVB0Hh1SoCDQH8prr2mfgAhJIanufuzSsSysPpuIqlw8l0W2sexINqDYXh3Vs5fdfDUsdU1cfpVAQ56mynfQmpobcKO5reP7Fv+RfRk/Yv7vQ==",
        "MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIBRXwivXbXlWjvjWQPRunrtVe95x+2lfjs7Xyw0zewhbAJ1ecq06PGwFusSTxKejfaG6Xyl0RjW3PXtmlRRfmVZf6hgYkDgYYABADfNUP2xsSP9p4V5ZqLiZ4A29FD4f3YyV3ZEa6T+JoaJOUL6NY5U19RpZPtqnkpmtJpJH38uMTZqU8iiQgkpELMEgFnTrVSnv/jb6ocw3oqmJMuAP7y5Zo/oj5tmQdF0Tv7y/E97tMXYRQe9acMiUWdSgwjEzy2eFcV5ml/LYXuOsPpCg==",
    };

    private int _currentKeyIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsymmetricAlgorithmAdapterMock"/> class.
    /// </summary>
    public AsymmetricAlgorithmAdapterMock()
    {
        _hashAlgorithm = HashAlgorithmName.SHA512;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsymmetricAlgorithmAdapterMock"/> class.
    /// </summary>
    /// <param name="hashAlgorithm">The hash algorithm to use.</param>
    public AsymmetricAlgorithmAdapterMock(HashAlgorithmName hashAlgorithm)
    {
        _hashAlgorithm = hashAlgorithm;
    }

    /// <summary>
    /// Creates a random key from a pool of private keys (not truly random).
    /// </summary>
    /// <returns>Returns a private key.</returns>
    public EcdsaPrivateKey CreateRandomPrivateKey()
    {
        var ecdsa = ECDsa.Create();
        var base64PrivateKey = _pkcs8PrivateKeyBase64Store[_currentKeyIndex];
        ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(base64PrivateKey), out _);
        return new EcdsaPrivateKey(ecdsa, base64PrivateKey.Substring(KeyIdMockStartIndex, KeyIdMockLength));
    }

    /// <inheritdoc />
    public EcdsaPublicKey CreatePublicKey(byte[] publicKey, string keyId)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return new EcdsaPublicKey(ecdsa, keyId);
    }

    /// <inheritdoc />
    public byte[] CreateSignature(byte[] data, EcdsaPrivateKey key)
    {
        return key.AsymmetricAlgorithm.SignData(data, _hashAlgorithm);
    }

    /// <inheritdoc />
    public bool VerifySignature(byte[] data, byte[] signature, EcdsaPublicKey key)
    {
        return key.AsymmetricAlgorithm.VerifyData(data, signature, _hashAlgorithm);
    }

    /// <summary>
    /// Sets the index at which the mock will retrieve the private key from the mocked key store.
    /// </summary>
    /// <param name="index">Key index for key store.</param>
    public void SetNextKeyIndex(int index)
    {
        if (index < 0 || index >= _pkcs8PrivateKeyBase64Store.Length)
        {
            throw new ArgumentException($"Key index {index} is out of range");
        }

        _currentKeyIndex = index;
    }
}
