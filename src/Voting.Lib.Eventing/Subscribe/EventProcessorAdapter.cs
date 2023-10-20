// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Adapter for processing events, helps with event deserialization.
/// </summary>
/// <typeparam name="TScope">The type of event processor scope.</typeparam>
/// <typeparam name="TEvent">The event type.</typeparam>
public class EventProcessorAdapter<TScope, TEvent> : IEventProcessorAdapter
    where TScope : IEventProcessorScope
    where TEvent : IMessage<TEvent>, new()
{
    private readonly ICatchUpDetectorEventProcessor<TScope, TEvent> _eventProcessor;
    private readonly IEventSerializer _serializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessorAdapter{TScope, TEvent}"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="serializer">The event serializer.</param>
    public EventProcessorAdapter(ICatchUpDetectorEventProcessor<TScope, TEvent> eventProcessor, IEventSerializer serializer)
    {
        _eventProcessor = eventProcessor;
        _serializer = serializer;
    }

    /// <inheritdoc />
    public async Task Process(ReadOnlyMemory<byte> eventData, bool isCatchUp)
    {
        var payload = _serializer.Deserialize<TEvent>(eventData);
        await _eventProcessor.Process(payload, isCatchUp).ConfigureAwait(false);
    }
}
