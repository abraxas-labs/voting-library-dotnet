// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Cryptography.Kms.IntegrationTest;

public class BulkCreateHmacSha256Test : BaseKmsIntegrationTest
{
    [Fact]
    public async Task BulkCreateHmacSha256WithExistingKeyShouldWork()
    {
        var keyId = await GetOrCreateMacKey(nameof(BulkCreateHmacSha256WithExistingKeyShouldWork));
        var data = "foo bar baz"u8.ToArray();
        var hmac = await CryptoProvider.BulkCreateHmacSha256([data], keyId);
        var hmacB64 = Convert.ToBase64String(hmac[0]);
        hmacB64.Should().Be("996e8257f12e00adba191ef62772a2b816f04e16eee051573e15060aad943880");
    }

    [Fact]
    public async Task BulkCreateHmacSha256ShouldWork()
    {
        var keyId = await CreateMacKey(nameof(BulkCreateHmacSha256ShouldWork));
        var data = "foo bar baz"u8.ToArray();
        var data2 = "foo bar baz 2"u8.ToArray();
        var hmacs = await CryptoProvider.BulkCreateHmacSha256([data, data2], keyId);
        var hmacs2 = await CryptoProvider.BulkCreateHmacSha256([data, data2], keyId);
        hmacs.Should().BeEquivalentTo(hmacs2, x => x.WithStrictOrdering());
    }
}
