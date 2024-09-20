// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using EventStore.Client;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Represents an event with its metadata.
/// </summary>
public class EventWithMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventWithMetadata"/> class.
    /// </summary>
    /// <param name="data">The event data.</param>
    /// <param name="metadata">The event metadata.</param>
    /// <param name="id">The optional ID. If not provided, a new ID will be generated.</param>
    public EventWithMetadata(IMessage data, IMessage? metadata = null, Guid? id = null)
    {
        Id = id ?? Uuid.NewUuid().ToGuid();
        Data = data;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the event ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the event data.
    /// </summary>
    public IMessage Data { get; }

    /// <summary>
    /// Gets the event metadata.
    /// </summary>
    public IMessage? Metadata { get; }
}
