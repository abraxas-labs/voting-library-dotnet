// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Voting.Lib.Scheduler.Diagnostics;

namespace Voting.Lib.Scheduler;

/// <summary>
/// A service that handles running jobs.
/// </summary>
public sealed class JobRunner
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobRunner> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobRunner"/> class.
    /// </summary>
    /// <param name="scopeFactory">The service scope factory.</param>
    /// <param name="logger">The logger.</param>
    public JobRunner(IServiceScopeFactory scopeFactory, ILogger<JobRunner> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Runs the job once.
    /// </summary>
    /// <typeparam name="TJob">The type of job to run.</typeparam>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunJob<TJob>(CancellationToken cancellationToken)
        where TJob : IScheduledJob
    {
        var jobName = typeof(TJob).Name;
        using var jobRun = SchedulerMeter.JobRunning();
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["JobName"] = jobName,
        });

        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var jobInstance = scope.ServiceProvider.GetRequiredService<TJob>();
            _logger.LogDebug("Start running job...");
            await jobInstance.Run(cancellationToken).ConfigureAwait(false);
            _logger.LogDebug("Finished running job");
            SchedulerMeter.JobRun(jobName, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job execution failed");
            SchedulerMeter.JobRun(jobName, false);
        }
    }
}
