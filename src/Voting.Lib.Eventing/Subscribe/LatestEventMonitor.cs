// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.Extensions.Logging;
using Voting.Lib.Eventing.Diagnostics;
using Voting.Lib.Eventing.Read;
using Voting.Lib.Scheduler;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Reports the latest event metadata in a given interval.
/// </summary>
public class LatestEventMonitor : IScheduledJob
{
    private readonly IEventReader _reader;
    private readonly ILogger<LatestEventMonitor> _logger;
    private readonly Dictionary<string, HashSet<string>> _scopeNamesByStringName;

    /// <summary>
    /// Initializes a new instance of the <see cref="LatestEventMonitor"/> class.
    /// </summary>
    /// <param name="reader">The EventStore reader.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="subscriptions">The list of subscriptions.</param>
    public LatestEventMonitor(ILogger<LatestEventMonitor> logger, IEnumerable<ISubscription> subscriptions, IEventReader reader)
    {
        _logger = logger;
        _reader = reader;
        _scopeNamesByStringName = subscriptions
            .GroupBy(x => x.StreamName)
            .ToDictionary(x => x.Key, x => x.Select(y => y.ScopeName).ToHashSet());
    }

    /// <summary>
    /// Runs this job and fetches the latest event position for each subscription.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Run(CancellationToken ct)
    {
        foreach (var (stream, scopeNames) in _scopeNamesByStringName)
        {
            _logger.LogDebug("Fetching latest event position for {Stream}", stream);
            var latestEvent = await _reader.GetLatestEvent(stream, ct).ConfigureAwait(false);
            EventingMeter.LatestEvent(scopeNames, latestEvent);
            _logger.LogDebug(
                "Latest event position fetched {Position} / {Number} for {Stream}",
                latestEvent?.Position ?? Position.Start,
                latestEvent?.EventNumber ?? StreamPosition.Start,
                stream);
        }
    }
}
