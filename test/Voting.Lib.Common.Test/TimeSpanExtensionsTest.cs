// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class TimeSpanExtensionsTest
{
    [Theory]
    [InlineData(.5, "0s")]
    [InlineData(1, "1s")]
    [InlineData(59, "59s")]
    [InlineData(60, "1m")]
    [InlineData(60 * 59, "59m")]
    [InlineData(60 * 60, "1h")]
    [InlineData(60 * 60 * 10, "10h")]
    public void TestToHumanReadableString(double seconds, string humanReadable)
    {
        TimeSpan.FromSeconds(seconds)
            .ToHumanReadableString()
            .Should()
            .Be(humanReadable);
    }
}
