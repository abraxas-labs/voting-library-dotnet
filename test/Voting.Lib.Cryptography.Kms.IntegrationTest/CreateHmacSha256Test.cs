// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class CreateHmacSha256Test : BaseKmsIntegrationTest
{
    [Fact]
    public async Task CreateHmacSha256WithExistingKeyShouldWork()
    {
        var keyId = await GetOrCreateMacKey(nameof(CreateHmacSha256WithExistingKeyShouldWork));
        var data = "foo bar baz"u8.ToArray();
        var hmac = await CryptoProvider.CreateHmacSha256(data, keyId);
        var hmacB64 = Convert.ToBase64String(hmac);
        hmacB64.Should().Be("b4460ed5a5b56f8756d508d22bba76eb3a2032fad55b88ede5871d2d92ea5cec");
    }

    [Fact]
    public async Task CreateHmacSha256ShouldWork()
    {
        var keyId = await CreateMacKey(nameof(CreateHmacSha256ShouldWork));
        var data = "foo bar baz"u8.ToArray();
        var hmac = await CryptoProvider.CreateHmacSha256(data, keyId);
        var hmac2 = await CryptoProvider.CreateHmacSha256(data, keyId);
        hmac.Should().BeEquivalentTo(hmac2, x => x.WithStrictOrdering());

        var key2Id = await CreateMacKey(nameof(CreateHmacSha256ShouldWork));
        var hmac3 = await CryptoProvider.CreateHmacSha256(data, key2Id);
        hmac.Should().NotBeEquivalentTo(hmac3, x => x.WithStrictOrdering());
    }
}
