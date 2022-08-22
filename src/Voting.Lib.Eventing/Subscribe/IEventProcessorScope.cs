// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using EventStore.Client;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Interface for an event processing scope.
/// </summary>
public interface IEventProcessorScope
{
    /// <summary>
    /// Begin the processing of an event.
    /// </summary>
    /// <param name="position">The position of the event which will be processed.</param>
    /// <param name="streamPosition">The position of the event in the stream.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Begin(Position position, StreamPosition streamPosition);

    /// <summary>
    /// Completed processing an event. Only called when the event processing was successful.
    /// </summary>
    /// <param name="position">The position of the event which will be processed.</param>
    /// <param name="streamPosition">The position of the event in the stream.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Complete(Position position, StreamPosition streamPosition);

    /// <summary>
    /// Returns the position of the snapshot (last successfully processed event number) or null if no snapshot exists yet
    /// or the subscription should start from the very beginning.
    /// Subscriptions to <see cref="WellKnownStreams.All"/> use the <see cref="Position"/> to resubscribe,
    /// all others use the <see cref="StreamPosition"/>.
    /// </summary>
    /// <returns>A task which resolves to the last processed event position and number or null if none was processed yet.</returns>
    Task<(Position, StreamPosition)?> GetSnapshotPosition();
}
