// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.IntegrationTest.Subscribe;

public class TestRetryPolicy : IEventProcessingRetryPolicy
{
    private TaskCompletionSource? _failureWaiter;
    private int _failureWaitCounter;

    public int SucceededCounter { get; set; }

    public int FailedCounter { get; set; }

    public Func<int, SubscriptionDroppedReason, Task<bool>> OnRetry { get; set; } = (_, _) => Task.FromResult(true);

    public Task WaitForNrOfFailures(int count)
    {
        if (_failureWaiter != null)
        {
            throw new InvalidOperationException("Only one concurrent wait call is supported");
        }

        if (FailedCounter >= count)
        {
            return Task.CompletedTask;
        }

        _failureWaitCounter = count;
        _failureWaiter = new();

        return _failureWaiter.Task;
    }

    public void Succeeded()
        => SucceededCounter++;

    public Task<bool> Failed(SubscriptionDroppedReason reason)
    {
        FailedCounter++;

        if (FailedCounter >= _failureWaitCounter)
        {
            _failureWaiter?.SetResult();
            _failureWaiter = null;
        }

        return OnRetry(FailedCounter, reason);
    }
}
