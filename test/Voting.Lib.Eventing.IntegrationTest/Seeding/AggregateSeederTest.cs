// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Seeding;
using Xunit;

namespace Voting.Lib.Eventing.IntegrationTest.Seeding;

public class AggregateSeederTest : EventStoreFixture
{
    [Fact]
    public async Task ShouldSeedEvents()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddSingleton<IAggregateSeedSource, TestAggregateSeedSource>()
            .AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly).AddPublishing().Services
            .BuildServiceProvider(true);

        await serviceProvider.StartHostedServices();

        var writtenEvents = await serviceProvider
            .GetRequiredService<EventStoreClient>()
            .ReadStreamAsync(Direction.Forwards, "EventSeederTest-" + Guid.Empty, StreamPosition.Start)
            .ToListAsync();

        writtenEvents.Should().HaveCount(5);
        Encoding.UTF8.GetString(writtenEvents[0].Event.Data.ToArray()).Should().Be("{ \"testValue\": 1 }");

        await serviceProvider.StopHostedServices();
    }

    private class TestAggregateSeedSource : IAggregateSeedSource
    {
        public Task<BaseEventSourcingAggregate> GetAggregate()
        {
            var aggregate = new TestAggregate();
            for (var i = 0; i < 5; i++)
            {
                aggregate.Add();
            }

            return Task.FromResult<BaseEventSourcingAggregate>(aggregate);
        }
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public override string AggregateName => "EventSeederTest";

        public int EventsCounter { get; private set; }

        public void Add()
            => RaiseEvent(new TestEvent { TestValue = EventsCounter + 1 });

        protected override void Apply(IMessage eventData)
        {
            EventsCounter++;
        }
    }
}
