// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using FluentAssertions;
using Google.Protobuf;
using Moq;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Persistence;

public class AggregateRepositoryTest
{
    private static readonly Guid TestAggregateId = Guid.Parse("59e1916a-a128-4c02-85c1-73a287f5a238");

    [Fact]
    public async Task GetByIdShouldWork()
    {
        var events = Enumerable.Range(0, 3)
            .Select(x => (IDomainEvent)new DomainEventMock(TestAggregateId, x, StreamRevision.FromInt64(x)))
            .AsAsync();
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Returns(events)
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var publisherMock = new Mock<IEventPublisher>();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();

        var repo = new AggregateRepository(publisherMock.Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.GetById<TestAggregate>(TestAggregateId);
        aggregate.Sum.Should().Be(3);
        aggregate.Version!.Value.Should().Be(2);
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task GetSnapshotByIdShouldReadInclusiveTimestamp()
    {
        var events = Enumerable.Range(0, 20)
            .Select(x => (IDomainEvent)new DomainEventMock(TestAggregateId, x, StreamRevision.FromInt64(x)))
            .AsAsync();
        var snapshotTimestamp = new DateTime(10);
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, snapshotTimestamp))
            .Returns(events)
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var publisherMock = new Mock<IEventPublisher>();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();

        var repo = new AggregateRepository(publisherMock.Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.GetSnapshotById<TestAggregate>(TestAggregateId, snapshotTimestamp);
        aggregate.Sum.Should().Be(190);
        aggregate.Version!.Value.Should().Be(19);
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task TryGetByIdShouldReturnIfFound()
    {
        var events = Enumerable.Range(0, 3)
            .Select(x => (IDomainEvent)new DomainEventMock(TestAggregateId, x, StreamRevision.FromInt64(x)))
            .AsAsync();
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Returns(events)
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var publisherMock = new Mock<IEventPublisher>();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();

        var repo = new AggregateRepository(publisherMock.Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.TryGetById<TestAggregate>(TestAggregateId);
        aggregate.Should().NotBeNull();
        aggregate!.Sum.Should().Be(3);
        aggregate.Version!.Value.Should().Be(2);
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task TryGetByIdShouldReturnNullIfNotFound()
    {
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Throws(new AggregateNotFoundException(TestAggregateId))
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();
        var repo = new AggregateRepository(new Mock<IEventPublisher>().Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.TryGetById<TestAggregate>(TestAggregateId);
        aggregate.Should().BeNull();
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task GetOrCreateByIdShouldCreateIfNotFound()
    {
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Throws(new AggregateNotFoundException(TestAggregateId))
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();
        var repo = new AggregateRepository(new Mock<IEventPublisher>().Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.GetOrCreateById<TestAggregate>(TestAggregateId);
        aggregate.Should().NotBeNull();
        aggregate.Version.Should().BeNull();
        aggregate.OriginalVersion.Should().BeNull();
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task GetOrCreateByIdShouldReturnExistingIfNotFound()
    {
        var events = Enumerable.Range(0, 3)
            .Select(x => (IDomainEvent)new DomainEventMock(TestAggregateId, x, StreamRevision.FromInt64(x)))
            .AsAsync();
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Returns(events)
            .Verifiable();

        var factoryMock = NewFactoryMock();
        var publisherMock = new Mock<IEventPublisher>();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();

        var repo = new AggregateRepository(publisherMock.Object, eventReaderMock.Object, factoryMock.Object, handlerMock.Object);
        var aggregate = await repo.GetOrCreateById<TestAggregate>(TestAggregateId);
        aggregate.Sum.Should().Be(3);
        aggregate.Version!.Value.Should().Be(2);
        factoryMock.Verify();
        eventReaderMock.Verify();
    }

    [Fact]
    public async Task SaveShouldPublishAndClearUncommittedEventsForExistingAggregate()
    {
        var events = Enumerable.Range(0, 3)
            .Select(x => (IDomainEvent)new DomainEventMock(TestAggregateId, x, StreamRevision.FromInt64(x)))
            .AsAsync();
        var eventReaderMock = new Mock<IAggregateEventReader>();
        eventReaderMock
            .Setup(x => x.ReadEvents("TestAggregate-" + TestAggregateId, TestAggregateId, null))
            .Returns(events);

        var publisherMock = new Mock<IEventPublisher>();
        publisherMock
            .Setup(x => x.Publish(
                "TestAggregate-" + TestAggregateId,
                It.Is<IEnumerable<EventWithMetadata>>(e => e.Count() == 2),
                StreamRevision.FromInt64(2)))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var handlerMock = new Mock<IAggregateRepositoryHandler>();
        handlerMock
            .Setup(x => x.BeforeSaved(
                It.Is<TestAggregate>(e => e.GetUncommittedEvents().Count == 2 && e.OriginalVersion!.Value == 2)))
            .Verifiable();
        handlerMock
            .Setup(x => x.AfterSaved(
                It.Is<TestAggregate>(e => e.GetUncommittedEvents().Count == 0 && e.OriginalVersion!.Value == 4),
                It.Is<IReadOnlyCollection<IDomainEvent>>(e => e.Count == 2)))
            .Verifiable();

        var repo = new AggregateRepository(publisherMock.Object, eventReaderMock.Object, NewFactoryMock().Object, handlerMock.Object);
        var aggregate = await repo.GetById<TestAggregate>(TestAggregateId);
        aggregate.OriginalVersion!.Value.Should().Be(2);
        aggregate.Version!.Value.Should().Be(2);
        aggregate.Add();
        aggregate.Add();
        aggregate.OriginalVersion!.Value.Should().Be(2);
        aggregate.Version!.Value.Should().Be(4);
        await repo.Save(aggregate);

        aggregate.OriginalVersion!.Value.Should().Be(4);
        aggregate.Version!.Value.Should().Be(4);
        aggregate.GetUncommittedEvents().Should().BeEmpty();
        publisherMock.Verify();
        handlerMock.Verify();
    }

    [Fact]
    public async Task SaveShouldPublishAndClearUncommittedEventsForNewAggregate()
    {
        var publisherMock = new Mock<IEventPublisher>();
        publisherMock
            .Setup(x => x.Publish(
                "TestAggregate-" + TestAggregateId,
                It.Is<IEnumerable<EventWithMetadata>>(e => e.Count() == 3),
                null))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var handlerMock = new Mock<IAggregateRepositoryHandler>();
        handlerMock
            .Setup(x => x.BeforeSaved(
                It.Is<TestAggregate>(e => e.GetUncommittedEvents().Count == 3 && !e.OriginalVersion.HasValue)))
            .Verifiable();
        handlerMock
            .Setup(x => x.AfterSaved(
                It.Is<TestAggregate>(e => e.GetUncommittedEvents().Count == 0 && e.OriginalVersion!.Value == 2),
                It.Is<IReadOnlyCollection<IDomainEvent>>(e => e.Count == 3)))
            .Verifiable();

        var repo = new AggregateRepository(publisherMock.Object, new Mock<IAggregateEventReader>().Object, NewFactoryMock().Object, handlerMock.Object);
        var aggregate = new TestAggregate();
        aggregate.Add();
        aggregate.Add();
        aggregate.Add();
        aggregate.OriginalVersion.Should().BeNull();
        aggregate.Version!.Value.Should().Be(2);
        await repo.Save(aggregate);

        aggregate.OriginalVersion!.Value.Should().Be(2);
        aggregate.Version!.Value.Should().Be(2);
        aggregate.GetUncommittedEvents().Should().BeEmpty();
        publisherMock.Verify();
        handlerMock.Verify();
    }

    private Mock<IAggregateFactory> NewFactoryMock()
    {
        var factoryMock = new Mock<IAggregateFactory>();
        factoryMock
            .Setup(x => x.New<TestAggregate>())
            .Returns(() => new())
            .Verifiable();
        return factoryMock;
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public TestAggregate()
        {
            Id = Guid.Parse("59e1916a-a128-4c02-85c1-73a287f5a238");
        }

        public override string AggregateName => "TestAggregate";

        public int Sum { get; private set; }

        public void Add()
            => RaiseEvent(new TestEvent { TestValue = Sum + 1 });

        protected override void Apply(IMessage eventData)
        {
            Sum += ((TestEvent)eventData).TestValue;
        }
    }

    private class DomainEventMock : IDomainEvent
    {
        public DomainEventMock(Guid aggregateId, int testValue, StreamRevision revision)
        {
            AggregateId = aggregateId;
            AggregateVersion = revision;
            Data = new TestEvent { TestValue = testValue };
            Created = new DateTime(revision.ToInt64());
        }

        public Guid Id { get; } = Guid.NewGuid();

        public Guid AggregateId { get; }

        public IMessage Data { get; }

        public IMessage? Metadata => null;

        public StreamRevision AggregateVersion { get; }

        public DateTime? Created { get; }

        public Position? Position => null;
    }
}
