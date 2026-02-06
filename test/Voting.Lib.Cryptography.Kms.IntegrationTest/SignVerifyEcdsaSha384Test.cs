// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class SignVerifyEcdsaSha384Test : BaseKmsIntegrationTest
{
    [Fact]
    public async Task SignAndVerifyWithExistingKeyShouldWork()
    {
        var keyId = await GetOrCreateEcdsaSha384Key(nameof(SignAndVerifyWithExistingKeyShouldWork));

        var plaintext = "The quick brown fox jumps over the lazy dog"u8.ToArray();
        var signature = await CryptoProvider.CreateEcdsaSha384Signature(plaintext, keyId);
        var verified = await CryptoProvider.VerifyEcdsaSha384Signature(plaintext, signature, keyId);
        verified.Should().BeTrue();
    }

    [Fact]
    public async Task RoundtripEcdsaSha384ShouldWork()
    {
        var keyId = await CreateEcdsaSha384Key(nameof(RoundtripEcdsaSha384ShouldWork));

        var plaintext = "The quick brown fox jumps over the lazy dog"u8.ToArray();
        var signature = await CryptoProvider.CreateEcdsaSha384Signature(plaintext, keyId);
        var verified = await CryptoProvider.VerifyEcdsaSha384Signature(plaintext, signature, keyId);
        verified.Should().BeTrue();

        // decrypt with other key should fail
        var keyId2 = await CreateEcdsaSha384Key(nameof(RoundtripEcdsaSha384ShouldWork) + "2");
        var verified2 = await CryptoProvider.VerifyEcdsaSha384Signature(plaintext, signature, keyId2);
        verified2.Should().BeFalse();
    }
}
