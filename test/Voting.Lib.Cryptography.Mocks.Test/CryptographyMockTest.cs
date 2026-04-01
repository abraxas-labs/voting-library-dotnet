// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cryptography.Mocks.Test;

public class CryptographyMockTest
{
    [Fact]
    public async Task HmacRoundtripShouldWork()
    {
        var testee = BuildCryptoProviderMock();
        var keyLabel = "hmac-secret-key";

        await testee.GenerateMacSecretKey(keyLabel);

        var plainText = new byte[] { 17, 12, 21, 51, 53, 5, 9 };

        var hash = await testee.CreateHmacSha256(plainText, keyLabel);
        (await testee.VerifyHmacSha256(plainText, hash, keyLabel)).Should().BeTrue();

        await testee.DeleteMacSecretKey(keyLabel);
    }

    [Fact]
    public async Task AesRoundtripShouldWork()
    {
        var testee = BuildCryptoProviderMock();
        var aesSecretKeyLabel = "aes-secret-key";
        await testee.GenerateAesSecretKey(aesSecretKeyLabel);

        var plainText = new byte[] { 121, 12, 67, 55, 14, 37, 12, 15 };

        var cipherText = await testee.EncryptAesGcm(plainText, aesSecretKeyLabel);
        var decryptedCipherText = await testee.DecryptAesGcm(cipherText, aesSecretKeyLabel);

        plainText.SequenceEqual(decryptedCipherText).Should().BeTrue();

        await testee.DeleteAesSecretKey(aesSecretKeyLabel);
    }

    [Fact]
    public async Task SignVerifyRoundtripShouldWork()
    {
        var testee = BuildCryptoProviderMock();
        var privateKey = "my_PRIVATE_key";
        var publicKey = "my_PUBLIC_key";

        var content = new byte[] { 17, 12, 21, 51, 53, 5, 9 };
        var signature = await testee.CreateSignature(content, privateKey);
        var ok = await testee.VerifySignature(content, signature, publicKey);
        ok.Should().BeTrue();
    }

    [Fact]
    public async Task SignVerifyRoundtripWithoutKeyPairDerivationShouldWork()
    {
        var testee = BuildCryptoProviderMock(false);
        var key = "my_key";

        var content = new byte[] { 17, 12, 21, 51, 53, 5, 9 };
        var signature = await testee.CreateSignature(content, key);
        var ok = await testee.VerifySignature(content, signature, key);
        ok.Should().BeTrue();
    }

    private CryptoProviderMock BuildCryptoProviderMock(bool deriveKeyPairFromKeyIdSegments = true)
    {
        return new(new() { DeriveKeyPairFromKeyIdSegments = deriveKeyPairFromKeyIdSegments });
    }
}
