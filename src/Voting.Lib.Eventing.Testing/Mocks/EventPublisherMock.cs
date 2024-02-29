// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// This is a mock implementation of <see cref="IEventPublisher"/>.
/// </summary>
public class EventPublisherMock : IEventPublisher
{
    private readonly List<EventWithMetadata> _publishedEvents = new();

    /// <summary>
    /// Gets all published events since the last clear operation.
    /// </summary>
    public IEnumerable<EventWithMetadata> AllPublishedEvents => _publishedEvents;

    /// <summary>
    /// Clears the published events list.
    /// </summary>
    public void Clear() => _publishedEvents.Clear();

    /// <summary>
    /// Get all published events where the metadata is of the defined type.
    /// </summary>
    /// <typeparam name="T">The metadata type to search.</typeparam>
    /// <returns>All published events with metadata of the defined type.</returns>
    public IEnumerable<EventWithMetadata> GetPublishedEventsWithMetadata<T>()
        => AllPublishedEvents.Where(ev => ev.Data is T);

    /// <summary>
    /// Get a single published event where the metadata is of the defined type.
    /// </summary>
    /// <typeparam name="T">The metadata type to search.</typeparam>
    /// <returns>Returns a single published event where the metadata is of the defined type.</returns>
    public EventWithMetadata GetSinglePublishedEventWithMetadata<T>()
        => GetPublishedEventsWithMetadata<T>().Single();

    /// <summary>
    /// Fetches an enumerable of all published events of type T since the last <see cref="Clear"/> operation.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    /// <returns>An enumerable of the event type.</returns>
    public IEnumerable<T> GetPublishedEvents<T>()
        => AllPublishedEvents.Select(ev => ev.Data).OfType<T>();

    /// <summary>
    /// Get all published events filtered by the event data type and event metadata type.
    /// </summary>
    /// <typeparam name="TEventData">The event data type to filter.</typeparam>
    /// <typeparam name="TEventMetadata">The event metadata type to filter.</typeparam>
    /// <returns>Returns all published events that match the type filter.</returns>
    public IEnumerable<(TEventData Data, TEventMetadata? Metadata)> GetPublishedEvents<TEventData, TEventMetadata>()
        where TEventData : class
        where TEventMetadata : class
        => AllPublishedEvents
            .Where(ev => ev.Data is TEventData && ev.Metadata is TEventMetadata)
            .Select(ev => ((TEventData)ev.Data, (TEventMetadata?)ev.Metadata));

    /// <summary>
    /// Fetches a single published event of type T since the last <see cref="Clear"/> operation.
    /// Throws if none or multiple matching events are found.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    /// <returns>The published event.</returns>
    public T GetSinglePublishedEvent<T>()
        => GetPublishedEvents<T>().Single();

    /// <summary>
    /// Get a single published event filtered by the event data type and event metadata type.
    /// </summary>
    /// <typeparam name="TEventData">The event data type to filter.</typeparam>
    /// <typeparam name="TEventMetadata">The event metadata type to filter.</typeparam>
    /// <returns>Returns a single published event that matches the type filter.</returns>
    public (TEventData Data, TEventMetadata? Metadata) GetSinglePublishedEvent<TEventData, TEventMetadata>()
        where TEventData : class
        where TEventMetadata : class
        => GetPublishedEvents<TEventData, TEventMetadata>().Single();

    /// <summary>
    /// Publishes an event to the in-memory mock store.
    /// </summary>
    /// <param name="stream">The stream name (ignored).</param>
    /// <param name="event">The event to publish.</param>
    /// <param name="expectedRevision">The expected revision (ignored).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(string stream, EventWithMetadata @event, StreamRevision? expectedRevision)
        => SaveEvents(new[] { @event });

    /// <summary>
    /// Publishes events to the in-memory mock store.
    /// </summary>
    /// <param name="stream">The stream name (ignored).</param>
    /// <param name="events">The events to publish.</param>
    /// <param name="expectedRevision">The expected revision (ignored).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Publish(string stream, IEnumerable<EventWithMetadata> events, StreamRevision? expectedRevision)
        => SaveEvents(events);

    /// <summary>
    /// Publishes an event to the in-memory mock store.
    /// </summary>
    /// <param name="stream">The stream name (ignored).</param>
    /// <param name="event">The event to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task PublishWithoutIdempotencyGuarantee(string stream, EventWithMetadata @event)
        => SaveEvents(new[] { @event });

    /// <summary>
    /// Publishes events to the in-memory mock store.
    /// </summary>
    /// <param name="stream">The stream name (ignored).</param>
    /// <param name="events">The events to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task PublishWithoutIdempotencyGuarantee(string stream, IEnumerable<EventWithMetadata> events)
        => SaveEvents(events);

    private Task SaveEvents(IEnumerable<EventWithMetadata> events)
    {
        _publishedEvents.AddRange(events);
        return Task.CompletedTask;
    }
}
