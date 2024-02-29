// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Test.Messages;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class EnumValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new EnumValidator();

    [Fact]
    public void UnspecifiedWithAllowUnspecifiedShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new() { AllowUnspecified = true }), State.Unspecified);
    }

    [Fact]
    public void UnspecifiedWithoutAllowUnspecifiedShouldFail()
    {
        var failure = Validate(BuildRules(new() { ExactEnum = true }), State.Unspecified).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is Unspecified.");
    }

    [Theory]
    [InlineData((State)(-1))]
    [InlineData((State)2)]
    public void NotInEnumWithExactEnumShouldFail(State state)
    {
        var failure = Validate(BuildRules(new() { ExactEnum = true }), state).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not defined in the Enum.");
    }

    [Fact]
    public void ValidExactEnumShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new() { ExactEnum = true }), State.Active);
    }

    private Rules BuildRules(EnumRules rules) => new() { Enum = rules };
}
