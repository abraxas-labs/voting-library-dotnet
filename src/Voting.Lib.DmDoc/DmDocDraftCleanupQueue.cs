// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Concurrent;
using Voting.Lib.DmDoc.Models;

namespace Voting.Lib.DmDoc;

/// <inheritdoc />
public class DmDocDraftCleanupQueue : IDmDocDraftCleanupQueue
{
    private readonly ConcurrentQueue<DraftCleanupItem> _draftCleanupQueue = new();

    /// <inheritdoc />
    public void Enqueue(int draftId, DraftCleanupMode draftCleanupMode) => _draftCleanupQueue.Enqueue(new DraftCleanupItem
    {
        Id = draftId,
        CleanupMode = draftCleanupMode,
    });

    /// <inheritdoc />
    public bool TryDequeue(out DraftCleanupItem? draftCleanupItem) => _draftCleanupQueue.TryDequeue(out draftCleanupItem);
}
