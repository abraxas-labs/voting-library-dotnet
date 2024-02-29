// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// Item for the draft cleanup queue.
/// </summary>
public class DraftCleanupItem
{
    /// <summary>
    /// Gets or sets the draft ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the draft cleanup mode.
    /// Default is <see cref="DraftCleanupMode.Hard"/>.
    /// </summary>
    public DraftCleanupMode CleanupMode { get; set; } = DraftCleanupMode.Hard;
}
