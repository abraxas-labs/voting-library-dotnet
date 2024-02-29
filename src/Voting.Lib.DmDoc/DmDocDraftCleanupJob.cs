// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.DmDoc.Models;
using Voting.Lib.Scheduler;

namespace Voting.Lib.DmDoc;

/// <summary>
/// A scheduled job that runs in the background which cleans up DmDoc drafts that are listed for deletion.
/// </summary>
public class DmDocDraftCleanupJob : IScheduledJob
{
    private readonly IDmDocDraftCleanupQueue _dmDocDraftCleanupQueue;
    private readonly IDmDocService _dmDoc;
    private readonly ILogger<DmDocDraftCleanupJob> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocDraftCleanupJob"/> class.
    /// </summary>
    /// <param name="dmDocDraftCleanupQueue">The DmDoc draft cleanup queue.</param>
    /// <param name="dmDoc">The DmDoc service to request draft cleanups.</param>
    /// <param name="logger">The logger.</param>
    public DmDocDraftCleanupJob(
        IDmDocDraftCleanupQueue dmDocDraftCleanupQueue,
        IDmDocService dmDoc,
        ILogger<DmDocDraftCleanupJob> logger)
    {
        _dmDocDraftCleanupQueue = dmDocDraftCleanupQueue;
        _dmDoc = dmDoc;
        _logger = logger;
    }

    /// <summary>
    /// Runs this cleanup job. Unhandled exceptions will be caught and logged.
    /// Between dequeuing cleanup items and requesting cleanups towards DmDoc the thread is suspended for 5s
    /// to avoid just queued items to be cleaned up immediately.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Run(CancellationToken ct)
    {
        var draftCleanupList = DequeueDraftCleanupItems(ct);

        await Task.Delay(5000, ct).ConfigureAwait(false);

        foreach (var draftCleanupItem in draftCleanupList)
        {
            await TryCleanUpDraft(draftCleanupItem, ct).ConfigureAwait(false);
        }
    }

    private List<DraftCleanupItem> DequeueDraftCleanupItems(CancellationToken ct)
    {
        var draftCleanupList = new List<DraftCleanupItem>();

        while (_dmDocDraftCleanupQueue.TryDequeue(out var draftCleanupItem))
        {
            if (ct.IsCancellationRequested)
            {
                return draftCleanupList;
            }

            if (draftCleanupItem == null)
            {
                continue;
            }

            draftCleanupList.Add(draftCleanupItem);
        }

        return draftCleanupList;
    }

    private async Task TryCleanUpDraft(DraftCleanupItem draftCleanupItem, CancellationToken ct)
    {
        try
        {
            _logger.LogDebug(
                "Cleaning up an outdated draft with {DraftId} and {CleanupMode}.",
                draftCleanupItem.Id,
                draftCleanupItem.CleanupMode);

            switch (draftCleanupItem.CleanupMode)
            {
                case DraftCleanupMode.Soft:
                    await _dmDoc.DeleteDraft(draftCleanupItem.Id, ct).ConfigureAwait(false);
                    break;
                case DraftCleanupMode.Hard:
                    await _dmDoc.DeleteDraftHard(draftCleanupItem.Id, ct).ConfigureAwait(false);
                    break;
                case DraftCleanupMode.Content:
                    await _dmDoc.DeleteDraftContent(draftCleanupItem.Id, ct).ConfigureAwait(false);
                    break;
                default:
                    _logger.LogError("Unsupported draft cleanup mode for {DraftId}.", draftCleanupItem.Id);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while cleaning up an outdated draft with {DraftId}.", draftCleanupItem.Id);
        }
    }
}
