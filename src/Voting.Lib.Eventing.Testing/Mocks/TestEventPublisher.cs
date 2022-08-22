// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// Provides methods to publish events for testing purposes.
/// </summary>
public class TestEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestEventPublisher"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public TestEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="data">The events.</param>
    /// <typeparam name="TEvent">Type of the events.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish<TEvent>(params TEvent[] data)
        where TEvent : IMessage<TEvent>
        => Publish(0, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="eventNumber">The event number of the first event.</param>
    /// <param name="data">The events.</param>
    /// <typeparam name="TEvent">Type of the events.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish<TEvent>(long eventNumber, params TEvent[] data)
        where TEvent : IMessage<TEvent>
        => PublishForAllScopes(false, eventNumber, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="isCatchUp">Whether the events are processed in catch up mode.</param>
    /// <param name="data">The events.</param>
    /// <typeparam name="TEvent">Type of the events.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish<TEvent>(bool isCatchUp, params TEvent[] data)
        where TEvent : IMessage<TEvent>
        => PublishForAllScopes(isCatchUp, 0L, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="isCatchUp">Whether the events are processed in catch up mode.</param>
    /// <param name="eventNumber">The event number of the first event.</param>
    /// <param name="data">The events.</param>
    /// <typeparam name="TEvent">Type of the events.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish<TEvent>(bool isCatchUp, long eventNumber, params TEvent[] data)
        where TEvent : IMessage<TEvent>
        => PublishForAllScopes(isCatchUp, eventNumber, data);

    private async Task PublishForAllScopes<TEvent>(bool isCatchUp, long eventNumber, TEvent[] events)
        where TEvent : IMessage<TEvent>
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var eventProcessorScopes = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventProcessorScope>>();
        foreach (var eventProcessorScope in eventProcessorScopes)
        {
            await Publish(eventProcessorScope.GetType(), isCatchUp, eventNumber, events).ConfigureAwait(false);
        }
    }

    private async Task Publish<TEvent>(Type eventProcessorScopeType, bool isCatchUp, long eventNumber, TEvent[] events)
        where TEvent : IMessage<TEvent>
    {
        var eventProcessorType = typeof(ICatchUpDetectorEventProcessor<,>).MakeGenericType(eventProcessorScopeType, typeof(TEvent));

        var position = new Position((ulong)eventNumber, (ulong)eventNumber);
        var streamPosition = new StreamPosition((ulong)eventNumber);
        foreach (var eventData in events)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var processor = scope.ServiceProvider.GetService(eventProcessorType) as IInternalEventProcessor<TEvent>;
            var eventProcessingScope = (IEventProcessorScope)scope.ServiceProvider.GetRequiredService(eventProcessorScopeType);
            await eventProcessingScope.Begin(position, streamPosition).ConfigureAwait(false);

            if (processor != null)
            {
                await processor.Process(eventData, isCatchUp).ConfigureAwait(false);
            }

            await eventProcessingScope.Complete(position, streamPosition).ConfigureAwait(false);
            position = new(position.CommitPosition + 1, position.PreparePosition + 1);
            streamPosition = streamPosition.Next();
        }
    }
}
