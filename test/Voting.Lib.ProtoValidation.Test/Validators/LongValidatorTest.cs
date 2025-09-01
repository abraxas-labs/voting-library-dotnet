// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class LongValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new LongValidator();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(-10, -10)]
    [InlineData(-10, -9)]
    [InlineData(-10, 9)]
    public void ValidMinValueShouldWork(long minValue, long value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = minValue }), value);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(2, 1)]
    [InlineData(-10, -11)]
    public void InvalidMinValueShouldFail(long minValue, long value)
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
    public void ValidMaxValueShouldWork(long maxValue, long value)
    {
        ShouldHaveNoFailures(BuildRules(new() { MaxValue = maxValue }), value);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(-10, -9)]
    [InlineData(-10, 0)]
    public void InvalidMaxValueShouldFail(long maxValue, long value)
    {
        var failure = Validate(BuildRules(new() { MaxValue = maxValue }), value).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is greater than the MaxValue {maxValue}");
    }

    [Fact]
    public void NullAndAnyOtherRuleShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new() { MinValue = 1, MaxValue = 100 }), null);
    }

    private Rules BuildRules(Integer64Rules rules) => new() { Integer64 = rules };
}
