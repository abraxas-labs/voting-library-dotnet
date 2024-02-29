// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.DmDoc.Models;

namespace Voting.Lib.DmDoc;

/// <summary>
/// Interface for the DmDoc draft cleanup queue.
/// </summary>
public interface IDmDocDraftCleanupQueue
{
    /// <summary>
    /// Enqueue a draft id to the cleanup queue to schedule it for deletion.
    /// </summary>
    /// <param name="draftId">The DmDoc draft id.</param>
    /// <param name="draftCleanupMode">The draft cleanup mode.</param>
    public void Enqueue(int draftId, DraftCleanupMode draftCleanupMode);

    /// <summary>
    /// Tries to dequeue a DmDoc draft id from the queue.
    /// </summary>
    /// <param name="draftCleanupItem">The draft cleanup item.</param>
    /// <returns>true if an element was removed form the queue and returned successfully; otherwise, false.</returns>
    public bool TryDequeue(out DraftCleanupItem? draftCleanupItem);
}
