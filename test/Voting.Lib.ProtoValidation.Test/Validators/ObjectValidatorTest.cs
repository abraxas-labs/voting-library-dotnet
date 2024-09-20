// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class ObjectValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new ObjectValidator();

    [Fact]
    public void NullWithRequiredShouldFail()
    {
        var failure = Validate(BuildRules(new() { Required = true }), null).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is required.");
    }

    [Fact]
    public void NullWithoutRequiredShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new()), null);
    }

    [Fact]
    public void ObjectWithRequiredShouldWork()
    {
        ShouldHaveNoFailures(BuildRules(new()), new object());
    }

    private Rules BuildRules(ObjectRules rules) => new() { Object = rules };
}
