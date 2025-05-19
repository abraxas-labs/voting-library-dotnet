// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for complex single line text.
/// </summary>
public class ComplexSlTextAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComplexSlTextAttribute"/> class.
    /// </summary>
    public ComplexSlTextAttribute()
        : base(StringValidation.ComplexSlTextPattern)
    {
    }
}
