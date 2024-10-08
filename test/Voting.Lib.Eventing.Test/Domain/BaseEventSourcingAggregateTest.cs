// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using FluentAssertions;
using Google.Protobuf;
using Moq;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Domain;

public class BaseEventSourcingAggregateTest
{
    [Fact]
    public void StreamNameShouldBeAggregateNameAndId()
    {
        var aggregate = new TestAggregate();
        aggregate.StreamName.Should().Be("TestAggregate-1e48a477-44a6-4de2-9f7c-4569c4d37c31");
    }

    [Fact]
    public void VersionShouldAdjustButOriginalVersionShouldStayTheSame()
    {
        var aggregate = new TestAggregate();
        aggregate.Version.Should().BeNull();
        aggregate.OriginalVersion.Should().BeNull();

        aggregate.Add();
        aggregate.Version.HasValue.Should().BeTrue();
        aggregate.Version!.Value.Should().Be(0UL);
        aggregate.OriginalVersion.Should().BeNull();

        aggregate.Add();
        aggregate.Version.HasValue.Should().BeTrue();
        aggregate.Version!.Value.Should().Be(1UL);
        aggregate.OriginalVersion.Should().BeNull();
    }

    [Fact]
    public void VersionShouldAdjustButOriginalVersionShouldStayTheSameWithInitialOriginalVersion()
    {
        var domainEventMock = new Mock<IDomainEvent>();
        domainEventMock.Setup(x => x.AggregateVersion).Returns(10UL);
        domainEventMock.Setup(x => x.Data).Returns(new TestEvent { TestValue = 10 });

        var aggregate = new TestAggregate();
        aggregate.ApplyEvent(domainEventMock.Object);
        aggregate.OriginalVersion = aggregate.Version;

        aggregate.Add();
        aggregate.Version.HasValue.Should().BeTrue();
        aggregate.Version!.Value.Should().Be(11UL);
        aggregate.OriginalVersion.HasValue.Should().BeTrue();
        aggregate.OriginalVersion!.Value.Should().Be(10UL);
    }

    [Fact]
    public void RaiseEventShouldAddUncommittedEvent()
    {
        var aggregate = new TestAggregate();
        aggregate.Add();
        aggregate.Add();

        var uncommittedEvents = aggregate.GetUncommittedEvents();
        uncommittedEvents.Should().HaveCount(2);
        var eventData = (TestEvent)uncommittedEvents.ElementAt(0).Data;
        var eventData2 = (TestEvent)uncommittedEvents.ElementAt(1).Data;
        eventData.TestValue.Should().Be(1);
        eventData2.TestValue.Should().Be(2);
    }

    [Fact]
    public void ClearUncommittedEventsShouldClearUncommitted()
    {
        var aggregate = new TestAggregate();
        aggregate.Add();
        aggregate.Add();

        aggregate.GetUncommittedEvents().Should().HaveCount(2);
        aggregate.ClearUncommittedEvents();
        aggregate.GetUncommittedEvents().Should().BeEmpty();
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public TestAggregate()
        {
            Id = Guid.Parse("1e48a477-44a6-4de2-9f7c-4569c4d37c31");
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
}
