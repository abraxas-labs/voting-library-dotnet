// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// A generic object proto validator.
/// </summary>
public class ObjectValidator : IProtoFieldValidator
{
    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var objectRules = rules.Object;
        if (objectRules == null)
        {
            return;
        }

        if (objectRules.Required && value == null)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is required."));
        }
    }
}
