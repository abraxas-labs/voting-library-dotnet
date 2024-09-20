// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class AgeUtilTest
{
    [Theory]
    [InlineData("2000-04-13", "2018-11-11", 18)]
    [InlineData("2000-04-13", "2018-02-11", 17)]
    [InlineData("2000-02-29", "2005-02-28", 4)]
    [InlineData("2000-02-29", "2005-03-01", 5)]
    [InlineData("2000-02-29", "2004-02-29", 4)]
    [InlineData("2000-02-29", "2004-02-28", 3)]
    [InlineData("2000-01-01", "2008-01-01", 8)]
    [InlineData("2000-01-01", "2007-12-31", 7)]
    [InlineData("1992-06-28", "1992-06-28", 0)]
    [InlineData("1992-06-28", "1992-10-02", 0)]
    public void ShouldCalculateAge(string dateOfBirth, string today, int expectedAge)
    {
        var age = AgeUtil.CalculateAge(DateOnly.Parse(dateOfBirth), DateOnly.Parse(today));
        age.Should().Be(expectedAge);
    }

    [Fact]
    public void CalculateAgeShouldThrowOnEarlierReferenceDate()
    {
        var action = () => AgeUtil.CalculateAge(DateOnly.Parse("1950-08-23"), DateOnly.Parse("1920-02-12"));
        action.Should().Throw<ArgumentException>();
    }
}
