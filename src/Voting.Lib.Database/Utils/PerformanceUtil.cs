// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Voting.Lib.Database.Utils;

/// <summary>
/// Util for db performance.
/// </summary>
public static class PerformanceUtil
{
    /// <summary>
    /// Disables auto change detection, which improves performance noticeably when inserting many entities.
    /// </summary>
    /// <param name="context">The db context.</param>
    /// <returns>A disposable to re-enable auto change detection.</returns>
    public static IDisposable DisableAutoChangeDetection(DbContext context)
    {
        context.ChangeTracker.AutoDetectChangesEnabled = false;
        return new Disposable(() => context.ChangeTracker.AutoDetectChangesEnabled = true);
    }
}
