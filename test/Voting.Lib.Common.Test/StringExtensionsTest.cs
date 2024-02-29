// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("fooBar", 4, "foo…")]
    [InlineData("fooBar", 20, "fooBar")]
    [InlineData("fooBar", 1, "…")]
    public void TruncateShouldWork(string input, int length, string expected)
    {
        input.Truncate(length).Should().Be(expected);
    }

    [Fact]
    public void TruncateMaxLengthZeroShouldThrow()
        => Assert.Throws<ArgumentOutOfRangeException>(() => "fooBar".Truncate(0));
}
