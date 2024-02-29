// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.Domain;
using Xunit;

namespace Voting.Lib.Eventing.Test.Domain;

public class AggregateFactoryTest
{
    [Fact]
    public void ShouldResolveAggregate()
    {
        using var serviceProvider = new ServiceCollection()
            .AddTransient<TestAggregate>()
            .BuildServiceProvider(true);

        var factory = new AggregateFactory(serviceProvider);
        var instance1 = factory.New<TestAggregate>();
        instance1.Should().NotBeNull();

        var instance2 = factory.New<TestAggregate>();
        instance2.Should().NotBeNull();
        instance1.Should().NotBe(instance2);
    }

    private class TestAggregate : BaseEventSourcingAggregate
    {
        public override string AggregateName => "TestAggregate";

        protected override void Apply(IMessage eventData)
        {
        }
    }
}
