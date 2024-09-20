// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class UuidV5Test
{
    [Theory]
    [InlineData("random sample", "49d9df71-735f-5cfe-addc-5091bcd4104f", "6cd6f4fb-798a-4426-aa5d-991d36424909", "test")]
    [InlineData("random sample2", "b4051f71-e468-556f-8ddb-95513b85caf2", "4f395963-8b36-4fcb-b65d-470dbd420776", "random input")]
    [InlineData("www.example.org dns sample from wiki", "74738ff5-5367-5958-9aee-98fffdcd1876", "6ba7b810-9dad-11d1-80b4-00c04fd430c8", "www.example.org")]
    public void Test(string testName, string expected, string @namespace, string name)
    {
        UuidV5.Create(Guid.Parse(@namespace), name)
            .ToString()
            .Should()
            .Be(expected, testName);
    }
}
