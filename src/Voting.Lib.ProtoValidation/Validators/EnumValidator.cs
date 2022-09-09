// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// An enum proto validator.
/// </summary>
public class EnumValidator : IProtoFieldValidator
{
    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var enumRules = rules.Enum;
        if (enumRules == null)
        {
            return;
        }

        var enumType = value?.GetType();

        if (value == null || enumType?.IsEnum != true)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is supposed to be an Enum and Proto Enums cannot be null"));
            return;
        }

        if (!enumRules.AllowUnspecified)
        {
            ValidateNotUnspecified(context, value, fieldName);
        }

        if (enumRules.ExactEnum)
        {
            ValidateExactEnum(context, enumType, value, fieldName);
        }
    }

    private void ValidateNotUnspecified(ProtoValidationContext context, object enumValue, string fieldName)
    {
        // Unspecified has the enum value 0 per protobuf guideline https://developers.google.com/protocol-buffers/docs/style#enums.
        if ((int)enumValue == 0)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is Unspecified."));
        }
    }

    private void ValidateExactEnum(ProtoValidationContext context, Type enumType, object enumValue, string fieldName)
    {
        if (!Enum.IsDefined(enumType, enumValue))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not defined in the Enum."));
        }
    }
}
