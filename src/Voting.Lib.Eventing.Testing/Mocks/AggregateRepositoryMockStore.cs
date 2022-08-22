// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// This is the in-memory mock store of the events which is registered as singleton.
/// </summary>
public class AggregateRepositoryMockStore
{
    private readonly Dictionary<Guid, List<IDomainEvent>> _eventPerAggregate = new();

    /// <summary>
    /// Clear all events.
    /// </summary>
    public void Clear() => _eventPerAggregate.Clear();

    /// <summary>
    /// Try to get published events for an aggregate.
    /// </summary>
    /// <param name="aggregateId">The aggregate ID.</param>
    /// <param name="events">The published events or null if no events have been published.</param>
    /// <returns>Returns true if events for the aggregate ID exist.</returns>
    public bool TryGetEvents(Guid aggregateId, [MaybeNullWhen(false)] out IEnumerable<IDomainEvent> events)
    {
        if (_eventPerAggregate.TryGetValue(aggregateId, out var domainEvents))
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
    /// <param name="aggregateId">The aggregate ID.</param>
    /// <param name="ev">The event to add.</param>
    public void AddEvent(Guid aggregateId, IDomainEvent ev)
    {
        if (_eventPerAggregate.TryGetValue(aggregateId, out var l))
        {
            l.Add(ev);
            return;
        }

        _eventPerAggregate[aggregateId] = new List<IDomainEvent> { ev };
    }
}
