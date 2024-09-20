// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Subscribe;
using Xunit;

namespace Voting.Lib.Eventing.IntegrationTest.Subscribe;

public class SubscribeTest : EventStoreSampleDataFixture
{
    [Fact]
    public async Task SubscribeToStreamShouldProcessEvents()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddSingleton<TestProcessorStore>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly)
            .AddSubscription<TransientTestScope>(MockEvents.TestStream1)
            .Services
            .BuildServiceProvider(true);

        await serviceProvider.StartHostedServices();

        var store = serviceProvider.GetRequiredService<TestProcessorStore>();
        var (nrOfEvents, sum) = await store.WaitForCatchUp();
        nrOfEvents.Should().Be(80);
        sum.Should().Be(4000);

        await serviceProvider.StopHostedServices();
    }

    [Fact]
    public async Task SubscribeToAllShouldProcessEvents()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddSingleton<TestProcessorStore>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly)
            .AddSubscription<TransientTestScope>(WellKnownStreams.All)
            .Services
            .BuildServiceProvider(true);

        await serviceProvider.StartHostedServices();

        var store = serviceProvider.GetRequiredService<TestProcessorStore>();
        var (nrOfEvents, sum) = await store.WaitForCatchUp();
        nrOfEvents.Should().Be(110);
        sum.Should().Be(4350);

        await serviceProvider.StopHostedServices();
    }

    [Fact]
    public async Task SubscribeShouldProcessLiveEvents()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddSingleton<TestProcessorStore>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly)
            .AddSubscription<TransientTestScope>(WellKnownStreams.All)
            .Services
            .BuildServiceProvider(true);

        await serviceProvider.StartHostedServices();

        var store = serviceProvider.GetRequiredService<TestProcessorStore>();
        await store.WaitForNumberOfEvents(2);
        store.LatestEventWasCatchUp.Should().BeTrue();

        var (nrOfEvents, sum) = await store.WaitForCatchUp();
        nrOfEvents.Should().Be(110);
        sum.Should().Be(4350);
        store.LatestEventWasCatchUp.Should().BeTrue();

        // 115 events in the default dataset, we publish additional 5
        var waitTask = store.WaitForNumberOfEvents(115);
        await MockEvents.PublishEvents(serviceProvider, MockEvents.TestStream1, 5);
        (nrOfEvents, sum) = await waitTask;
        nrOfEvents.Should().Be(115);
        sum.Should().Be(4360);
        store.LatestEventWasCatchUp.Should().BeFalse();

        await serviceProvider.StopHostedServices();
    }

    [Fact]
    public async Task SubscribeShouldCallRetryPolicyOnFailureAndReconnectFromSnapshot()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddForwardRefSingleton<IEventProcessingRetryPolicy<TransientTestScope>, TestRetryPolicy<TransientTestScope>>()
            .AddSingleton<TestProcessorStore>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly)
            .AddSubscription<TransientTestScope>(WellKnownStreams.All)
            .Services
            .BuildServiceProvider(true);

        var store = serviceProvider.GetRequiredService<TestProcessorStore>();
        store.EventProcessingShouldFail = true;

        await serviceProvider.StartHostedServices();

        var retryPolicy = serviceProvider.GetRequiredService<TestRetryPolicy<TransientTestScope>>();
        await retryPolicy.WaitForNrOfFailures(10);
        retryPolicy.SucceededCount.Should().Be(0);
        retryPolicy.FailureCount.Should().Be(10);
        store.EventProcessingShouldFail = false;

        var (nrOfEvents, sum) = await store.WaitForCatchUp();
        nrOfEvents.Should().Be(110);
        sum.Should().Be(4350);
        retryPolicy.SucceededCount.Should().Be(110);
        retryPolicy.FailureCount.Should().Be(10);

        await serviceProvider.StopHostedServices();
    }

    [Fact]
    public async Task SubscribeShouldCallRetryPolicyAndAbortIfNoRetry()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddForwardRefSingleton<IEventProcessingRetryPolicy<TransientTestScope>, TestRetryPolicy<TransientTestScope>>()
            .AddSingleton<TestProcessorStore>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly)
            .AddSubscription<TransientTestScope>(WellKnownStreams.All)
            .Services
            .BuildServiceProvider(true);

        var retryPolicy = serviceProvider.GetRequiredService<TestRetryPolicy<TransientTestScope>>();
        retryPolicy.OnRetry = (_, _) => Task.FromResult(false);

        var store = serviceProvider.GetRequiredService<TestProcessorStore>();
        store.EventProcessingShouldFail = true;

        await serviceProvider.StartHostedServices();

        await retryPolicy.WaitForNrOfFailures(1);
        retryPolicy.SucceededCount.Should().Be(0);
        retryPolicy.FailureCount.Should().Be(1);

        await serviceProvider.StopHostedServices();
    }
}
