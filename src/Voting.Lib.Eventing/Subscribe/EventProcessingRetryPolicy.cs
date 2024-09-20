// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Class responsible for retrying the event processing in case of failures.
/// </summary>
/// <typeparam name="TScope">The type of event processing scope.</typeparam>
public class EventProcessingRetryPolicy<TScope> : IEventProcessingRetryPolicy<TScope>
    where TScope : IEventProcessorScope
{
    private const int MaxDelaySeconds = 60;
    private readonly ILogger<EventProcessingRetryPolicy<TScope>> _logger;

    // volatile, since access may come from different threads but is never parallel.
    private volatile int _failureCounter;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventProcessingRetryPolicy{TScope}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public EventProcessingRetryPolicy(ILogger<EventProcessingRetryPolicy<TScope>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public int FailureCount => _failureCounter;

    /// <inheritdoc />
    public void Succeeded()
    {
        if (_failureCounter != 0)
        {
            _logger.LogInformation("Subscription back to normal, an event succeeded");
            _failureCounter = 0;
        }
    }

    /// <inheritdoc />
    public async Task<bool> Failed(SubscriptionDroppedReason reason)
    {
        if (reason == SubscriptionDroppedReason.Disposed)
        {
            _logger.LogInformation("Subscription disposed, not trying to reconnect");
            return false;
        }

        if (_failureCounter < int.MaxValue)
        {
            Interlocked.Increment(ref _failureCounter);
        }

        if (_failureCounter == 1)
        {
            _logger.LogInformation("Retrying immediately");
            return true;
        }

        // first retry should be immediately, then 2s^({failureCounter}-1)
        var delaySecs = Math.Min(Math.Pow(2, _failureCounter - 1), MaxDelaySeconds);
        _logger.LogInformation("Retrying in {Delay}s, already failed {FailureCounter} times", delaySecs, _failureCounter);
        await Task.Delay(TimeSpan.FromSeconds(delaySecs)).ConfigureAwait(false);
        return true;
    }
}
