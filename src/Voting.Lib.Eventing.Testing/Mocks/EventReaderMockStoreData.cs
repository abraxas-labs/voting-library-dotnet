// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using EventStore.Client;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// A mock wrapper for storing event data.
/// </summary>
public class EventReaderMockStoreData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventReaderMockStoreData"/> class.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="position">The event position.</param>
    /// <param name="created">The creation time.</param>
    /// <param name="eventNumber">The event number.</param>
    public EventReaderMockStoreData(EventWithMetadata @event, Position position, StreamPosition eventNumber, DateTime created)
    {
        Event = @event;
        Position = position;
        EventNumber = eventNumber;
        Created = created;
    }

    /// <summary>
    /// Gets the event.
    /// </summary>
    public EventWithMetadata Event { get; }

    /// <summary>
    /// Gets the event position.
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// Gets the event number.
    /// </summary>
    public StreamPosition EventNumber { get; }

    /// <summary>
    /// Gets the creation time.
    /// </summary>
    public DateTime Created { get; }
}
