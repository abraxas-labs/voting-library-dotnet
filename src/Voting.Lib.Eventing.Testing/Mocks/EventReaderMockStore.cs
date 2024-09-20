// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EventStore.Client;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// A mock store for reading events.
/// </summary>
public class EventReaderMockStore
{
    private readonly Dictionary<string, List<EventReaderMockStoreData>> _eventPerStream = new();

    /// <summary>
    /// Clear all events.
    /// </summary>
    public void Clear() => _eventPerStream.Clear();

    /// <summary>
    /// Try to get events for a stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="events">The events or null if no events have been stored.</param>
    /// <returns>Returns true if events for the stream exist.</returns>
    public bool TryGetEvents(string stream, [MaybeNullWhen(false)] out IEnumerable<EventReaderMockStoreData> events)
    {
        if (_eventPerStream.TryGetValue(stream, out var existingEvents))
        {
            events = existingEvents;
            return true;
        }

        events = default;
        return false;
    }

    /// <summary>
    /// Get the stored events.
    /// </summary>
    /// <param name="startPosition">The position to start reading events.</param>
    /// <returns>All streams and events.</returns>
    public IEnumerable<(string Stream, EventReaderMockStoreData Data)> GetEvents(Position? startPosition = null)
    {
        return _eventPerStream
            .SelectMany(kvp => kvp.Value.Select(v => (kvp.Key, Data: v)))
            .Where(e => e.Data.Position >= startPosition)
            .OrderBy(e => e.Data.Position);
    }

    /// <summary>
    /// Add an event for a specific stream to the mock store.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="data">The event data to add.</param>
    public void AddEvent(string stream, EventReaderMockStoreData data)
    {
        if (_eventPerStream.TryGetValue(stream, out var l))
        {
            l.Add(data);
            return;
        }

        _eventPerStream[stream] = new List<EventReaderMockStoreData> { data };
    }
}
