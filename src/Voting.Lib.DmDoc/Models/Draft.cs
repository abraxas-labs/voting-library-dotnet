// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json.Serialization;

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// A DmDoc draft.
/// </summary>
public class Draft
{
    /// <summary>
    /// Gets or sets the draft ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the draft state.
    /// </summary>
    [JsonPropertyName("istatus")]
    public DraftState State { get; set; }
}
