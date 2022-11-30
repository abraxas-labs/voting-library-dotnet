// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.Extensions.Logging;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.Read;

/// <inheritdoc />
public class EventReader : IEventReader
{
    private readonly IEventSerializer _serializer;
    private readonly EventStoreClient _client;
    private readonly EventTypeResolver _eventTypeResolver;
    private readonly ILogger<EventReader> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventReader"/> class.
    /// </summary>
    /// <param name="serializer">The event serializer.</param>
    /// <param name="client">The EventStore client.</param>
    /// <param name="eventTypeResolver">The event type resolver.</param>
    /// <param name="logger">The logger.</param>
    public EventReader(IEventSerializer serializer, EventStoreClient client, EventTypeResolver eventTypeResolver, ILogger<EventReader> logger)
    {
        _serializer = serializer;
        _client = client;
        _eventTypeResolver = eventTypeResolver;
        _logger = logger;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<EventReadResult> ReadEvents(string stream, Func<IMessage, IDescriptor>? metadataDescriptorProvider = null, bool ignoreUnknownEvents = true)
    {
        await using var reader = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);
        var state = await reader.ReadState;
        if (state == ReadState.StreamNotFound)
        {
            throw new StreamNotFoundException(stream);
        }

        await foreach (var eventData in reader)
        {
            if (TryDeserialize(eventData, metadataDescriptorProvider, out var eventReadResult))
            {
                yield return eventReadResult;
                continue;
            }

            if (!ignoreUnknownEvents)
            {
                throw new UnknownEventException(eventData);
            }

            var level = eventData.Event.EventType.StartsWith(EventStoreConstants.SystemEventTypePrefix, StringComparison.InvariantCulture)
                ? LogLevel.Debug
                : LogLevel.Warning;
            _logger.Log(level, "Encountered unknown event {EventType} with number {EventNumber} in {Stream}", eventData.Event.EventType, eventData.OriginalEventNumber, stream);
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<EventReadResult> ReadEventsFromAll(
        Position startPositionExclusive,
        Func<EventReadResult, bool> endCondition,
        Func<IMessage, IDescriptor>? metadataDescriptorProvider = null,
        bool ignoreUnknownEvents = true)
    {
        await foreach (var eventData in _client.ReadAllAsync(Direction.Forwards, startPositionExclusive))
        {
            if (!TryDeserialize(eventData, metadataDescriptorProvider, out var eventReadResult))
            {
                if (!ignoreUnknownEvents)
                {
                    throw new UnknownEventException(eventData);
                }

                var level = eventData.Event.EventType.StartsWith(EventStoreConstants.SystemEventTypePrefix, StringComparison.InvariantCulture)
                    ? LogLevel.Debug
                    : LogLevel.Warning;
                _logger.Log(level, "Encountered unknown event {EventType} with number {EventPosition}", eventData.Event.EventType, eventData.OriginalPosition);
                continue;
            }

            yield return eventReadResult;

            if (endCondition(eventReadResult))
            {
                yield break;
            }
        }
    }

    /// <inheritdoc />
    // TODO: not needed anymore with https://jira.abraxas-tools.ch/jira/browse/VOTING-1856.
    public async IAsyncEnumerable<EventReadResult> ReadEventsFromAll(
        Position startPositionExclusive,
        IReadOnlyCollection<Type> eventTypes,
        Func<EventReadResult, bool> endCondition,
        Func<IMessage, IDescriptor>? metadataDescriptorProvider = null)
    {
        // we need to get the end position to know when the $all subscription has catched up.
        var endPosition = await GetLatestEventPosition(WellKnownStreams.All);

        var eventDescriptorFullNames = _eventTypeResolver.GetDescriptorNames(eventTypes);
        var eventsChannel = Channel.CreateUnbounded<EventReadResult>();

        using var subscription = await _client.SubscribeToAllAsync(
            startPositionExclusive,
            (_, data, _) =>
            {
                if (!TryDeserialize(data, metadataDescriptorProvider, out var eventReadResult))
                {
                    throw new UnknownEventException(data);
                }

                eventsChannel.Writer.TryWrite(eventReadResult);

                if (endCondition(eventReadResult) || eventReadResult.Position >= endPosition)
                {
                    eventsChannel.Writer.TryComplete();
                }

                return Task.CompletedTask;
            },
            subscriptionDropped: (_, dropReason, ex) =>
            {
                if (dropReason != SubscriptionDroppedReason.Disposed)
                {
                    throw ex ?? new("Subscription dropped in the middle of reading");
                }
            },
            filterOptions: new(
                EventTypeFilter.RegularExpression(string.Join("|", eventDescriptorFullNames.Select(Regex.Escape))),
                checkpointReached: (_, pos, _) =>
                {
                    if (pos > endPosition)
                    {
                        eventsChannel.Writer.TryComplete();
                    }

                    return Task.CompletedTask;
                }));

        await foreach (var eventReadResult in eventsChannel.Reader.ReadAllAsync())
        {
            yield return eventReadResult;
        }
    }

    /// <inheritdoc cref="IEventReader.GetLatestEventNumber"/>
    public async Task<StreamPosition?> GetLatestEventNumber(string stream, CancellationToken ct = default)
    {
        var eventRecord = await GetLatestEvent(stream, ct).ConfigureAwait(false);
        return eventRecord?.EventNumber;
    }

    /// <inheritdoc cref="IEventReader.GetLatestEventPosition"/>
    public async Task<Position?> GetLatestEventPosition(string stream, CancellationToken ct = default)
    {
        var eventRecord = await GetLatestEvent(stream, ct).ConfigureAwait(false);
        return eventRecord?.Position;
    }

    /// <inheritdoc cref="IEventReader.GetLatestEvent"/>
    public Task<EventRecord?> GetLatestEvent(string stream, CancellationToken ct = default)
    {
        return WellKnownStreams.All.Equals(stream, StringComparison.Ordinal)
            ? GetLatestEventFromAll(ct)
            : GetLatestEventFromStream(stream, ct);
    }

    private async Task<EventRecord?> GetLatestEventFromAll(CancellationToken ct = default)
    {
        var events = await _client
            .ReadAllAsync(Direction.Backwards, Position.End, 1, cancellationToken: ct)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return events.Count == 0
            ? null
            : events[0].Event;
    }

    private async Task<EventRecord?> GetLatestEventFromStream(string stream, CancellationToken ct = default)
    {
        try
        {
            var events = await _client
                .ReadStreamAsync(Direction.Backwards, stream, StreamPosition.End, 1, cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            return events.Count == 0
                ? null
                : events[0].Event;
        }
        catch (StreamNotFoundException)
        {
            return null;
        }
    }

    private bool TryDeserialize(ResolvedEvent resolvedEvent, Func<IMessage, IDescriptor>? metadataDescriptorProvider, [NotNullWhen(true)] out EventReadResult? eventReadResult)
    {
        if (!_serializer.TryDeserialize(resolvedEvent.Event, metadataDescriptorProvider, out var eventWithMetadata))
        {
            eventReadResult = null;
            return false;
        }

        eventReadResult = new EventReadResult(
            resolvedEvent.Event.EventId.ToGuid(),
            eventWithMetadata.Data,
            eventWithMetadata.Metadata,
            resolvedEvent.Event.Position,
            resolvedEvent.Event.Created,
            resolvedEvent.Event.EventStreamId);
        return true;
    }
}
