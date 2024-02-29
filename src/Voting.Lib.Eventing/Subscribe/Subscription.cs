// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using EventStore.Client;

namespace Voting.Lib.Eventing.Subscribe;

/// <inheritdoc />
public class Subscription<TScope> : ISubscription
    where TScope : IEventProcessorScope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Subscription{TScope}"/> class.
    /// </summary>
    /// <param name="eventTypes">The event types for this subscription.</param>
    /// <param name="streamName">The stream name of this subscription.</param>
    public Subscription(IReadOnlyCollection<Type> eventTypes, string streamName)
    {
        EventTypes = eventTypes;
        StreamName = streamName;
        IsAllStream = WellKnownStreams.All.Equals(streamName, StringComparison.Ordinal);
        ScopeName = typeof(TScope).FullName ?? "<unknown>";
    }

    /// <inheritdoc cref="ISubscription.ScopeName"/>
    public string ScopeName { get; }

    /// <summary>
    /// Gets a value indicating whether this subscription targets the <see cref="WellKnownStreams.All"/> stream.
    /// </summary>
    public bool IsAllStream { get; }

    /// <inheritdoc cref="ISubscription.StreamName"/>
    public string StreamName { get; }

    /// <summary>
    /// Gets a collection of the event clr types of all registered <see cref="ICatchUpDetectorEventProcessor{TScope,TEvent}"/>.
    /// These names are used to filter events the subscription subscribes to.
    /// This is only supported if subscribing to <see cref="WellKnownStreams.All"/>.
    /// </summary>
    public IReadOnlyCollection<Type> EventTypes { get; }

    /// <summary>
    /// Gets the current position.
    /// Only has a valid value if <see cref="IsAllStream"/> is <c>true</c>.
    /// <c>null</c> if the subscription has not processed any event yet.
    /// https://discuss.eventstore.com/t/event-position-18446744073709551615/3172/3.
    /// </summary>
    public Position? CurrentPosition { get; internal set; }

    /// <summary>
    /// Gets the current event number in the stream.
    /// This is not a meaningful value when <see cref="StreamName"/> is <see cref="WellKnownStreams.All"/>.
    /// <c>null</c> if the subscription has not processed any event yet.
    /// </summary>
    public StreamPosition? CurrentEventNumber { get; internal set; }

    /// <summary>
    /// Gets the position when the catch up ends.
    /// Before the subscription connects, this is set to the latest event position written to the EventStore.
    /// </summary>
    public Position CatchUpEndsPosition { get; internal set; }

    /// <summary>
    /// Gets the event number when the catch up ends.
    /// Before the subscription connects, this is set to the latest event number written to the EventStore.
    /// </summary>
    public StreamPosition CatchUpEndsEventNumber { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the subscription is in catch up or not. This remains true until the event at <see cref="CatchUpEndsPosition"/> is processed.
    /// </summary>
    public bool IsCatchUp { get; internal set; } = true;
}
