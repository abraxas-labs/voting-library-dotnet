// (c) Copyright 2022 by Abraxas Informatik AG
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
    public async Task<IDisposable> AcquireAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        return new Disposable(() => _semaphore.Release());
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
