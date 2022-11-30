// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc template.
/// </summary>
public class Template
{
    /// <summary>
    /// Gets or sets the template ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the template name.
    /// </summary>
    [JsonPropertyName("name_value")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the internal name of the template.
    /// </summary>
    public string InternName { get; set; } = string.Empty;
}
