// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc data field.
/// </summary>
public class DataField
{
    /// <summary>
    /// Gets or sets the field name.
    /// </summary>
    [JsonPropertyName("field_name")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name value.
    /// </summary>
    [JsonPropertyName("name_value")]
    public string Name { get; set; } = string.Empty;
}
