// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Voting.Lib.Scheduler.Diagnostics;

namespace Voting.Lib.Scheduler;

/// <summary>
/// A background service that schedules jobs in intervals.
/// </summary>
/// <typeparam name="TJob">The type of job to run.</typeparam>
public sealed class SchedulerService<TJob> : BackgroundService
    where TJob : IScheduledJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SchedulerService<TJob>> _logger;
    private readonly string _jobName;
    private readonly bool _runOnStart;
    private readonly PeriodicTimer _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulerService{TJob}"/> class.
    /// </summary>
    /// <param name="scopeFactory">The service scope factory.</param>
    /// <param name="jobConfig">The job configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the job config is invalid.</exception>
    public SchedulerService(IServiceScopeFactory scopeFactory, IJobConfig jobConfig, ILogger<SchedulerService<TJob>> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        if (jobConfig.Interval <= TimeSpan.Zero)
        {
            throw new ValidationException($"{nameof(jobConfig.Interval)} should be greater than 0");
        }

        _jobName = typeof(TJob).Name;
        _timer = new PeriodicTimer(jobConfig.Interval);
        _runOnStart = jobConfig.RunOnStart;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _timer.Dispose();
        base.Dispose();
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_runOnStart)
        {
            await RunJob(stoppingToken).ConfigureAwait(false);
        }

        while (await _timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            await RunJob(stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task RunJob(CancellationToken cancellationToken)
    {
        using var jobRun = SchedulerMeter.JobRunning();
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["JobName"] = _jobName,
        });

        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var jobInstance = scope.ServiceProvider.GetRequiredService<TJob>();
            _logger.LogDebug("Start running job...");
            await jobInstance.Run(cancellationToken).ConfigureAwait(false);
            _logger.LogDebug("Finished running job");
            SchedulerMeter.JobRun(_jobName, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job execution failed");
            SchedulerMeter.JobRun(_jobName, false);
        }
    }
}
