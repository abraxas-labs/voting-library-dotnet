// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using EventStore.Client;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;

namespace Voting.Lib.Eventing.Persistence;

/// <inheritdoc />
public class AggregateEventReader : IAggregateEventReader
{
    private readonly IEventSerializer _serializer;
    private readonly EventStoreClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateEventReader"/> class.
    /// </summary>
    /// <param name="serializer">The event serializer.</param>
    /// <param name="client">The EventStore client.</param>
    public AggregateEventReader(IEventSerializer serializer, EventStoreClient client)
    {
        _serializer = serializer;
        _client = client;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<IDomainEvent> ReadEvents(string stream, Guid aggregateId, DateTime? endTimestampInclusive = null)
    {
        var reader = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);
        var state = await reader.ReadState.ConfigureAwait(false);
        if (state == ReadState.StreamNotFound)
        {
            throw new AggregateNotFoundException(aggregateId);
        }

        await foreach (var eventData in reader)
        {
            if (endTimestampInclusive.HasValue && eventData.Event.Created > endTimestampInclusive)
            {
                yield break;
            }

            yield return Deserialize(aggregateId, eventData);
        }
    }

    private IDomainEvent Deserialize(Guid aggregateId, ResolvedEvent eventData)
    {
        var eventPayload = _serializer.Deserialize(eventData.Event);

        return new EventDataWrapper(
            eventData.Event.EventId.ToGuid(),
            aggregateId,
            eventPayload,
            StreamRevision.FromStreamPosition(eventData.Event.EventNumber),
            null,
            eventData.Event.Position,
            eventData.Event.Created);
    }
}
