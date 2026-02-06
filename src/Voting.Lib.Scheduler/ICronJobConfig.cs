// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common;

namespace Voting.Lib.Scheduler;

/// <summary>
/// Configuration for a cron job.
/// </summary>
public interface ICronJobConfig
{
    /// <summary>
    /// Gets the Cron schedule for the background job.
    /// </summary>
    public string CronSchedule { get; }

    /// <summary>
    /// Gets the time zone in which the cron schedule should be evaluated.
    /// </summary>
    public string CronTimeZone => DateTimeConstants.EuropeZurichTimeZoneId;
}
