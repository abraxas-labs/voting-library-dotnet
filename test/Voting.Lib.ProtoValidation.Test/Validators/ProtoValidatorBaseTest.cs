// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public abstract class ProtoValidatorBaseTest
{
    public abstract IProtoFieldValidator Validator { get; }

    protected string FieldName { get; } = "TestFieldName";

    protected void ShouldHaveNoFailures(Rules rules, object? value)
    {
        Validate(rules, value).Any().Should().BeFalse();
    }

    protected IReadOnlyCollection<ProtoValidationError> Validate(Rules rules, object? value)
    {
        var context = new ProtoValidationContext();
        Validator!.Validate(context, rules, value, FieldName);
        return context.Failures;
    }
}
