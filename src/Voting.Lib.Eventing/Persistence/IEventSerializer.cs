// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using EventStore.Client;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Interface for serializing and deserializing event data.
/// </summary>
public interface IEventSerializer
{
    /// <summary>
    /// Gets the content type of this serializer.
    /// </summary>
    string ContentType => "application/json";

    /// <summary>
    /// Serialize the event data to a byte representation.
    /// </summary>
    /// <param name="eventData">The event data to serialize.</param>
    /// <returns>Returns the serialized data.</returns>
    ReadOnlyMemory<byte> Serialize(IMessage eventData);

    /// <summary>
    /// Deserialize an event.
    /// </summary>
    /// <param name="eventRecord">The event to deserialize.</param>
    /// <returns>Returns the deserialized event data.</returns>
    IMessage Deserialize(EventRecord eventRecord);

    /// <summary>
    /// Deserialize an event.
    /// </summary>
    /// <param name="eventRecord">The event to deserialize.</param>
    /// <returns>Returns the deserialized event data.</returns>
    EventWithMetadata DeserializeWithMetadata(EventRecord eventRecord);

    /// <summary>
    /// Tries to deserialize an event.
    /// </summary>
    /// <param name="eventRecord">The event to deserialize.</param>
    /// <param name="eventWithMetadata">The deserialized event.</param>
    /// <returns>Returns true if the deserialization succeeded.</returns>
    bool TryDeserialize(EventRecord eventRecord, [NotNullWhen(true)] out EventWithMetadata? eventWithMetadata);
}
