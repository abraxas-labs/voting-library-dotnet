// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Common;

/// <summary>
/// A simple async lock implementation that serializes access per key.
/// Concurrent callers acquiring different keys do not block each other.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public sealed class KeyedAsyncLock<TKey> : IDisposable
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, Entry> _entries = new();

    /// <summary>
    /// Acquires the lock for the given key.
    /// </summary>
    /// <param name="key">The key to lock on.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the async operation, resolving to a disposable which when disposed releases the lock.</returns>
    public Task<IDisposable> AcquireAsync(TKey key, CancellationToken ct = default)
        => AcquireAsync(key, Timeout.InfiniteTimeSpan, ct);

    /// <summary>
    /// Acquires the lock for the given key.
    /// </summary>
    /// <param name="key">The key to lock on.</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the time to wait.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task representing the async operation, resolving to a disposable which when disposed releases the lock.</returns>
    public async Task<IDisposable> AcquireAsync(TKey key, TimeSpan timeout, CancellationToken ct = default)
    {
        var entry = Rent(key);
        bool acquired;
        try
        {
            acquired = await entry.Semaphore.WaitAsync(timeout, ct).ConfigureAwait(false);
        }
        catch
        {
            Return(key, entry);
            throw;
        }

        if (!acquired)
        {
            Return(key, entry);
            throw new InvalidOperationException("Could not acquire lock");
        }

        return new Disposable(() => Release(key, entry));
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var entry in _entries.Values)
        {
            entry.Dispose();
        }

        _entries.Clear();
    }

    private Entry Rent(TKey key)
    {
        while (true)
        {
            var entry = _entries.GetOrAdd(key, static _ => new Entry());
            lock (entry)
            {
                // Another thread may have just decremented the refcount to zero and removed the entry from
                // the dictionary in the small window between GetOrAdd and acquiring the entry lock.
                // In that case the semaphore has been disposed and we must retry to obtain a new entry.
                if (entry.Disposed)
                {
                    continue;
                }

                entry.RefCount++;
                return entry;
            }
        }
    }

    private void Return(TKey key, Entry entry)
    {
        lock (entry)
        {
            DecrementAndRemoveIfUnused(key, entry);
        }
    }

    private void Release(TKey key, Entry entry)
    {
        lock (entry)
        {
            entry.Semaphore.Release();
            DecrementAndRemoveIfUnused(key, entry);
        }
    }

    private void DecrementAndRemoveIfUnused(TKey key, Entry entry)
    {
        if (--entry.RefCount != 0)
        {
            return;
        }

        if (_entries.TryRemove(KeyValuePair.Create(key, entry)))
        {
            // SemaphoreSlim.WaitAsync has a documented race where the semaphore may be
            // internally acquired (count decremented) before a CancellationToken exception
            // propagates. In that case the catch block calls Return instead of Release,
            // so Semaphore.Release is never invoked. At RefCount == 0 no other thread
            // holds a reference to this entry, which means CurrentCount == 0 can only
            // be caused by that race. Release it here to ensure a consistent state
            // before disposal.
            if (entry.Semaphore.CurrentCount == 0)
            {
                entry.Semaphore.Release();
            }

            entry.Dispose();
        }
    }

    private sealed class Entry : IDisposable
    {
        public SemaphoreSlim Semaphore { get; } = new(1, 1);

        public int RefCount { get; set; }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
            Semaphore.Dispose();
        }
    }
}
