// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Diagnostics;
using Voting.Lib.Eventing.Exceptions;
using EventStoreEventData = EventStore.Client.EventData;

namespace Voting.Lib.Eventing.Persistence;
internal class EventPublisher : IEventPublisher
{
    private readonly EventStoreClient _client;
    private readonly IEventSerializer _serializer;

    public EventPublisher(EventStoreClient client, IEventSerializer serializer)
    {
        _client = client;
        _serializer = serializer;
    }

    /// <inheritdoc />
    public Task Publish(string stream, EventWithMetadata @event, StreamRevision? expectedRevision)
        => Publish(stream, new[] { @event }, expectedRevision);

    /// <inheritdoc />
    public Task Publish(string stream, IEnumerable<EventWithMetadata> events, StreamRevision? expectedRevision)
        => PublishEvents(stream, events, expectedRevision, StreamState.NoStream);

    /// <inheritdoc />
    public Task PublishWithoutIdempotencyGuarantee(string stream, EventWithMetadata @event)
        => PublishWithoutIdempotencyGuarantee(stream, new[] { @event });

    /// <inheritdoc />
    public Task PublishWithoutIdempotencyGuarantee(string stream, IEnumerable<EventWithMetadata> events)
        => PublishEvents(stream, events, null, StreamState.Any);

    private async Task PublishEvents(
        string stream,
        IEnumerable<EventWithMetadata> events,
        StreamRevision? expectedRevision,
        StreamState fallbackExpectedState)
    {
        var mappedEvents = BuildEventData(events).ToList();
        try
        {
            if (expectedRevision.HasValue)
            {
                await _client.AppendToStreamAsync(stream, expectedRevision.Value, mappedEvents).ConfigureAwait(false);
            }
            else
            {
                await _client.AppendToStreamAsync(stream, fallbackExpectedState, mappedEvents).ConfigureAwait(false);
            }

            EventingMeter.EventsPublished(mappedEvents.Count);
        }
        catch (WrongExpectedVersionException ex)
        {
            throw new VersionMismatchException(ex, stream, ex.ExpectedStreamRevision, ex.ActualStreamRevision);
        }
    }

    private IEnumerable<EventStoreEventData> BuildEventData(IEnumerable<EventWithMetadata> events)
    {
        return events.Select(ev =>
            new EventStoreEventData(
                Uuid.FromGuid(ev.Id),
                ev.Data.Descriptor.FullName,
                _serializer.Serialize(ev.Data),
                ev.Metadata != null ? _serializer.Serialize(ev.Metadata) : null,
                _serializer.ContentType));
    }
}
