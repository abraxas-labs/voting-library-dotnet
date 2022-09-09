// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// An integer proto validator.
/// </summary>
public class IntegerValidator : IProtoFieldValidator
{
    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var integerRules = rules.Integer;
        if (integerRules == null)
        {
            return;
        }

        var integerValue = value as int?;

        if (value != null && integerValue == null)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not an Integer."));
            return;
        }

        if (integerValue == null)
        {
            return;
        }

        if (integerRules.HasMinValue)
        {
            ValidateMinValue(context, integerValue.Value, integerRules.MinValue, fieldName);
        }

        if (integerRules.HasMaxValue)
        {
            ValidateMaxValue(context, integerValue.Value, integerRules.MaxValue, fieldName);
        }
    }

    private void ValidateMinValue(ProtoValidationContext context, int value, int minValue, string fieldName)
    {
        if (value < minValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is smaller than the MinValue {minValue}"));
        }
    }

    private void ValidateMaxValue(ProtoValidationContext context, int value, int maxValue, string fieldName)
    {
        if (value > maxValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is smaller than the MaxValue {maxValue}"));
        }
    }
}
