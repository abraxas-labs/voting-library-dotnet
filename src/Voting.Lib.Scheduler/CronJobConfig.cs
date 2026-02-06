// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common;

namespace Voting.Lib.Scheduler;

/// <inheritdoc />
public class CronJobConfig : ICronJobConfig
{
    /// <inheritdoc />
    public string CronSchedule { get; set; } = string.Empty;

    /// <inheritdoc />
    public string CronTimeZone { get; set; } = DateTimeConstants.EuropeZurichTimeZoneId;
}
