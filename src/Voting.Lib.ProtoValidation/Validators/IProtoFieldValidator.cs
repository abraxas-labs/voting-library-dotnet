// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// Interface for a proto field validator.
/// </summary>
public interface IProtoFieldValidator
{
    /// <summary>
    /// Validates the data.
    /// </summary>
    /// <param name="context">The proto validation context.</param>
    /// <param name="rules">The validation rules.</param>
    /// <param name="value">The field value.</param>
    /// <param name="fieldName">The field name.</param>
    void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName);
}
