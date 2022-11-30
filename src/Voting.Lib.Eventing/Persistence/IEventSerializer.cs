// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using EventStore.Client;
using Google.Protobuf;
using Google.Protobuf.Reflection;

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
    /// Deserialize event data.
    /// </summary>
    /// <param name="data">The event data to deserialize.</param>
    /// <param name="messageType">The message type.</param>
    /// <returns>Returns the deserialized event data.</returns>
    IMessage Deserialize(ReadOnlyMemory<byte> data, string messageType);

    /// <summary>
    /// Deserialize an event.
    /// </summary>
    /// <param name="eventRecord">The event to deserialize.</param>
    /// <typeparam name="T">The type of the deserialized event data.</typeparam>
    /// <returns>Returns the deserialized event data.</returns>
    T Deserialize<T>(EventRecord eventRecord)
        where T : IMessage<T>, new();

    /// <summary>
    /// Tries to deserialize an event.
    /// </summary>
    /// <param name="eventRecord">The event to deserialize.</param>
    /// <param name="metadataDescriptorProvider">The metadata descriptor provider by the event data.</param>
    /// <param name="eventWithMetadata">The deserialized event.</param>
    /// <returns>Returns true if the deserialization succeeded.</returns>
    bool TryDeserialize(EventRecord eventRecord, Func<IMessage, IDescriptor>? metadataDescriptorProvider, [NotNullWhen(true)] out EventWithMetadata? eventWithMetadata);
}
