// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// A double proto validator.
/// </summary>
public class DoubleValidator : IProtoFieldValidator
{
    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var doubleRules = rules.Double;
        if (doubleRules == null)
        {
            return;
        }

        var doubleValue = value as double?;

        if (value != null && doubleValue == null)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a Double."));
            return;
        }

        if (doubleValue == null)
        {
            return;
        }

        if (doubleRules.HasMinValue)
        {
            ValidateMinValue(context, doubleValue.Value, doubleRules.MinValue, fieldName);
        }

        if (doubleRules.HasMaxValue)
        {
            ValidateMaxValue(context, doubleValue.Value, doubleRules.MaxValue, fieldName);
        }
    }

    private void ValidateMinValue(ProtoValidationContext context, double value, double minValue, string fieldName)
    {
        if (value < minValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is smaller than the MinValue {minValue}"));
        }
    }

    private void ValidateMaxValue(ProtoValidationContext context, double value, double maxValue, string fieldName)
    {
        if (value > maxValue)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"is smaller than the MaxValue {maxValue}"));
        }
    }
}
