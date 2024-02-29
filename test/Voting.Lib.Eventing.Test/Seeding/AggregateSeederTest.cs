// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Seeding;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Seeding;

public class AggregateSeederTest
{
    [Fact]
    public async Task ShouldSeed()
    {
        var eventSeederMock = new Mock<IEventSeeder>();
        eventSeederMock.Setup(x => x.Seed(
            "TestAggregate-e0023bba-a363-49da-a97b-b8748c250358",
            It.Is<IEnumerable<IMessage>>(e => e.Count() == 10 && e.Cast<TestEvent>().Select((ev, i) => ev.TestValue == i).All(ok => ok))))
            .Verifiable();

        await using var serviceProvider = new ServiceCollection()
            .AddSingleton<IAggregateSeedSource, TestAggregateSeedSource>()
            .BuildServiceProvider(true);

        var seeder = new AggregateSeeder(serviceProvider, eventSeederMock.Object, NullLogger<AggregateSeeder>.Instance);
        await seeder.StartAsync(CancellationToken.None);
        await seeder.StopAsync(CancellationToken.None);

        eventSeederMock.Verify();
    }

    private class TestAggregateSeedSource : IAggregateSeedSource
    {
        public Task<BaseEventSourcingAggregate> GetAggregate()
        {
            var aggregate = new TestAggregate();
            for (var i = 0; i < 10; i++)
            {
                aggregate.Add(i);
            }

            return Task.FromResult<BaseEventSourcingAggregate>(aggregate);
        }
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public TestAggregate()
        {
            Id = Guid.Parse("e0023bba-a363-49da-a97b-b8748c250358");
        }

        public override string AggregateName => "TestAggregate";

        public int Sum { get; private set; }

        public void Add(int v)
            => RaiseEvent(new TestEvent { TestValue = v });

        protected override void Apply(IMessage eventData)
        {
            var testEvent = (TestEvent)eventData;
            Sum += testEvent.TestValue;
        }
    }
}
