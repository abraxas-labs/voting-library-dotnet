// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Interface for when an EventProcessor catches up (processed all "historic" events).
/// </summary>
/// <typeparam name="TScope">The type of the event processor scope.</typeparam>
public interface IEventProcessorCatchUpCompleter<TScope>
    where TScope : IEventProcessorScope
{
    /// <summary>
    /// Gets called once when the catch up mode is over and the last event of the catch up mode is processed.
    /// </summary>
    /// <returns>A task.</returns>
    Task CatchUpCompleted();
}
