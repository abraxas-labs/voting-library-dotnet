// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Exceptions;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Event publisher to publish events to a stream.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to publish the data to.</param>
    /// <param name="event">The event to publish.</param>
    /// <param name="expectedRevision">The expected revision of the stream. If null is provided, no stream should exist.</param>
    /// <exception cref="VersionMismatchException">When the expected revision is different from the actual revision.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Publish(string stream, EventWithMetadata @event, StreamRevision? expectedRevision);

    /// <summary>
    /// Publishes multiple events to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to publish the data to.</param>
    /// <param name="events">The events to publish.</param>
    /// <param name="expectedRevision">The expected revision of the stream. If <c>null</c> is provided, no stream should exist.</param>
    /// <exception cref="VersionMismatchException">When the expected revision is different from the actual revision.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Publish(string stream, IEnumerable<EventWithMetadata> events, StreamRevision? expectedRevision);

    /// <summary>
    /// Publishes an event to the specified stream. Does not guarantee idempotency, meaning concurrent events may be saved in any order.
    /// </summary>
    /// <param name="stream">The stream to publish the data to.</param>
    /// <param name="event">The event to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task PublishWithoutIdempotencyGuarantee(string stream, EventWithMetadata @event);

    /// <summary>
    /// Publishes multiple events to the specified stream. Does not guarantee idempotency, meaning concurrent events may be saved in any order.
    /// </summary>
    /// <param name="stream">The stream to publish the data to.</param>
    /// <param name="events">The events to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task PublishWithoutIdempotencyGuarantee(string stream, IEnumerable<EventWithMetadata> events);
}
