// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Voting.Lib.Messaging.IntegrationTest;

public class MessagingTest : RabbitMqFixture
{
    private interface ITestMessage
    {
        public int Value { get; }
    }

    [Fact]
    public Task PublishConsumeShouldWorkWithFilter()
        => PublishAndConsumeWithFilter<MessageConsumerHub<TestMessage>, TestMessage>(1100);

    [Fact]
    public Task PublishConsumeShouldWorkWithFilterAndTransform()
        => PublishAndConsumeWithFilter<MessageConsumerHub<TestMessage, TestTransformedMessage>, TestTransformedMessage>(1120);

    protected override void ConfigureMessaging(IServiceCollectionBusConfigurator bus)
    {
        bus.AddConsumer<MessageConsumer<TestMessage>>();
        bus.AddConsumer<TestTransformConsumer>();
    }

    private async Task PublishAndConsumeWithFilter<T, TTransformedMessage>(int expectedSum)
        where T : MessageConsumerHub<TestMessage, TTransformedMessage>
        where TTransformedMessage : class, ITestMessage
    {
        await using var scope = ServiceProvider.CreateAsyncScope();

        using var listenCancellationTokenSource = new CancellationTokenSource();
        listenCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
        var publisher = scope.ServiceProvider.GetRequiredService<MessageProducerBuffer>();
        var hub = scope.ServiceProvider.GetRequiredService<T>();

        var sum = 0;
        var taskCompletionSource = new TaskCompletionSource();
        var listenTask = hub.Listen(
            m => m.Value > 20,
            m =>
            {
                if (Interlocked.Add(ref sum, m.Value) == expectedSum)
                {
                    taskCompletionSource.SetResult();
                }

                return Task.CompletedTask;
            },
            listenCancellationTokenSource.Token);

        publisher.Add(new TestMessage(1));
        publisher.Add(new TestMessage(10));
        publisher.Add(new TestMessage(100));
        publisher.Add(new TestMessage(1000));
        await publisher.TryComplete();

        await taskCompletionSource.Task.WaitAsync(listenCancellationTokenSource.Token);
        listenCancellationTokenSource.Cancel();
        await listenTask;

        sum.Should().Be(expectedSum);
    }

    private record TestMessage(int Value) : ITestMessage;

    private record TestTransformedMessage(int Value) : ITestMessage;

    private class TestTransformConsumer : MessageConsumer<TestMessage, TestTransformedMessage>
    {
        public TestTransformConsumer(MessageConsumerHub<TestMessage, TestTransformedMessage> hub)
            : base(hub)
        {
        }

        protected override Task<TestTransformedMessage?> Transform(TestMessage message)
            => Task.FromResult<TestTransformedMessage?>(new(message.Value + 10));
    }
}
