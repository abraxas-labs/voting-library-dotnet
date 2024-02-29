// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Reader to read all events from an aggregate stream.
/// </summary>
public interface IAggregateEventReader
{
    /// <summary>
    /// Read all aggregate events from a stream.
    /// </summary>
    /// <param name="stream">The name of the stream to read.</param>
    /// <param name="aggregateId">The id of the aggregate to read.</param>
    /// <param name="endTimestampInclusive">Reads from beginning to this timestamp (created timestamp).</param>
    /// <exception cref="Voting.Lib.Eventing.Exceptions.AggregateNotFoundException">Throws if the stream/aggregate can't be found.</exception>
    /// <returns>All events in the specified stream.</returns>
    // TODO: change datetime to position with https://jira.abraxas-tools.ch/jira/browse/VOTING-1856.
    IAsyncEnumerable<IDomainEvent> ReadEvents(string stream, Guid aggregateId, DateTime? endTimestampInclusive = null);
}
