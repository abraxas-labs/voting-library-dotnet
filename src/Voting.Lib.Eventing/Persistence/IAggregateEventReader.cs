// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Reader to read all events from an aggregate stream.
/// </summary>
public interface IAggregateEventReader
{
    /// <summary>
    /// Reads the version of an aggregate.
    /// </summary>
    /// <param name="stream">The name of the stream to read.</param>
    /// <param name="aggregateId">The id of the aggregate to read.</param>
    /// <returns>The stream revision or null if the stream/aggregate cannot be found.</returns>
    Task<StreamRevision?> TryGetVersion(string stream, Guid aggregateId);

    /// <summary>
    /// Read all aggregate events from a stream.
    /// </summary>
    /// <param name="stream">The name of the stream to read.</param>
    /// <param name="aggregateId">The id of the aggregate to read.</param>
    /// <param name="endTimestampInclusive">Reads from beginning to this timestamp (created timestamp).</param>
    /// <exception cref="Voting.Lib.Eventing.Exceptions.AggregateNotFoundException">Throws if the stream/aggregate can't be found.</exception>
    /// <returns>All events in the specified stream.</returns>
    // TODO: change datetime to position with ticket VOTING-1856.
    IAsyncEnumerable<IDomainEvent> ReadEvents(string stream, Guid aggregateId, DateTime? endTimestampInclusive = null);
}
