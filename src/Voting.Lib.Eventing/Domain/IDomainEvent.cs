// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using EventStore.Client;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// Represents an event including domain related data, such as the aggregate ID.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the event identifier.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the identifier of the aggregate which has generated the event.
    /// </summary>
    Guid AggregateId { get; }

    /// <summary>
    /// Gets the actual event data.
    /// </summary>
    IMessage Data { get; }

    /// <summary>
    /// Gets the event metadata.
    /// </summary>
    IMessage? Metadata { get; }

    /// <summary>
    /// Gets the version of the aggregate when the event has been generated.
    /// </summary>
    StreamRevision AggregateVersion { get; }

    /// <summary>
    /// Gets the created timestamp of the event. This will not be applied to the event store.
    /// </summary>
    DateTime? Created { get; }

    /// <summary>
    /// Gets the position of the event in the global stream. This will not be applied to the event store.
    /// </summary>
    Position? Position { get; }
}
