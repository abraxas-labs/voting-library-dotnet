// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Voting.Lib.Eventing.IntegrationTest.Events;

namespace Voting.Lib.Eventing.IntegrationTest.Subscribe;

public class TestProcessorStore
{
    private TaskCompletionSource<(int NumberOfEvents, int Sum)>? _catchUpWaiter;
    private TaskCompletionSource<(int NumberOfEvents, int Sum)>? _eventWaiter;
    private int _eventWaiterNumberOfEvents;

    public int Sum { get; private set; }

    public int NumberOfProcessedEvents { get; private set; }

    public bool LatestEventWasCatchUp { get; private set; }

    public bool EventProcessingShouldFail { get; set; }

    public Task<(int NumberOfProcessedEvents, int Sum)> WaitForNumberOfEvents(int count)
    {
        if (_eventWaiter != null)
        {
            throw new InvalidOperationException("Only one concurrent wait call is supported");
        }

        if (NumberOfProcessedEvents >= count)
        {
            return Task.FromResult((NumberOfProcessedEvents, Sum));
        }

        _eventWaiterNumberOfEvents = count;
        _eventWaiter = new();
        return _eventWaiter.Task;
    }

    public Task<(int NumberOfProcessedEvents, int Sum)> WaitForCatchUp()
    {
        if (_catchUpWaiter != null)
        {
            throw new InvalidOperationException("Only one concurrent wait call is supported");
        }

        _catchUpWaiter = new();
        return _catchUpWaiter.Task;
    }

    public void OnEvent(TestEvent eventData, bool isCatchUp)
    {
        Sum += eventData.TestValue;
        NumberOfProcessedEvents++;
        LatestEventWasCatchUp = isCatchUp;

        if (NumberOfProcessedEvents >= _eventWaiterNumberOfEvents)
        {
            _eventWaiter?.SetResult((NumberOfProcessedEvents, Sum));
            _eventWaiter = null;
        }
    }

    public void OnCatchUpCompleted()
    {
        _catchUpWaiter?.SetResult((NumberOfProcessedEvents, Sum));
        _catchUpWaiter = null;
    }
}
