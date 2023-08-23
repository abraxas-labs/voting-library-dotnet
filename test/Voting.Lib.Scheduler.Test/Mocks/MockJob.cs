// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Scheduler.Test.Mocks;

public class MockJob : IScheduledJob
{
    private readonly JobStore _store;

    public MockJob(JobStore store)
    {
        _store = store;
    }

    public async Task Run(CancellationToken ct)
    {
        _store.CountOfExecutions++;

        try
        {
            await Task.Delay(_store.JobExecutionTime, ct);
        }
        catch (TaskCanceledException)
        {
            _store.CountOfCancellations++;
        }
    }
}
