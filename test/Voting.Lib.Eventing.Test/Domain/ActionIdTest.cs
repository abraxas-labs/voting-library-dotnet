// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using FluentAssertions;
using Google.Protobuf;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Domain;

public class ActionIdTest
{
    [Theory]
    [InlineData("action1", 2, 1, "4771768f8daf0c86b1ba3eb4f31119d2b02d1b6da118662dcb27b9c1d54c766e")]
    [InlineData("action2", 1, 2, "2c11fe5a1ed31f684339577056eba6031d1de1a4f7b2d79effa30fe14ef640f0")]
    [InlineData("action3", 0, 3, "81473f8bb31ae1caa9fc63f5c21a43ad2c5481675f6a76c8b7b1e5d5ad2e6855")]
    [InlineData("action4", 5, 4, "50b003b1d855905953eb24b66166b2d9ec7072680b65e2f65fd4535e9e409014")]
    [InlineData("action5", 15, 0, "8cf3004b9d45bfd8000188b8e8ba420311b4dd58ebd775623537cab499a444dd")]
    public void ShouldComputeHash(string action, int amountOfAggregates, int amountOfEvents, string hash)
    {
        var aggregates = Enumerable.Range(0, amountOfAggregates)
            .Select(i => new TestAggregate("aggregate" + i))
            .ToList();

        foreach (var aggregate in aggregates)
        {
            Enumerable.Range(0, amountOfEvents).ToList().ForEach(_ => aggregate.RaiseEvent());
        }

        new ActionId(action, aggregates.Cast<BaseEventSourcingAggregate>().ToArray())
            .ComputeHash()
            .Should()
            .Be(hash);
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public TestAggregate(string aggregateName)
        {
            Id = Guid.Parse("7630ae13-1748-49bf-a0d8-15b04b994f58");
            AggregateName = aggregateName;
        }

        public override string AggregateName { get; }

        public void RaiseEvent() => RaiseEvent(new TestEvent());

        protected override void Apply(IMessage eventData)
        {
        }
    }
}
