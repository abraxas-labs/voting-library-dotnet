// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Validation attribute for simple single line text.
/// </summary>
public class SimpleSlTextAttribute : RegexAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleSlTextAttribute"/> class.
    /// </summary>
    public SimpleSlTextAttribute()
        : base(StringValidation.SimpleSlTextPattern)
    {
    }
}
