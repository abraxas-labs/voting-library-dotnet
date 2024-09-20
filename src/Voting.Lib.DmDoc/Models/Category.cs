// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc category.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets or sets the category text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the internal name.
    /// </summary>
    public string InternName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the access.
    /// </summary>
    public bool? Access { get; set; }

    /// <summary>
    /// Gets or sets the list of child categories.
    /// </summary>
    public List<Category>? Children { get; set; }
}
