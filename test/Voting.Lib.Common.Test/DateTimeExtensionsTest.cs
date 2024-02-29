// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class DateTimeExtensionsTest
{
    [Fact]
    public void TestNextUtcDate()
    {
        var dataSet = new List<(DateTime Input, DateTime ExpectedResult)>
        {
            (new DateTime(2021, 12, 31, 1, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2021, 12, 31, 22, 59, 59, DateTimeKind.Utc), new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2021, 12, 31, 23, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 2, 0, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 6, 1, 21, 59, 59, DateTimeKind.Utc), new DateTime(2022, 6, 2, 0, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 6, 1, 22, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 2, 0, 0, 0, DateTimeKind.Utc)),
        };

        foreach (var (input, expectedResult) in dataSet)
        {
            var result = input.NextUtcDate();
            result.Should().Be(expectedResult);
        }
    }

    [Fact]
    public void TestNextUtcDateWithChTimeZone()
    {
        var dataSet = new List<(DateTime Input, DateTime ExpectedResult)>
        {
            (new DateTime(2021, 12, 31, 1, 0, 0, DateTimeKind.Utc), new DateTime(2021, 12, 31, 23, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2021, 12, 31, 22, 59, 59, DateTimeKind.Utc), new DateTime(2021, 12, 31, 23, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2021, 12, 31, 23, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 1, 23, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 1, 1, 23, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 6, 1, 21, 59, 59, DateTimeKind.Utc), new DateTime(2022, 6, 1, 22, 0, 0, DateTimeKind.Utc)),
            (new DateTime(2022, 6, 1, 22, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 2, 22, 0, 0, DateTimeKind.Utc)),
        };

        foreach (var (input, expectedResult) in dataSet)
        {
            var result = input.NextUtcDate(true);
            result.Should().Be(expectedResult);
        }
    }

    [Theory]
    [InlineData(DateTimeKind.Unspecified)]
    [InlineData(DateTimeKind.Local)]
    public void TestNextUtcDateWithNonUtcDateTimeKindShouldThrow(DateTimeKind dateTimeKind)
    {
        var action = () => new DateTime(2021, 12, 31, 0, 0, 0, dateTimeKind).NextUtcDate();
        action.Should().Throw<ArgumentException>()
            .WithMessage("DateTime must be of kind Utc");
    }
}
