// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// An internal interface for an event processor.
/// Implementors should implement <see cref="ICatchUpDetectorEventProcessor{TScope,TEvent}"/>
/// or <see cref="IEventProcessor{TScope,TEvent}"/> instead.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IInternalEventProcessor<TEvent>
    where TEvent : IMessage<TEvent>
{
    /// <summary>
    /// Processes the event.
    /// </summary>
    /// <param name="eventData">Protobuf message.</param>
    /// <param name="isCatchUp">Describes whether the incoming event data is received in catch up mode or not.</param>
    /// <returns>A Task.</returns>
    Task Process(TEvent eventData, bool isCatchUp);
}
