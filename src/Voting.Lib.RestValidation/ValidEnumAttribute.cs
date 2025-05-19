// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validates an enum and check if it has a valid value.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ValidEnumAttribute : ValidationAttribute
{
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            // This should be handled by a Required attribute
            return true;
        }

        var type = value.GetType();
        return type.IsEnum && Enum.IsDefined(type, value);
    }
}
