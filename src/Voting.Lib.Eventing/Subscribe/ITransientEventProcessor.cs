// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// An processor for an event type.
/// If catch up information is required, implement <see cref="ITransientCatchUpDetectorEventProcessor{TEvent}"/> instead.
/// A class may implement <see cref="ITransientEventProcessor{TEvent}"/> for multiple event types.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface ITransientEventProcessor<TEvent> : IEventProcessor<TransientEventProcessorScope, TEvent>
    where TEvent : IMessage<TEvent>
{
}
