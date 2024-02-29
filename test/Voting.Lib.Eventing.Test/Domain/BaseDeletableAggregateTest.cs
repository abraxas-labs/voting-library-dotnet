// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Google.Protobuf;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Domain;

public class BaseDeletableAggregateTest
{
    [Fact]
    public void ShouldWorkAndThrowAfterSecondTimeDeletion()
    {
        var aggregate = new TestAggregate();
        aggregate.Delete();
        aggregate.Deleted.Should().BeTrue();
        Assert.Throws<AggregateDeletedException>(() => aggregate.Delete());
    }

    private class TestAggregate : BaseDeletableAggregate
    {
        public override string AggregateName => "TestAggregate";

        public void Delete()
        {
            EnsureNotDeleted();
            RaiseEvent(new TestEvent());
        }

        protected override void Apply(IMessage eventData)
        {
            Deleted = true;
        }
    }
}
