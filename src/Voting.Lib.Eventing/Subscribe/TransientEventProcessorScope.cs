// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using EventStore.Client;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// A default transient implementation for <see cref="IEventProcessorScope"/>,
/// which keeps the state only in memory.
/// </summary>
public class TransientEventProcessorScope : IEventProcessorScope
{
    private Position? _position;
    private StreamPosition? _streamPosition;

    /// <inheritdoc />
    public Task Begin(Position position, StreamPosition streamPosition) => Task.CompletedTask;

    /// <inheritdoc />
    public Task Complete(Position position, StreamPosition streamPosition)
    {
        _position = position;
        _streamPosition = streamPosition;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<(Position, StreamPosition)?> GetSnapshotPosition()
    {
        return _position.HasValue && _streamPosition.HasValue
            ? Task.FromResult<(Position, StreamPosition)?>((_position.Value, _streamPosition.Value))
            : Task.FromResult<(Position, StreamPosition)?>(null);
    }
}
