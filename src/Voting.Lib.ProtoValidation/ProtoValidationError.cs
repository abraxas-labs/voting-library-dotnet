// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.ProtoValidation;

/// <summary>
/// A class which contains the details of a proto validation error.
/// </summary>
public class ProtoValidationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProtoValidationError"/> class.
    /// </summary>
    /// <param name="fieldName">Field name which contains the error.</param>
    /// <param name="fieldError">Error text.</param>
    public ProtoValidationError(string fieldName, string fieldError)
    {
        ErrorMessage = $"'{fieldName}' {fieldError}";
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string ErrorMessage { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ErrorMessage;
    }
}
