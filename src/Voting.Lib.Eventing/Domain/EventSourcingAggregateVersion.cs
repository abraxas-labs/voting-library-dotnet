// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using EventStore.Client;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// A version of an event sourcing stream.
/// </summary>
/// <param name="StreamName">The name of the stream.</param>
/// <param name="Version">The version of the stream, null if the stream does not yet exist.</param>
public record EventSourcingAggregateVersion(string StreamName, StreamRevision? Version) : IEventSourcingAggregateVersion;
