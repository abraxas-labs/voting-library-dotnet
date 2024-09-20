// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// This is the in-memory mock store of the events.
/// </summary>
public class AggregateRepositoryMockStore
{
    // Multiple aggregates may have the same ID, we cannot simply store the aggregate ID
    // Need to store the stream ID like in the real implementation.
    private readonly Dictionary<string, List<IDomainEvent>> _eventPerAggregate = new();

    /// <summary>
    /// Clear all events.
    /// </summary>
    public void Clear() => _eventPerAggregate.Clear();

    /// <summary>
    /// Try to get published events for an aggregate.
    /// </summary>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate ID.</param>
    /// <param name="events">The published events or null if no events have been published.</param>
    /// <returns>Returns true if events for the aggregate ID exist.</returns>
    public bool TryGetEvents(string aggregateName, Guid aggregateId, [MaybeNullWhen(false)] out IEnumerable<IDomainEvent> events)
    {
        var streamName = BuildStreamName(aggregateName, aggregateId);
        if (_eventPerAggregate.TryGetValue(streamName, out var domainEvents))
        {
            events = domainEvents;
            return true;
        }

        events = default;
        return false;
    }

    /// <summary>
    /// Add an event for a specific aggregate to the mock store.
    /// </summary>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate ID.</param>
    /// <param name="ev">The event to add.</param>
    public void AddEvent(string aggregateName, Guid aggregateId, IDomainEvent ev)
    {
        var streamName = BuildStreamName(aggregateName, aggregateId);
        if (_eventPerAggregate.TryGetValue(streamName, out var l))
        {
            l.Add(ev);
            return;
        }

        _eventPerAggregate[streamName] = new List<IDomainEvent> { ev };
    }

    private string BuildStreamName(string aggregateName, Guid aggregateId)
        => $"{aggregateName}-{aggregateId}";
}
