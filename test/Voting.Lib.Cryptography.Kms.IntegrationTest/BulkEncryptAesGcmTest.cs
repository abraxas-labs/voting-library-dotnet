// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Voting.Lib.Cryptography.Kms.Exceptions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class BulkEncryptAesGcmTest : BaseKmsIntegrationTest
{
    [Fact]
    public async Task RoundtripAesGcmShouldWork()
    {
        var keyId = await CreateAesKey(nameof(RoundtripAesGcmShouldWork));
        var plaintexts = new List<byte[]>
        {
            "The quick brown fox jumps over the lazy dog"u8.ToArray(),
            "The quick brown fox jumps over the lazy dog 2"u8.ToArray(),
        };
        var ciphertexts = await CryptoProvider.BulkEncryptAesGcm(plaintexts, keyId);
        for (var i = 0; i < ciphertexts.Count; i++)
        {
            var plaintextDecrypted = await CryptoProvider.DecryptAesGcm(ciphertexts[i], keyId);
            plaintextDecrypted.Should().BeEquivalentTo(plaintexts[i], x => x.WithStrictOrdering());

            // decrypt with other key should fail
            var keyId2 = await CreateAesKey(nameof(RoundtripAesGcmShouldWork));
            await Assert.ThrowsAsync<KmsException>(async () => await CryptoProvider.DecryptAesGcm(ciphertexts[i], keyId2));
        }
    }
}
