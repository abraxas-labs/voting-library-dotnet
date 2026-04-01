// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Converters;

/// <summary>
/// Converts <see cref="MarkdownString"/> to and from string for database storage.
/// </summary>
public class MarkdownStringConverter : ValueConverter<MarkdownString, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownStringConverter"/> class.
    /// </summary>
    public MarkdownStringConverter()
        : base(
            v => v.Markdown,
            v => new MarkdownString(v))
    {
    }
}
