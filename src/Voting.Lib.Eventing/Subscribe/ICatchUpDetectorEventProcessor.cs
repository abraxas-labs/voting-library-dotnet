// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// An event processor that receives and handles events.
/// Event handler implementation may differ depending whether the event is processed in catch up mode,
/// otherwise implement <see cref="IEventProcessor{TScope,TEvent}"/> instead.
/// </summary>
/// <typeparam name="TScope">The type of the scope of the event processors group.</typeparam>
/// <typeparam name="TEvent">Type of the Event.</typeparam>
public interface ICatchUpDetectorEventProcessor<TScope, TEvent>
    : IInternalEventProcessor<TEvent>
    where TScope : IEventProcessorScope
    where TEvent : IMessage<TEvent>
{
}
