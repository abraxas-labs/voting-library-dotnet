// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// A transient event processor that receives and handles events.
/// Event handler implementation may differ depending whether the event is processed in catch up mode,
/// otherwise implement <see cref="ITransientEventProcessor{TEvent}"/> instead.
/// </summary>
/// <typeparam name="TEvent">Type of the Event.</typeparam>
public interface ITransientCatchUpDetectorEventProcessor<TEvent> :
    ICatchUpDetectorEventProcessor<TransientEventProcessorScope, TEvent>
    where TEvent : IMessage<TEvent>
{
}
