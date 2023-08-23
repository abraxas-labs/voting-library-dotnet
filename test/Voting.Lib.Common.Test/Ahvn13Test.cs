// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class Ahvn13Test
{
    [Theory]
    [InlineData("756.1234.5678.97", true, 7561234567897)]
    [InlineData("756.1234.5678.80", true, 7561234567880)]
    [InlineData("756.1234.5678.98", false, null)]
    [InlineData("test", false, null)]
    [InlineData("856.1234.5678.98", false, null)]
    [InlineData("", false, null)]
    [InlineData(null, false, null)]
    public void StringTryParseShouldWork(string input, bool expectedResult, long? numberRepresentation)
    {
        var result = Ahvn13.TryParse(input, out var parsed);
        result.Should().Be(expectedResult);

        if (result)
        {
            parsed!.ToString().Should().Be(input);
            parsed.ToNumber().Should().Be(numberRepresentation);
        }
    }

    [Theory]
    [InlineData("756.1234.5678.97", 7561234567897)]
    [InlineData("756.1234.5678.80", 7561234567880)]
    public void StringParseShouldWork(string input, long expectedResult)
    {
        var result = Ahvn13.Parse(input);
        result.ToNumber().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("756.1234.5678.98")]
    [InlineData("test")]
    [InlineData("856.1234.5678.98")]
    [InlineData("")]
    public void StringParseShouldThrowOnBadInput(string input)
    {
        var parseAction = () => Ahvn13.Parse(input);
        parseAction.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData(7561234567897, true, "756.1234.5678.97")]
    [InlineData(7561234567898, false, null)]
    [InlineData(0, false, null)]
    [InlineData(1234, false, null)]
    [InlineData(-7561234567897, false, null)]
    public void NumberTryParseShouldWork(long input, bool expectedResult, string? stringRepresentation)
    {
        var result = Ahvn13.TryParse(input, out var parsed);
        result.Should().Be(expectedResult);

        if (result)
        {
            parsed!.ToNumber().Should().Be(input);
            parsed.ToString().Should().Be(stringRepresentation);
        }
    }

    [Theory]
    [InlineData(7561234567897, "756.1234.5678.97")]
    public void NumberParseShouldWork(long input, string expectedResult)
    {
        var result = Ahvn13.Parse(input);
        result.ToString().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(7561234567898)]
    [InlineData(0)]
    [InlineData(1234)]
    [InlineData(-7561234567897)]
    public void NumberParseShouldThrowOnInvalidInput(long input)
    {
        var parseAction = () => Ahvn13.Parse(input);
        parseAction.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData("756.1234.5678.97", true)]
    [InlineData("756.1234.5678.80", true)]
    [InlineData("756.1234.5678.98", false)]
    [InlineData("test", false)]
    [InlineData("856.1234.5678.98", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void StringIsValidShouldWork(string? input, bool expected)
    {
        var isValid = Ahvn13.IsValid(input);
        isValid.Should().Be(expected);
    }

    [Theory]
    [InlineData(7561234567897, true)]
    [InlineData(7561234567880, true)]
    [InlineData(7561234567898, false)]
    [InlineData(75611234567898, false)]
    [InlineData(8561234567898, false)]
    [InlineData(-7561234567897, false)]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    public void NumberIsValidShouldWork(long input, bool expected)
    {
        var isValid = Ahvn13.IsValid(input);
        isValid.Should().Be(expected);
    }
}
