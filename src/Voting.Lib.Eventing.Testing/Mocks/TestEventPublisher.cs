// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.Persistence;
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
    /// <param name="data">The events.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(params IMessage[] data)
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
        => Publish(false, eventNumber, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="eventNumber">The event number of the first event.</param>
    /// <param name="data">The events.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(long eventNumber, params IMessage[] data)
        => Publish(false, eventNumber, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="isCatchUp">Whether the events are processed in catch up mode.</param>
    /// <param name="data">The events.</param>
    /// <typeparam name="TEvent">Type of the events.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish<TEvent>(bool isCatchUp, params TEvent[] data)
        where TEvent : IMessage<TEvent>
        => Publish(isCatchUp, 0L, data);

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="isCatchUp">Whether the events are processed in catch up mode.</param>
    /// <param name="data">The events.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(bool isCatchUp, params IMessage[] data)
        => Publish(isCatchUp, 0L, data);

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
        => Publish(isCatchUp, eventNumber, data.Cast<IMessage>().ToArray());

    /// <summary>
    /// Publishes test events.
    /// </summary>
    /// <param name="isCatchUp">Whether the events are processed in catch up mode.</param>
    /// <param name="eventNumber">The event number of the first event.</param>
    /// <param name="data">The events.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Publish(bool isCatchUp, long eventNumber, params IMessage[] data)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var eventProcessorScopes = scope.ServiceProvider.GetRequiredService<IEnumerable<IEventProcessorScope>>();
        foreach (var eventProcessorScope in eventProcessorScopes)
        {
            await Publish(eventProcessorScope.GetType(), isCatchUp, eventNumber, data).ConfigureAwait(false);
        }
    }

    private async Task Publish(Type eventProcessorScopeType, bool isCatchUp, long eventNumber, IEnumerable<IMessage> events)
    {
        var eventProcessorAdapterRegistry = typeof(EventProcessorAdapterRegistry<>).MakeGenericType(eventProcessorScopeType);

        var position = new Position((ulong)eventNumber, (ulong)eventNumber);
        var streamPosition = new StreamPosition((ulong)eventNumber);
        foreach (var eventData in events)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var registry = (EventProcessorAdapterRegistry)scope.ServiceProvider.GetRequiredService(eventProcessorAdapterRegistry);
            var processor = registry.GetProcessorAdapter(scope.ServiceProvider, eventData.GetType().FullName!);

            var eventProcessingScope = (IEventProcessorScope)scope.ServiceProvider.GetRequiredService(eventProcessorScopeType);
            await eventProcessingScope.Begin(position, streamPosition).ConfigureAwait(false);

            if (processor != null)
            {
                var eventSerializer = scope.ServiceProvider.GetRequiredService<IEventSerializer>();
                var data = eventSerializer.Serialize(eventData);
                await processor.Process(data, isCatchUp).ConfigureAwait(false);
            }

            await eventProcessingScope.Complete(position, streamPosition).ConfigureAwait(false);
            position = new Position(position.CommitPosition + 1, position.PreparePosition + 1);
            streamPosition = streamPosition.Next();
        }
    }
}
