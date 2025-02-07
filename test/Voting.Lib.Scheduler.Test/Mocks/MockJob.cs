// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Scheduler.Test.Mocks;

public class MockJob : IScheduledJob
{
    private readonly JobStore _store;
    private readonly TimeProvider _timeProvider;

    public MockJob(JobStore store, TimeProvider timeProvider)
    {
        _store = store;
        _timeProvider = timeProvider;
    }

    public async Task Run(CancellationToken ct)
    {
        _store.StartedAt.Add(_timeProvider.GetLocalNow());

        try
        {
            // configure await true to ensure time provider works correct in tests
            await Task.Delay(_store.JobExecutionTime, _timeProvider, ct).ConfigureAwait(true);
        }
        catch (TaskCanceledException)
        {
            _store.CancelledAt.Add(_timeProvider.GetLocalNow());
        }
    }
}
