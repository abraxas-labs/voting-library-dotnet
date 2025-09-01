// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cryptography.Mocks.Test;

public class CryptographyMockTest
{
    private readonly CryptoProviderMock _testee = new();

    [Fact]
    public async Task HmacRoundtripShouldWork()
    {
        var keyLabel = "hmac-secret-key";

        await _testee.GenerateMacSecretKey(keyLabel);

        var plainText = new byte[] { 17, 12, 21, 51, 53, 5, 9 };

        var hash = await _testee.CreateHmacSha256(plainText, keyLabel);
        (await _testee.VerifyHmacSha256(plainText, hash, keyLabel)).Should().BeTrue();

        await _testee.DeleteMacSecretKey(keyLabel);
    }

    [Fact]
    public async Task AesRoundtripShouldWork()
    {
        var aesSecretKeyLabel = "aes-secret-key";
        await _testee.GenerateAesSecretKey(aesSecretKeyLabel);

        var plainText = new byte[] { 121, 12, 67, 55, 14, 37, 12, 15 };

        var cipherText = await _testee.EncryptAesGcm(plainText, aesSecretKeyLabel);
        var decryptedCipherText = await _testee.DecryptAesGcm(cipherText, aesSecretKeyLabel);

        plainText.SequenceEqual(decryptedCipherText).Should().BeTrue();

        await _testee.DeleteAesSecretKey(aesSecretKeyLabel);
    }

    [Fact]
    public async Task SignVerifyRoundtripShouldWork()
    {
        var privateKey = "my_PRIVATE_key";
        var publicKey = "my_PUBLIC_key";

        var content = new byte[] { 17, 12, 21, 51, 53, 5, 9 };
        var signature = await _testee.CreateSignature(content, privateKey);
        var ok = await _testee.VerifySignature(content, signature, publicKey);
        ok.Should().BeTrue();
    }
}
