// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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
}
