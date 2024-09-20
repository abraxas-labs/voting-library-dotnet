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
    private readonly ILogger<CronSchedulerService<TJob>> _logger;
    private readonly CronExpression _cronExpression;
    private readonly TimeZoneInfo _timeZone;

    /// <summary>
    /// Initializes a new instance of the <see cref="CronSchedulerService{TJob}"/> class.
    /// </summary>
    /// <param name="jobRunner">The job runner.</param>
    /// <param name="jobConfig">The job configuration.</param>
    /// <param name="logger">The logger.</param>
    public CronSchedulerService(JobRunner jobRunner, ICronJobConfig jobConfig, ILogger<CronSchedulerService<TJob>> logger)
    {
        _jobRunner = jobRunner;
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
            var now = DateTimeOffset.Now;
            var nextRun = _cronExpression.GetNextOccurrence(now, _timeZone);

            if (nextRun == null)
            {
                _logger.LogWarning("Cron expression does not have a next run!");
                return;
            }

            await Task.Delay(nextRun.Value - now, stoppingToken).ConfigureAwait(false);
            await _jobRunner.RunJob<TJob>(stoppingToken).ConfigureAwait(false);
        }
    }
}
