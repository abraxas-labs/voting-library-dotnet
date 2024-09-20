// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.DependencyInjection;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Persistence;
using Xunit;

namespace Voting.Lib.Eventing.IntegrationTest.Persistence;

public class AggregateTest : EventStoreSampleDataFixture
{
    private AsyncServiceScope _scope; // initialized during InitializeAsync
    private IAggregateRepository _repo = null!; // initialized during InitializeAsync

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _scope = GetService<IServiceScopeFactory>().CreateAsyncScope();
        _repo = _scope.ServiceProvider.GetRequiredService<IAggregateRepository>();
    }

    [Fact]
    public async Task GetByIdShouldReadExistingEvents()
    {
        var aggregate = await _repo.GetById<TestAggregate>(MockEvents.AggregateId1);
        aggregate.Sum1.Should().Be(4000);
        aggregate.Sum2.Should().Be(950);
    }

    [Fact]
    public async Task GetUpdateAndSaveShouldWork()
    {
        var aggregate = await _repo.GetById<TestAggregate>(MockEvents.AggregateId1);
        aggregate.Sum1.Should().Be(4000);
        aggregate.Sum2.Should().Be(950);

        aggregate.Add();
        aggregate.Add2();
        aggregate.Add();
        aggregate.Sum1.Should().Be(16003);
        aggregate.Sum2.Should().Be(1901);

        await _repo.Save(aggregate);
        aggregate.Sum1.Should().Be(16003);
        aggregate.Sum2.Should().Be(1901);

        aggregate = await _repo.GetById<TestAggregate>(MockEvents.AggregateId1);
        aggregate.Sum1.Should().Be(16003);
        aggregate.Sum2.Should().Be(1901);
    }

    [Fact]
    public async Task GetUpdateWhileConcurrentUpdateOccuredShouldThrow()
    {
        var aggregate = await _repo.GetById<TestAggregate>(MockEvents.AggregateId1);
        aggregate.Add();

        await MockEvents.PublishEvents(_scope.ServiceProvider, MockEvents.TestStream1, 1);
        await Assert.ThrowsAsync<VersionMismatchException>(async () => await _repo.Save(aggregate));
    }

    protected override void ConfigureEventing(IEventingServiceCollection eventing)
    {
        base.ConfigureEventing(eventing);
        eventing.AddPublishing<TestAggregate>();
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public override string AggregateName => MockEvents.TestStreamName;

        public int Sum1 { get; private set; }

        public int Sum2 { get; private set; }

        public void Add()
            => RaiseEvent(new TestEvent { TestValue = Sum1 + 1 });

        public void Add2()
            => RaiseEvent(new TestEvent2 { TestValue2 = Sum2 + 1 });

        protected override void Apply(IMessage eventData)
        {
            switch (eventData)
            {
                case TestEvent e:
                    Sum1 += e.TestValue;
                    break;
                case TestEvent2 e:
                    Sum2 += e.TestValue2;
                    break;
            }
        }
    }
}
