// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Messaging.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Messaging.Test;

public class MessagingTest
{
    [Fact]
    public async Task TestMessaging()
    {
        await using var sp = new ServiceCollection()
            .AddLogging()
            .AddVotingLibMessagingMocks(c => c.AddConsumerAndConsumerTestHarness<TestTransformConsumer>())
            .BuildServiceProvider(true);

        var harness = sp.GetRequiredService<InMemoryTestHarness>();
        await harness.Start();

        try
        {
            sp.GetRequiredService<IMessagingHealth>()
                .IsHealthy()
                .Should()
                .BeTrue();

            using var consumerCancellationTokenSource = new CancellationTokenSource();
            var producerTask = PublishValues(sp);
            var consumerTask = ConsumeValues(sp, consumerCancellationTokenSource.Token);

            await producerTask;

            var published = await harness.Published.SelectAsync<TestMessage>(CancellationToken.None).ToListAsync(CancellationToken.None);
            published.Should().HaveCount(4);

            consumerCancellationTokenSource.Cancel();

            var consumedValues = await consumerTask;
            consumedValues.Should().BeEquivalentTo(new[] { 1, 4 });
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task ShouldNotPublishIfUnhealthy()
    {
        await using var sp = new ServiceCollection()
            .AddLogging()
            .AddVotingLibMessagingMocks(c => c.AddConsumerAndConsumerTestHarness<TestTransformConsumer>())
            .RemoveAll<MessagingHealthMock>()
            .AddSingleton(new MessagingHealthMock(false))
            .BuildServiceProvider(true);

        var harness = sp.GetRequiredService<InMemoryTestHarness>();
        await harness.Start();

        try
        {
            sp.GetRequiredService<IMessagingHealth>()
                .IsHealthy()
                .Should()
                .BeFalse();

            using var consumerCancellationTokenSource = new CancellationTokenSource();
            await PublishValues(sp);

            var published = await harness.Published.SelectAsync<TestMessage>(CancellationToken.None).ToListAsync(CancellationToken.None);
            published.Should().BeEmpty();
        }
        finally
        {
            await harness.Stop();
        }
    }

    private async Task<IReadOnlyCollection<int>> ConsumeValues(IServiceProvider sp, CancellationToken listenerCancellationToken)
    {
        var values = new ConcurrentBag<int>();

        var consumer = sp.GetRequiredService<MessageConsumerHub<TestMessage, TestTransformedMessage>>();
        await consumer.Listen(
            m => m.ShouldPassClientFilter,
            m =>
            {
                values.Add(m.Value);
                return Task.CompletedTask;
            },
            listenerCancellationToken);
        return values;
    }

    private async Task PublishValues(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var producer = scope.ServiceProvider.GetRequiredService<MessageProducerBuffer>();
        producer.Add(new TestMessage { Value = 1, ShouldPassTransform = true, ShouldPassClientFilter = true });
        producer.Add(new TestMessage { Value = 2, ShouldPassTransform = false, ShouldPassClientFilter = true });
        producer.Add(new TestMessage { Value = 3, ShouldPassTransform = true, ShouldPassClientFilter = false });
        producer.Add(new TestMessage { Value = 4, ShouldPassTransform = true, ShouldPassClientFilter = true });
        await producer.TryComplete();
    }

    private class TestTransformConsumer : MessageConsumer<TestMessage, TestTransformedMessage>
    {
        public TestTransformConsumer(MessageConsumerHub<TestMessage, TestTransformedMessage> hub)
            : base(hub)
        {
        }

        protected override Task<TestTransformedMessage?> Transform(TestMessage message)
        {
            return message.ShouldPassTransform
                ? Task.FromResult<TestTransformedMessage?>(new() { Value = message.Value })
                : Task.FromResult<TestTransformedMessage?>(null);
        }
    }

    private class TestMessage
    {
        public bool ShouldPassTransform { get; set; }

        public bool ShouldPassClientFilter { get; set; }

        public int Value { get; set; }
    }

    private class TestTransformedMessage
    {
        public int Value { get; set; }
    }
}
