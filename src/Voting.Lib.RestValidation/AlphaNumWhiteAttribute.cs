// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for alpha numeric text with whitespace.
/// </summary>
public class AlphaNumWhiteAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaNumWhiteAttribute"/> class.
    /// </summary>
    public AlphaNumWhiteAttribute()
        : base(StringValidation.AlphaNumWhitePattern)
    {
    }
}
