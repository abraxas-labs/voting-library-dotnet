// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// A long proto validator.
/// </summary>
public class LongValidator : IProtoFieldValidator
{
    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var integerRules = rules.Integer64;
        if (integerRules == null)
        {
            return;
        }

        var integerValue = value as long?;

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

    private void ValidateMinValue(ProtoValidationContext context, long value, long minValue, string fieldName)
    {
        if (value < minValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is smaller than the MinValue {minValue}"));
        }
    }

    private void ValidateMaxValue(ProtoValidationContext context, long value, long maxValue, string fieldName)
    {
        if (value > maxValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is greater than the MaxValue {maxValue}"));
        }
    }
}
