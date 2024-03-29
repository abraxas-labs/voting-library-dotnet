// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

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
}
