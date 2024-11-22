// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using EventStore.Client;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// A version of an event sourcing stream.
/// </summary>
public interface IEventSourcingAggregateVersion
{
    /// <summary>
    /// Gets the name of the stream.
    /// </summary>
    string StreamName { get; }

    /// <summary>
    /// Gets the version of the stream.
    /// Null if the stream has not yet been created.
    /// </summary>
    StreamRevision? Version { get; }
}
