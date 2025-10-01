// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using EventStore.Client;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Read;

/// <summary>
/// The result of reading an event.
/// </summary>
public class EventReadResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventReadResult"/> class.
    /// </summary>
    /// <param name="id">The event ID.</param>
    /// <param name="data">The event data.</param>
    /// <param name="metadata">The event metadata.</param>
    /// <param name="rawByteData">The event raw byte data (as written in the event store).</param>
    /// <param name="position">The event position.</param>
    /// <param name="created">The creation time of the event.</param>
    /// <param name="streamId">The event stream ID.</param>
    public EventReadResult(Guid id, IMessage data, IMessage? metadata, ReadOnlyMemory<byte> rawByteData, Position position, DateTime created, string streamId)
    {
        Id = id;
        Data = data;
        Metadata = metadata;
        RawByteData = rawByteData;
        Position = position;
        Created = created;
        StreamId = streamId;
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

    /// <summary>
    /// Gets the event raw byte data (as written in the event store).
    /// </summary>
    public ReadOnlyMemory<byte> RawByteData { get; }

    /// <summary>
    /// Gets the position in the $all stream.
    /// Has an invalid value when it is read from an other stream than $all stream.
    /// https://jira.abraxas-tools.ch/jira/browse/VOTING-1856.
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// Gets the event creation time.
    /// </summary>
    // TODO: this should be removed with https://jira.abraxas-tools.ch/jira/browse/VOTING-1856.
    public DateTime Created { get; }

    /// <summary>
    /// Gets the event stream ID.
    /// </summary>
    public string StreamId { get; }
}
