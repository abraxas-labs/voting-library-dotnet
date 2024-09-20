// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.Metrics;
using Voting.Lib.Common;

namespace Voting.Lib.Scheduler.Diagnostics;

/// <summary>
/// Meter for scheduling related operations.
/// </summary>
public static class SchedulerMeter
{
    /// <summary>
    /// Gets the name of the scheduler meter.
    /// </summary>
    public const string Name = VotingMeters.NamePrefix + "Scheduler";

    private static readonly Meter Meter = new(Name, VotingMeters.Version);

    private static readonly Counter<long> RunJobs = Meter.CreateCounter<long>(
        VotingMeters.InstrumentNamePrefix + "run_jobs",
        "Job",
        "Run jobs");

    private static int _runningJobs;

    static SchedulerMeter()
    {
        Meter.CreateObservableGauge(
            VotingMeters.InstrumentNamePrefix + "running_jobs",
            () => _runningJobs,
            "Job",
            "Currently running jobs");
    }

    internal static void JobRun(string jobType, bool succeeded)
        => RunJobs.Add(1, new("job", jobType), new("succeeded", succeeded));

    internal static IDisposable JobRunning()
    {
        _runningJobs++;
        return new Disposable(() => _runningJobs--);
    }
}
