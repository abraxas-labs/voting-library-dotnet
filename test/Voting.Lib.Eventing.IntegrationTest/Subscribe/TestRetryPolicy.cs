// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.IntegrationTest.Subscribe;

public class TestRetryPolicy<TScope> : IEventProcessingRetryPolicy<TScope>
    where TScope : IEventProcessorScope
{
    private TaskCompletionSource? _failureWaiter;
    private int _failureWaitCounter;

    public int SucceededCount { get; set; }

    public int FailureCount { get; set; }

    public Func<int, SubscriptionDroppedReason, Task<bool>> OnRetry { get; set; } = (_, _) => Task.FromResult(true);

    public Task WaitForNrOfFailures(int count)
    {
        if (_failureWaiter != null)
        {
            throw new InvalidOperationException("Only one concurrent wait call is supported");
        }

        if (FailureCount >= count)
        {
            return Task.CompletedTask;
        }

        _failureWaitCounter = count;
        _failureWaiter = new();

        return _failureWaiter.Task;
    }

    public void Succeeded()
        => SucceededCount++;

    public Task<bool> Failed(SubscriptionDroppedReason reason)
    {
        FailureCount++;

        if (FailureCount >= _failureWaitCounter)
        {
            _failureWaiter?.SetResult();
            _failureWaiter = null;
        }

        return OnRetry(FailureCount, reason);
    }
}
