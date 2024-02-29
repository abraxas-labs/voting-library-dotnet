// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using EventStore.Client;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Interface for the event processing retry policy.
/// </summary>
/// <typeparam name="TScope">The type of event processing scope.</typeparam>
public interface IEventProcessingRetryPolicy<TScope>
    where TScope : IEventProcessorScope
{
    /// <summary>
    /// Gets the count of consecutive failed event processing attempts.
    /// </summary>
    int FailureCount { get; }

    /// <summary>
    /// Gets called each time the subscription has successfully processed an event.
    /// </summary>
    void Succeeded();

    /// <summary>
    /// Gets called when the subscription failed, and decides how the subscription should continue.
    /// </summary>
    /// <param name="reason">The reason why the subscription was dropped.</param>
    /// <returns>A boolean indicating, whether the subscription should reconnect.</returns>
    Task<bool> Failed(SubscriptionDroppedReason reason);
}
