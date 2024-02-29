// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Runtime.Serialization;
using EventStore.Client;

namespace Voting.Lib.Eventing.Exceptions;

/// <summary>
/// Represents errors that are thrown when an unknown event is encountered.
/// </summary>
[Serializable]
public class UnknownEventException : Exception
{
    internal UnknownEventException(ResolvedEvent eventData)
        : base($"Encountered an unknown event of type {eventData.Event.EventType} on stream {eventData.OriginalStreamId} at position {eventData.OriginalPosition} with number {eventData.OriginalEventNumber}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownEventException"/> class.
    /// </summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="streamingContext">The streaming context.</param>
    protected UnknownEventException(SerializationInfo info, StreamingContext streamingContext)
        : base(info, streamingContext)
    {
    }
}
