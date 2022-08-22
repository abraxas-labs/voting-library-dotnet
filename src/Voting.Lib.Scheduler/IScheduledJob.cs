// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Scheduler;

/// <summary>
/// A job that is scheduled to run in intervals.
/// </summary>
public interface IScheduledJob
{
    /// <summary>
    /// Run this job. Unhandled exceptions will be caught and logged.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Run(CancellationToken ct);
}
