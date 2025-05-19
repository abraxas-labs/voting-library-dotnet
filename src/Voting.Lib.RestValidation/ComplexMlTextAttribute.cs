// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for complex multiline text.
/// </summary>
public class ComplexMlTextAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComplexMlTextAttribute"/> class.
    /// </summary>
    public ComplexMlTextAttribute()
        : base(StringValidation.ComplexMlTextPattern)
    {
    }
}
