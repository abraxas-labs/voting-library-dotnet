// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Read;

namespace Voting.Lib.Eventing.Seeding;

internal class EventSeeder : IEventSeeder
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IEventReader _eventReader;

    public EventSeeder(IEventPublisher eventPublisher, IEventReader eventReader)
    {
        _eventPublisher = eventPublisher;
        _eventReader = eventReader;
    }

    /// <inheritdoc/>
    public async Task Seed(string stream, IEnumerable<IMessage> events)
    {
        var eventNumber = await _eventReader.GetLatestEventNumber(stream).ConfigureAwait(false);
        var eventsWithMetadata = events.Select(data => new EventWithMetadata(data, null, Uuid.NewUuid().ToGuid()));
        if (!eventNumber.HasValue)
        {
            await _eventPublisher.Publish(stream, eventsWithMetadata, null).ConfigureAwait(false);
            return;
        }

        var revision = StreamRevision.FromStreamPosition(eventNumber.Value);
        await _eventPublisher.Publish(stream, eventsWithMetadata.Skip((int)eventNumber.Value.Next().ToInt64()), revision).ConfigureAwait(false);
    }
}
