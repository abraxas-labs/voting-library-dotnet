// (c) Copyright 2022 by Abraxas Informatik AG
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtobufJsonSerializer"/> class.
    /// </summary>
    /// <param name="formatter">The protobuf JSON formatter.</param>
    /// <param name="parser">The protobuf JSON parser.</param>
    /// <param name="typeRegistry">The protobuf type registry.</param>
    public ProtobufJsonSerializer(JsonFormatter formatter, JsonParser parser, IProtobufTypeRegistry typeRegistry)
    {
        _formatter = formatter;
        _parser = parser;
        _typeRegistry = typeRegistry;
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
    public bool TryDeserialize(EventRecord eventRecord, IDescriptor? metadataDescriptor, [NotNullWhen(true)] out EventWithMetadata? eventWithMetadata)
    {
        if (!TryDeserialize(eventRecord.Data, eventRecord.EventType, out var data))
        {
            eventWithMetadata = default;
            return false;
        }

        IMessage? metadata = null;
        if (metadataDescriptor != null && eventRecord.Metadata.Length > 0 && !TryDeserialize(eventRecord.Metadata, metadataDescriptor.FullName, out metadata))
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
        => Deserialize(eventRecord.Data, eventRecord.EventType);

    /// <inheritdoc />
    public T Deserialize<T>(EventRecord eventRecord)
        where T : IMessage<T>, new()
    {
        var json = Encoding.UTF8.GetString(eventRecord.Data.ToArray());
        return _parser.Parse<T>(json);
    }

    /// <inheritdoc />
    public IMessage Deserialize(ReadOnlyMemory<byte> data, string messageType)
    {
        return TryDeserialize(data, messageType, out var message)
            ? message
            : throw new InvalidOperationException("could not find protobuf type for " + messageType);
    }

    private bool TryDeserialize(ReadOnlyMemory<byte> data, string messageType, [NotNullWhen(true)] out IMessage? message)
    {
        var messageDescriptor = _typeRegistry.Find(messageType);
        if (messageDescriptor == null)
        {
            message = default;
            return false;
        }

        var json = Encoding.UTF8.GetString(data.ToArray());
        message = _parser.Parse(json, messageDescriptor);
        return true;
    }
}
