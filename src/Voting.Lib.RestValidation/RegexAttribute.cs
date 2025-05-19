// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Voting.Lib.Validation;

namespace Voting.Lib.RestValidation;

/// <summary>
/// Base regex validation attribute.
/// </summary>
public abstract class RegexAttribute : RegularExpressionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegexAttribute"/> class.
    /// </summary>
    /// <param name="pattern">The regex pattern.</param>
    internal RegexAttribute(string pattern)
        : base(pattern)
    {
        MatchTimeoutInMilliseconds = StringValidation.ValidationTimeout.Milliseconds;
    }
}
