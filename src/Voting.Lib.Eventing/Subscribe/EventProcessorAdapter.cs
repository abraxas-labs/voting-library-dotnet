// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

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

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessorAdapter{TScope, TEvent}"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    public EventProcessorAdapter(ICatchUpDetectorEventProcessor<TScope, TEvent> eventProcessor)
    {
        _eventProcessor = eventProcessor;
    }

    /// <inheritdoc />
    public async Task Process(EventWithMetadata eventWithMetadata, bool isCatchUp)
    {
        await _eventProcessor.Process((TEvent)eventWithMetadata.Data, isCatchUp).ConfigureAwait(false);
    }
}
