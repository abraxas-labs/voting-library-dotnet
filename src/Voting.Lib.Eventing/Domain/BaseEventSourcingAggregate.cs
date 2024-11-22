// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using EventStore.Client;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// Base class for event sourced aggregates.
/// </summary>
public abstract class BaseEventSourcingAggregate : IEventSourcingAggregateVersion
{
    private readonly LinkedList<IDomainEvent> _uncommittedEvents = new();

    /// <summary>
    /// Gets or sets the aggregate id.
    /// </summary>
    public Guid Id { get; protected internal set; }

    /// <summary>
    /// Gets the name of the eventstore stream.
    /// </summary>
    public virtual string StreamName => $"{AggregateName}-{Id}";

    /// <summary>
    /// Gets the current aggregate version.
    /// Null if no event has been applied yet.
    /// The first saved aggregate version begins with zero.
    /// </summary>
    public StreamRevision? Version { get; private set; }

    /// <summary>
    /// Gets the original aggregate version (the version at the time the aggregate was created, read or saved).
    /// This should always be the current stream revision value of the stream in eventstore.
    /// Null if this is a new and unsaved aggregate.
    /// The first saved aggregate version begins with zero.
    /// <remarks>
    /// For example:
    /// This aggregate is created. The <see cref="Version"/> and the <see cref="OriginalVersion"/> are both <c>null</c>.
    /// An event is raised. The <see cref="Version"/> is set to 0. The <see cref="OriginalVersion"/> keeps the value <c>null</c>.
    /// The aggregate is saved. The <see cref="OriginalVersion"/> value gets overwritten by the value of <see cref="Version"/> and has the value 0 now.
    /// Later this aggregate is loaded with one event. The <see cref="Version"/> and the <see cref="OriginalVersion"/> are both 0.
    /// An event is raised on this aggregate. The <see cref="Version"/> increases to 1. The <see cref="OriginalVersion"/> keeps the value 0.
    /// The aggregate is saved. The <see cref="Version"/> keeps the value 1. The <see cref="OriginalVersion"/> gets the value of <see cref="Version"/> and updates to 1.
    /// </remarks>
    /// </summary>
    public StreamRevision? OriginalVersion { get; internal set; }

    /// <summary>
    /// Gets the name of this aggregate.
    /// Should start with 'voting-'.
    /// </summary>
    public abstract string AggregateName { get; }

    /// <summary>
    /// Applies an event to this aggregate.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    public void ApplyEvent(IDomainEvent @event)
    {
        Apply(@event.Data);
        Version = @event.AggregateVersion;
    }

    /// <summary>
    /// Clears all uncommited events.
    /// </summary>
    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    /// <summary>
    /// Gets all uncommited events.
    /// </summary>
    /// <returns>The uncommited events.</returns>
    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _uncommittedEvents;

    /// <summary>
    /// Raises an event. This stores the event in the uncommited events and immediately applies it to this aggregate.
    /// </summary>
    /// <param name="eventData">The event data of the event.</param>
    /// <param name="eventMetadata">The event metadata of the event.</param>
    /// <param name="eventId">The id of the event. If not provided it will generate a new one.</param>
    protected virtual void RaiseEvent(IMessage eventData, IMessage? eventMetadata = null, Guid? eventId = null)
    {
        var wrapper = new EventDataWrapper(
            eventId ?? Uuid.NewUuid().ToGuid(),
            Id,
            eventData,
            Version?.Next() ?? StreamRevision.FromStreamPosition(StreamPosition.Start),
            eventMetadata);

        ApplyEvent(wrapper);
        _uncommittedEvents.AddLast(wrapper);
    }

    /// <summary>
    /// Applies an event to this aggregate.
    /// </summary>
    /// <param name="eventData">The event data to apply.</param>
    protected abstract void Apply(IMessage eventData);
}
