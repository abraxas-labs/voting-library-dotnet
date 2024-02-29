// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// An processor for an event type.
/// If catch up information is required, implement <see cref="ICatchUpDetectorEventProcessor{TScope,TEvent}"/> instead.
/// A class may implement <see cref="IEventProcessor{TScope,TEvent}"/> for multiple event types.
/// </summary>
/// <typeparam name="TScope">The scope of the event processing.</typeparam>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IEventProcessor<TScope, TEvent> : ICatchUpDetectorEventProcessor<TScope, TEvent>
    where TScope : IEventProcessorScope
    where TEvent : IMessage<TEvent>
{
    Task IInternalEventProcessor<TEvent>.Process(TEvent eventData, bool isCatchUp)
        => Process(eventData);

    /// <summary>
    /// Processes the event.
    /// </summary>
    /// <param name="eventData">Protobuf message.</param>
    /// <returns>A Task.</returns>
    Task Process(TEvent eventData);
}
