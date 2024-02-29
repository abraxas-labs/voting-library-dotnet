// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Scheduler;

/// <inheritdoc />
public class CronJobConfig : ICronJobConfig
{
    /// <inheritdoc />
    public string CronSchedule { get; set; } = string.Empty;

    /// <inheritdoc />
    public string CronTimeZone { get; set; } = string.Empty;
}
