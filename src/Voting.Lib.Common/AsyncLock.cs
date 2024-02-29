// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common;

/// <summary>
/// A simple async lock implementation.
/// </summary>
public sealed class AsyncLock : IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Acquires the lock.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the async operation, resolving to a disposable which when disposed releases the lock.</returns>
    public Task<IDisposable> AcquireAsync(CancellationToken ct = default)
        => AcquireAsync(Timeout.InfiniteTimeSpan, ct);

    /// <summary>
    /// Acquires the lock.
    /// </summary>
    /// <param name="timeout">A <see cref="System.TimeSpan"/> that represents the time to wait.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the async operation, resolving to a disposable which when disposed releases the lock.</returns>
    public async Task<IDisposable> AcquireAsync(TimeSpan timeout, CancellationToken ct = default)
    {
        if (!await _semaphore.WaitAsync(timeout, ct))
        {
            throw new InvalidOperationException("Could not acquire lock");
        }

        return new Disposable(() => _semaphore.Release());
    }

    /// <summary>
    /// Tries to acquire the lock immediately.
    /// </summary>
    /// <param name="locker">The lock disposable (releases the lock as soon as the disposable is disposed).</param>
    /// <returns>A boolean value indicating whether the lock could be acquired.</returns>
    public bool TryAcquireImmediately(out IDisposable? locker)
    {
        if (!_semaphore.Wait(0))
        {
            locker = null;
            return false;
        }

        locker = new Disposable(() => _semaphore.Release());
        return true;
    }

    /// <summary>
    /// Waits for the lock to be available and disposes it.
    /// </summary>
    /// <returns>A task representing the async operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _semaphore.WaitAsync();
        _semaphore.Dispose();
    }
}
