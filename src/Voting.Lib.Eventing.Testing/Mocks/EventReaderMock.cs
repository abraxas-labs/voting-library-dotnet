﻿// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Voting.Lib.Eventing.Read;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// Mock event reader that reads from <see cref="EventReaderMockStore"/>.
/// </summary>
public class EventReaderMock : IEventReader
{
    private readonly EventReaderMockStore _store;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventReaderMock"/> class.
    /// </summary>
    /// <param name="store">The event reader mock store to read from.</param>
    public EventReaderMock(EventReaderMockStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<EventReadResult> ReadEvents(string stream, bool ignoreUnknownEvents = true)
    {
        if (!_store.TryGetEvents(stream, out var events))
        {
            yield break;
        }

        foreach (var ev in await Task.FromResult(events))
        {
            yield return MapToEventReadResult((stream, ev));
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<EventReadResult> ReadEventsFromAll(Position startPositionExclusive, Func<EventReadResult, bool> endCondition, bool ignoreUnknownEvents = true)
    {
        foreach (var ev in await Task.FromResult(_store.GetEvents(startPositionExclusive)))
        {
            var eventReadResult = MapToEventReadResult(ev);

            yield return eventReadResult;

            if (endCondition(eventReadResult))
            {
                yield break;
            }
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<EventReadResult> ReadEventsFromAll(
        Position startPositionExclusive,
        IReadOnlyCollection<Type> eventTypes,
        Func<EventReadResult, bool> endCondition)
    {
        foreach (var ev in await Task.FromResult(_store.GetEvents(startPositionExclusive)))
        {
            var eventReadResult = MapToEventReadResult(ev);

            if (!eventTypes.Contains(eventReadResult.Data.GetType()))
            {
                continue;
            }

            yield return eventReadResult;

            if (endCondition(eventReadResult))
            {
                yield break;
            }
        }
    }

    /// <inheritdoc cref="IEventReader.GetLatestEvent"/>
    public Task<EventRecord?> GetLatestEvent(string stream, CancellationToken ct = default)
    {
        if (!_store.TryGetEvents(stream, out var events) || events.LastOrDefault() is not { } eventData)
        {
            return Task.FromResult<EventRecord?>(null);
        }

        var eventRecord = new EventRecord(
            stream,
            Uuid.Empty,
            eventData.EventNumber,
            eventData.Position,
            new Dictionary<string, string>
            {
                ["type"] = eventData.Event.Data.Descriptor.FullName,
                ["created"] = ((DateTimeOffset)eventData.Created).ToUnixTimeSeconds().ToString(),
                ["content-type"] = "application/json",
            },
            Encoding.UTF8.GetBytes(JsonFormatter.Default.Format(eventData.Event.Data)).AsMemory(),
            eventData.Event.Metadata == null ? null : Encoding.UTF8.GetBytes(JsonFormatter.Default.Format(eventData.Event.Metadata)).AsMemory());
        return Task.FromResult<EventRecord?>(eventRecord);
    }

    /// <inheritdoc cref="IEventReader.GetLatestEventNumber"/>
    public async Task<StreamPosition?> GetLatestEventNumber(string stream, CancellationToken ct = default)
        => (await GetLatestEvent(stream, ct))?.EventNumber;

    /// <inheritdoc cref="IEventReader.GetLatestEventPosition"/>
    public async Task<Position?> GetLatestEventPosition(string stream, CancellationToken ct = default)
        => (await GetLatestEvent(stream, ct))?.Position;

    private EventReadResult MapToEventReadResult((string Stream, EventReaderMockStoreData StoreData) data)
    {
        var metadata = data.StoreData.Event.Metadata;
        return new EventReadResult(
            data.StoreData.Event.Id,
            data.StoreData.Event.Data,
            metadata,
            data.StoreData.Position,
            data.StoreData.Created,
            data.Stream);
    }
}
