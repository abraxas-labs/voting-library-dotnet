// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.IntegrationTest.Subscribe;

public class TestProcessor :
    ICatchUpDetectorEventProcessor<TransientTestScope, TestEvent>,
    IEventProcessorCatchUpCompleter<TransientTestScope>
{
    private readonly TestProcessorStore _store;

    public TestProcessor(TestProcessorStore store)
    {
        _store = store;
    }

    public Task Process(TestEvent eventData, bool isCatchUp)
    {
        if (_store.EventProcessingShouldFail)
        {
            throw new InvalidOperationException(nameof(_store.EventProcessingShouldFail) + " is true");
        }

        _store.OnEvent(eventData, isCatchUp);
        return Task.CompletedTask;
    }

    public Task CatchUpCompleted()
    {
        _store.OnCatchUpCompleted();
        return Task.CompletedTask;
    }
}
