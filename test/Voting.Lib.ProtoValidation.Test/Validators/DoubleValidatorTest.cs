// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class DoubleValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new DoubleValidator();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 0.1)]
    [InlineData(0.1, 0.1)]
    [InlineData(0.1, 0.2)]
    [InlineData(-0.1, -0.1)]
    [InlineData(-0.1, -0.09)]
    [InlineData(-0.1, 0.09)]
    public void ValidMinValueShouldWork(double minValue, double value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = minValue }), value);
    }

    [Theory]
    [InlineData(0, -0.1)]
    [InlineData(0.1, 0)]
    [InlineData(0.2, 0)]
    [InlineData(0.2, 0.1)]
    [InlineData(-0.1, -0.11)]
    public void InvalidMinValueShouldFail(double minValue, double value)
    {
        var failure = Validate(BuildRules(new() { MinValue = minValue }), value).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is smaller than the MinValue {minValue}");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -0.1)]
    [InlineData(0.2, 0.1)]
    [InlineData(0.2, 0.2)]
    [InlineData(0.2, -0.3)]
    [InlineData(-0.1, -0.1)]
    [InlineData(-0.1, -0.11)]
    public void ValidMaxValueShouldWork(double maxValue, double value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MaxValue = maxValue }), value);
    }

    [Theory]
    [InlineData(0, 0.1)]
    [InlineData(0.1, 0.2)]
    [InlineData(-0.1, -0.09)]
    [InlineData(-0.1, 0)]
    public void InvalidMaxValueShouldFail(double maxValue, double value)
    {
        var failure = Validate(BuildRules(new() { MaxValue = maxValue }), value).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is smaller than the MaxValue {maxValue}");
    }

    [Fact]
    public void NullAndAnyOtherRuleShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = 1, MaxValue = 100 }), null);
    }

    private Rules BuildRules(DoubleRules rules) => new() { Double = rules };
}
