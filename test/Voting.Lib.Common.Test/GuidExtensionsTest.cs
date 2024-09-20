// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Voting.Lib.Common.Extensions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class GuidExtensionsTest
{
    [InlineData("00000000-0000-0000-0000-000000000000", new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 })]
    [InlineData("c7edaf16-c6ca-4837-84dd-bb0b2941e06e", new byte[] { 0xC7, 0xED, 0xAF, 0x16, 0xC6, 0xCA, 0x48, 0x37, 0x84, 0xDD, 0xBB, 0x0B, 0x29, 0x41, 0xE0, 0x6E })]
    [Theory]
    public void WriteBytesAsRfc4122ShouldSerializeAccordingToRfc4122(string guid, byte[] expected)
    {
        var parsed = Guid.Parse(guid);
        Span<byte> data = stackalloc byte[GuidExtensions.GuidByteLength];
        parsed.WriteBytesAsRfc4122(data);
        data.ToArray().Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }
}
