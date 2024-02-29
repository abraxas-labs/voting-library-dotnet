// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Interface for when a transient EventProcessor catches up (processed all "historic" events).
/// </summary>
public interface ITransientEventProcessorCatchUpCompleter : IEventProcessorCatchUpCompleter<TransientEventProcessorScope>
{
}
