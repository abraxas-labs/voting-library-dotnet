// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Scheduler;

/// <inheritdoc />
public class JobConfig : IJobConfig
{
    /// <inheritdoc />
    public TimeSpan Interval { get; set; }

    /// <inheritdoc />
    public bool RunOnStart { get; set; }
}
