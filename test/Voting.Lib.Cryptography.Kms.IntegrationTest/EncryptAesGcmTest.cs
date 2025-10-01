// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text;
using FluentAssertions;
using Voting.Lib.Cryptography.Kms.Exceptions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class EncryptAesGcmTest : BaseKmsIntegrationTest
{
    [Fact]
    public async Task DecryptAesGcmShouldWork()
    {
        var keyId = await GetOrCreateAesKey(nameof(DecryptAesGcmShouldWork));

        var ciphertext = Convert.FromBase64String("RWBMYPL7u40orGVrB3EE5B1rQxeW5dSDz0bBVP+VFXsTQ9163LCl+NBKJO45DggzmCIVNRg7TV5ixmruEhKlLmaRFdXilaI=");
        var plaintextDecrypted = await CryptoProvider.DecryptAesGcm(ciphertext, keyId);
        var plaintext2 = Encoding.UTF8.GetString(plaintextDecrypted);
        plaintext2.Should().Be("The quick brown fox jumps over the lazy dog");
    }

    [Fact]
    public async Task RoundtripAesGcmShouldWork()
    {
        var keyId = await CreateAesKey(nameof(RoundtripAesGcmShouldWork));

        var plaintext = "The quick brown fox jumps over the lazy dog"u8.ToArray();
        var ciphertext = await CryptoProvider.EncryptAesGcm(plaintext, keyId);
        var plaintextDecrypted = await CryptoProvider.DecryptAesGcm(ciphertext, keyId);
        plaintextDecrypted.Should().BeEquivalentTo(plaintext, x => x.WithStrictOrdering());

        // decrypt with other key should fail
        var keyId2 = await CreateAesKey(nameof(RoundtripAesGcmShouldWork));
        await Assert.ThrowsAsync<KmsException>(async () => await CryptoProvider.DecryptAesGcm(ciphertext, keyId2));
    }
}
