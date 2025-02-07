// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Scheduler;

/// <summary>
/// A background service that schedules cron jobs.
/// </summary>
/// <typeparam name="TJob">The type of job to run.</typeparam>
public sealed class CronSchedulerService<TJob> : BackgroundService
    where TJob : IScheduledJob
{
    private const char CronPartSeparator = ' ';
    private const int StandardCronFormatSeparatorCount = 4;
    private readonly JobRunner _jobRunner;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<CronSchedulerService<TJob>> _logger;
    private readonly CronExpression _cronExpression;
    private readonly TimeZoneInfo _timeZone;

    /// <summary>
    /// Initializes a new instance of the <see cref="CronSchedulerService{TJob}"/> class.
    /// </summary>
    /// <param name="jobRunner">The job runner.</param>
    /// <param name="jobConfig">The job configuration.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="logger">The logger.</param>
    public CronSchedulerService(JobRunner jobRunner, ICronJobConfig jobConfig, TimeProvider timeProvider, ILogger<CronSchedulerService<TJob>> logger)
    {
        _jobRunner = jobRunner;
        _timeProvider = timeProvider;
        _logger = logger;
        var cronFormat = jobConfig.CronSchedule.Count(x => x == CronPartSeparator) == StandardCronFormatSeparatorCount
            ? CronFormat.Standard
            : CronFormat.IncludeSeconds;
        _cronExpression = CronExpression.Parse(jobConfig.CronSchedule, cronFormat);
        _timeZone = TimeZoneInfo.FindSystemTimeZoneById(jobConfig.CronTimeZone);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = _timeProvider.GetLocalNow();
            var nextRun = _cronExpression.GetNextOccurrence(now, _timeZone);

            if (nextRun == null)
            {
                _logger.LogWarning("Cron expression does not have a next run!");
                return;
            }

            // configure await needs to be true, to ensure the time provider works correctly
            await Task.Delay(nextRun.Value - now, _timeProvider, stoppingToken).ConfigureAwait(true);
            await _jobRunner.RunJob<TJob>(stoppingToken).ConfigureAwait(true);
        }
    }
}
