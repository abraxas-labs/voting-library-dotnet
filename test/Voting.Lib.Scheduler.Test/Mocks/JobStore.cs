// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Scheduler.Test.Mocks;

public class JobStore
{
    public List<DateTimeOffset> StartedAt { get; } = new();

    public List<DateTimeOffset> CancelledAt { get; } = new();

    public TimeSpan JobExecutionTime { get; set; } = TimeSpan.FromSeconds(.5);
}
