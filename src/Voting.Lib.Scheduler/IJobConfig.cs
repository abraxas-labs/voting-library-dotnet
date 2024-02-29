// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Scheduler;

/// <summary>
/// Configuration for a job.
/// </summary>
public interface IJobConfig
{
    /// <summary>
    /// Gets the interval in which the job should be run.
    /// </summary>
    TimeSpan Interval { get; }

    /// <summary>
    /// Gets a value indicating whether this job should run immediately on startup.
    /// If this is set to false, the first job execution is performed only after the <see cref="Interval"/> has elapsed.
    /// </summary>
    bool RunOnStart => false;
}
