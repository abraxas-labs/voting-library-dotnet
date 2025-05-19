// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EventStore.Client;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Protobuf;

/// <summary>
/// Serializes protobuf event data to JSON.
/// </summary>
public class ProtobufJsonSerializer : IEventSerializer
{
    private readonly JsonFormatter _formatter;
    private readonly JsonParser _parser;
    private readonly IProtobufTypeRegistry _typeRegistry;
    private readonly IMetadataDescriptorProvider? _metadataDescriptorProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtobufJsonSerializer"/> class.
    /// </summary>
    /// <param name="formatter">The protobuf JSON formatter.</param>
    /// <param name="parser">The protobuf JSON parser.</param>
    /// <param name="typeRegistry">The protobuf type registry.</param>
    /// <param name="metadataDescriptorProvider">The metadata descriptor provider.</param>
    public ProtobufJsonSerializer(JsonFormatter formatter, JsonParser parser, IProtobufTypeRegistry typeRegistry, IMetadataDescriptorProvider? metadataDescriptorProvider = null)
    {
        _formatter = formatter;
        _parser = parser;
        _typeRegistry = typeRegistry;
        _metadataDescriptorProvider = metadataDescriptorProvider;
    }

    /// <inheritdoc />
    // allocations can be reduced easily as soon as https://github.com/dotnet/runtime/issues/58216 is implemented.
    public ReadOnlyMemory<byte> Serialize(IMessage eventData)
    {
        var writer = new ArrayBufferWriter<byte>();
        var s = _formatter.Format(eventData);
        Encoding.UTF8.GetBytes(s, writer);
        return writer.WrittenMemory;
    }

    /// <inheritdoc />
    public bool TryDeserialize(EventRecord eventRecord, [NotNullWhen(true)] out EventWithMetadata? eventWithMetadata)
    {
        if (!TryDeserialize(eventRecord.Data, eventRecord.EventType, out var data))
        {
            eventWithMetadata = default;
            return false;
        }

        IMessage? metadata = null;
        var metadataDescriptor = _metadataDescriptorProvider?.GetMetadataDescriptor(data);
        if (metadataDescriptor != null
            && eventRecord.Metadata.Length > 0
            && !TryDeserialize(eventRecord.Metadata, metadataDescriptor, out metadata))
        {
            eventWithMetadata = default;
            return false;
        }

        eventWithMetadata = new EventWithMetadata(
            data,
            metadata,
            eventRecord.EventId.ToGuid());
        return true;
    }

    /// <inheritdoc />
    public IMessage Deserialize(EventRecord eventRecord)
    {
        if (!TryDeserialize(eventRecord.Data, eventRecord.EventType, out var data))
        {
            throw new InvalidOperationException($"Could not deserialize event with metadata for event {eventRecord.EventId} of type {eventRecord.EventType}");
        }

        return data;
    }

    /// <inheritdoc />
    public EventWithMetadata DeserializeWithMetadata(EventRecord eventRecord)
    {
        if (!TryDeserialize(eventRecord, out var eventWithMetadata))
        {
            throw new InvalidOperationException($"Could not deserialize event with metadata for event {eventRecord.EventId} of type {eventRecord.EventType}");
        }

        return eventWithMetadata;
    }

    private bool TryDeserialize(ReadOnlyMemory<byte> data, string messageType, [NotNullWhen(true)] out IMessage? message)
    {
        var messageDescriptor = _typeRegistry.Find(messageType);
        if (messageDescriptor == null)
        {
            message = default;
            return false;
        }

        return TryDeserialize(data, messageDescriptor, out message);
    }

    private bool TryDeserialize(ReadOnlyMemory<byte> data, MessageDescriptor messageDescriptor, [NotNullWhen(true)] out IMessage? message)
    {
        var json = Encoding.UTF8.GetString(data.Span);
        message = _parser.Parse(json, messageDescriptor);
        return true;
    }
}
