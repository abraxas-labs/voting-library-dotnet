// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Scheduler.Test.Mocks;

public class JobStore
{
    public int CountOfExecutions { get; set; }

    public int CountOfCancellations { get; set; }

    public TimeSpan JobExecutionTime { get; set; } = TimeSpan.FromSeconds(.5);
}
