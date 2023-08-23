// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class HashBuilderTest
{
    private enum TestEnum
    {
        V1,
    }

    [Fact]
    public void HashBuilderShouldGenerateConsistentHash()
    {
        using var hasher = new HashBuilder(HashAlgorithmName.SHA256);
        AppendAllDataKinds(hasher);
        var hash = hasher.GetHashAndReset();
        var expectedHash = "a92a0c99fccd94579cfca383ed9a61045a0d700bd0048b706aba7e9fcb4fc4bb";

        HashUtil
            .ToHexString(hash)
            .Should()
            .Be(expectedHash);

        AppendAllDataKinds(hasher);
        hash = hasher.GetHashAndReset();

        HashUtil
            .ToHexString(hash)
            .Should()
            .Be(expectedHash);
    }

    [Fact]
    public void HashBuilderShouldGenerateConsistentHashWithDelimiter()
    {
        using var hasher = new HashBuilder(HashAlgorithmName.SHA256);
        AppendAllDelimitedDataKinds(hasher);
        var hash = hasher.GetHashAndReset();
        var expectedHash = "4be0f28c6572d5923d9fb33581516b0e085a9f6103b244ddf8cb26e6259f3a03";

        HashUtil
            .ToHexString(hash)
            .Should()
            .Be(expectedHash);

        AppendAllDelimitedDataKinds(hasher);
        hash = hasher.GetHashAndReset();

        HashUtil
            .ToHexString(hash)
            .Should()
            .Be(expectedHash);
    }

    private void AppendAllDataKinds(HashBuilder hashBuilder)
    {
        hashBuilder
            .Append(new byte[] { 1, 2, 3 })
            .Append((byte[]?)null)
            .Append(Enumerable.Repeat(new byte[] { 1, 2, 3 }, 3))
            .Append((IEnumerable<byte[]>?)null)
            .Append("fooBar")
            .Append((string?)null)
            .Append(true)
            .Append(false)
            .Append((bool?)null)
            .Append(Guid.Parse("c102b334-4842-40b2-8973-a4df1c7227fc"))
            .Append((Guid?)null)
            .Append((byte)3)
            .Append((byte?)null)
            .Append(10)
            .Append((int?)null)
            .Append(20L)
            .Append((long?)null)
            .Append(new DateOnly(2010, 12, 3))
            .Append((DateOnly?)null)
            .Append(new DateTime(2010, 11, 12, 13, 14, 15, DateTimeKind.Utc))
            .Append((DateTime?)null)
            .Append(TestEnum.V1)
            .Append((TestEnum?)null);
    }

    private void AppendAllDelimitedDataKinds(HashBuilder hashBuilder)
    {
        hashBuilder
            .AppendDelimited(new byte[] { 1, 2, 3 })
            .AppendDelimited((byte[]?)null)
            .AppendDelimited(Enumerable.Repeat(new byte[] { 1, 2, 3 }, 3))
            .AppendDelimited((IEnumerable<byte[]>?)null)
            .AppendDelimited("fooBar")
            .AppendDelimited((string?)null)
            .AppendDelimited(true)
            .AppendDelimited(false)
            .AppendDelimited((bool?)null)
            .AppendDelimited(Guid.Parse("c102b334-4842-40b2-8973-a4df1c7227fc"))
            .AppendDelimited((Guid?)null)
            .AppendDelimited((byte)3)
            .AppendDelimited((byte?)null)
            .AppendDelimited(10)
            .AppendDelimited((int?)null)
            .AppendDelimited(20L)
            .AppendDelimited((long?)null)
            .AppendDelimited(new DateOnly(2010, 12, 3))
            .AppendDelimited((DateOnly?)null)
            .AppendDelimited(new DateTime(2010, 11, 12, 13, 14, 15, DateTimeKind.Utc))
            .AppendDelimited((DateTime?)null)
            .AppendDelimited(TestEnum.V1)
            .AppendDelimited((TestEnum?)null);
    }
}
