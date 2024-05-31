// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class DecimalExtensionsTest
{
    [Theory]
    [InlineData(0.21, 0.2109, 2, true)]
    [InlineData(0.21, 0.2109, 3, true)]
    [InlineData(0.21, 0.211, 3, false)]
    public void ApproxEqualsShouldWork(decimal d1, decimal d2, int precision, bool expectedResult)
    {
        d1.ApproxEquals(d2, precision)
            .Should().Be(expectedResult);
    }

    [Fact]
    public void ApproxEqualsWithNonPositivePrecisionShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => 5.0M.ApproxEquals(2.0M, 0))
            .Message
            .Should()
            .Be("Precision must be a positive number");

        Assert.Throws<ArgumentException>(() => 5.0M.ApproxEquals(2.0M, -1))
            .Message
            .Should()
            .Be("Precision must be a positive number");
    }
}
