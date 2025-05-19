// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for untrimmed text.
/// </summary>
public class UntrimmedAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UntrimmedAttribute"/> class.
    /// </summary>
    public UntrimmedAttribute()
        : base(StringValidation.UntrimmedPattern)
    {
    }
}
