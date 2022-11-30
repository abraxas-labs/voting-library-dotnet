// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Voting.Lib.Eventing.Read;

/// <summary>
/// Event reader to read events from a stream.
/// </summary>
public interface IEventReader
{
    /// <summary>
    /// Reads all events with metadata from a stream.
    /// </summary>
    /// <param name="stream">The stream name.</param>
    /// <param name="metadataDescriptorProvider">The meta data type descriptor provider by the event data.</param>
    /// <param name="ignoreUnknownEvents">If true, unknown events (events for which no deserializer is available) are ignored.</param>
    /// <returns>All events in the specified stream.</returns>
    IAsyncEnumerable<EventReadResult> ReadEvents(
        string stream,
        Func<IMessage, IDescriptor>? metadataDescriptorProvider = null,
        bool ignoreUnknownEvents = true);

    /// <summary>
    /// Reads all events with metadata from the global stream from a start position until all events are read or end condition is fulfilled.
    /// </summary>
    /// <param name="startPositionExclusive">The stream name.</param>
    /// <param name="endCondition">A condition which will end the read including the event for which the condition is met.</param>
    /// <param name="metadataDescriptorProvider">The meta data type descriptor provider by the event data.</param>
    /// <param name="ignoreUnknownEvents">If true, unknown events (events for which no deserializer is available) are ignored.</param>
    /// <returns>All events in the specified stream.</returns>
    IAsyncEnumerable<EventReadResult> ReadEventsFromAll(
        Position startPositionExclusive,
        Func<EventReadResult, bool> endCondition,
        Func<IMessage, IDescriptor>? metadataDescriptorProvider = null,
        bool ignoreUnknownEvents = true);

    /// <summary>
    /// Reads all events with metadata from the global stream from a start position filtered by event types until all events are read or end condition is fulfilled.
    /// </summary>
    /// <param name="startPositionExclusive">The stream name.</param>
    /// <param name="eventTypes">Event types to read.</param>
    /// <param name="endCondition">A condition which will end the read including the event for which the condition is met.</param>
    /// <param name="metadataDescriptorProvider">The meta data type descriptor provider by the event data.</param>
    /// <returns>All events in the specified stream.</returns>
    IAsyncEnumerable<EventReadResult> ReadEventsFromAll(
        Position startPositionExclusive,
        IReadOnlyCollection<Type> eventTypes,
        Func<EventReadResult, bool> endCondition,
        Func<IMessage, IDescriptor>? metadataDescriptorProvider = null);

    /// <summary>
    /// Fetches the latest event of a stream.
    /// </summary>
    /// <param name="stream">The name of the stream.</param>
    /// <param name="ct">An optional cancellation token.</param>
    /// <returns>A task resolving to the fetched event record or <c>null</c>.</returns>
    Task<EventRecord?> GetLatestEvent(string stream, CancellationToken ct = default);

    /// <summary>
    /// Fetches the number of the latest event of a stream.
    /// </summary>
    /// <param name="stream">The name of the stream.</param>
    /// <param name="ct">An optional cancellation token.</param>
    /// <returns>A task resolving to the number of the latest event record or <c>null</c> if no event was found.</returns>
    Task<StreamPosition?> GetLatestEventNumber(string stream, CancellationToken ct = default);

    /// <summary>
    /// Fetches the position of the latest event of a stream.
    /// </summary>
    /// <param name="stream">The name of the stream.</param>
    /// <param name="ct">An optional cancellation token.</param>
    /// <returns>A task resolving to the position of the latest event record or <c>null</c> if no event was found.</returns>
    Task<Position?> GetLatestEventPosition(string stream, CancellationToken ct = default);
}
