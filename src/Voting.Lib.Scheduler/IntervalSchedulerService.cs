// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Voting.Lib.Scheduler;

/// <summary>
/// A background service that schedules jobs in intervals.
/// </summary>
/// <typeparam name="TJob">The type of job to run.</typeparam>
public sealed class IntervalSchedulerService<TJob> : BackgroundService
    where TJob : IScheduledJob
{
    private readonly JobRunner _jobRunner;
    private readonly bool _runOnStart;
    private readonly TimeSpan _jobInterval;
    private PeriodicTimer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntervalSchedulerService{TJob}"/> class.
    /// </summary>
    /// <param name="jobRunner">The job runner.</param>
    /// <param name="jobConfig">The job configuration.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the job config is invalid.</exception>
    public IntervalSchedulerService(JobRunner jobRunner, IJobConfig jobConfig)
    {
        if (jobConfig.Interval <= TimeSpan.Zero)
        {
            throw new ValidationException($"{nameof(jobConfig.Interval)} should be greater than 0");
        }

        _jobRunner = jobRunner;
        _jobInterval = jobConfig.Interval;
        _runOnStart = jobConfig.RunOnStart;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new PeriodicTimer(_jobInterval);

        if (_runOnStart)
        {
            await _jobRunner.RunJob<TJob>(stoppingToken).ConfigureAwait(false);
        }

        while (await _timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            await _jobRunner.RunJob<TJob>(stoppingToken).ConfigureAwait(false);
        }
    }
}
