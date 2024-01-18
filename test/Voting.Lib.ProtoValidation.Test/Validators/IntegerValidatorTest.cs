// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class IntegerValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new IntegerValidator();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(-10, -10)]
    [InlineData(-10, -9)]
    [InlineData(-10, 9)]
    public void ValidMinValueShouldWork(int minValue, int value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = minValue }), value);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(2, 1)]
    [InlineData(-10, -11)]
    public void InvalidMinValueShouldFail(int minValue, int value)
    {
        var failure = Validate(BuildRules(new() { MinValue = minValue }), value).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is smaller than the MinValue {minValue}");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, -3)]
    [InlineData(-10, -10)]
    [InlineData(-10, -11)]
    public void ValidMaxValueShouldWork(int maxValue, int value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MaxValue = maxValue }), value);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(-10, -9)]
    [InlineData(-10, 0)]
    public void InvalidMaxValueShouldFail(int maxValue, int value)
    {
        var failure = Validate(BuildRules(new() { MaxValue = maxValue }), value).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is greater than the MaxValue {maxValue}");
    }

    [Fact]
    public void NullAndAnyOtherRuleShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = 1, MaxValue = 100 }), null);
    }

    private Rules BuildRules(IntegerRules rules) => new() { Integer = rules };
}
