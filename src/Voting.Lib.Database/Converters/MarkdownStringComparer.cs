// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Converters;

/// <summary>
/// Compares <see cref="MarkdownString"/> instances for equality.
/// Needed for change tracking in EF Core.
/// </summary>
public class MarkdownStringComparer : ValueComparer<MarkdownString>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownStringComparer"/> class.
    /// </summary>
    public MarkdownStringComparer()
        : base(
            (c1, c2) => c1 != null ? c1.Equals(c2) : c2 == null,
            c => c.GetHashCode(),
            c => new MarkdownString(c.Markdown))
    {
    }
}
