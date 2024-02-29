// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc brick.
/// </summary>
public class Brick
{
    /// <summary>
    /// Gets or sets the brick ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the brick.
    /// </summary>
    [JsonPropertyName("name_value")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the brick.
    /// </summary>
    [JsonPropertyName("description_value")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the internal name of the brick.
    /// </summary>
    public string InternName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the preview of the brick (in HTML format).
    /// </summary>
    public string PreviewData { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets brick contents.
    /// </summary>
    public List<BrickContent> BrickContents { get; set; } = new();
}
