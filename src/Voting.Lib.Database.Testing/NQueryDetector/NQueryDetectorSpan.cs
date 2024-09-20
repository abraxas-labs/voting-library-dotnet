// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Voting.Lib.Database.Testing.NQueryDetector;

/// <summary>
/// Creates an N query detector span that spans multiple queries.
/// </summary>
public sealed class NQueryDetectorSpan : IDisposable
{
    private readonly int _maxCount;
    private readonly Action<NQueryDetectorSpan> _onDispose;
    private readonly ConcurrentDictionary<string, int> _counter = new();

    internal NQueryDetectorSpan(int maxCount, Action<NQueryDetectorSpan> onDispose)
    {
        _maxCount = maxCount;
        _onDispose = onDispose;
    }

    /// <summary>
    /// Gets the count of each recorded query.
    /// </summary>
    public IReadOnlyDictionary<string, int> QueryCounters => _counter;

    internal Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public void Dispose() => _onDispose(this);

    internal void Increment(string query)
    {
        var count = _counter.AddOrUpdate(query, _ => 1, (_, x) => x + 1);
        if (count > _maxCount)
        {
            throw new QueryRunTooManyTimesException(query, _maxCount, count);
        }
    }
}
