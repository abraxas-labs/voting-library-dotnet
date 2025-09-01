// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;
using FluentAssertions;
using Voting.Lib.Cryptography.Asymmetric;
using Xunit;

namespace Voting.Lib.Cryptography.Test.Asymmetric;

public class EcdsaAdapterTest
{
    private static readonly byte[] Plaintext = "fooBar"u8.ToArray();
    private static readonly byte[] Signature = Convert.FromBase64String("ADNxcDQzvZ633B+t5ucPfRDyCVQbOlxIG5pLSuAKpH5sQKsFJ3Ni8PFrqlb8aJuT3bgcYZry2Js8a3ewnuhTHnlPAQa1SYmuLMzsB5kjiktpDVaoVlJOoNL+Fzjzm+eHH9vHyo2zY4x2FepBQiXc39Rwg24DtfHPAcum5JpZrTceGqVr");
    private static readonly byte[] PubKey = Convert.FromBase64String("MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQAsU481eluXZWvdzIqoRUFRxH5Ecqx4NBeQVnUJPaUwHUkzvL6rlTZfXdnEstSKbhbUrQK6MOtIUZEf5eKtIMZrlgApDsZoCINmIcDxXDDHO/Sv478D3tYf2YrTW0luF6PnOmbUrzaafyC6u8QfypjzQuVgdUBV6hVzhqcUFRrDK9HC9A=");

    [Fact]
    public void VerifyPersistedSignatureShouldWork()
    {
        var adapter = new EcdsaAdapter();
        using var publicKey = adapter.CreatePublicKey(PubKey, "myKeyId");
        adapter.VerifySignature(Plaintext, Signature, publicKey)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void SignAndVerifyRandomShouldWork()
    {
        var plaintext = RandomNumberGenerator.GetBytes(32);

        var adapter = new EcdsaAdapter();
        using var privateKey = adapter.CreateRandomPrivateKey();
        var signature = adapter.CreateSignature(plaintext, privateKey);
        adapter.VerifySignature(plaintext, signature, privateKey)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void SignAndVerifyModifiedShouldFail()
    {
        var plaintext = RandomNumberGenerator.GetBytes(32);

        var adapter = new EcdsaAdapter();
        using var privateKey = adapter.CreateRandomPrivateKey();
        var signature = adapter.CreateSignature(plaintext, privateKey);

        var modifiedPlaintext = RandomNumberGenerator.GetBytes(32);
        adapter.VerifySignature(modifiedPlaintext, signature, privateKey)
            .Should()
            .BeFalse();
    }
}
