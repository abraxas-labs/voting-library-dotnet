// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for numeric text.
/// </summary>
public class NumericAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumericAttribute"/> class.
    /// </summary>
    public NumericAttribute()
        : base(StringValidation.NumericPattern)
    {
    }
}
