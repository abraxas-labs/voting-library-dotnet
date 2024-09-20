// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Voting.Lib.Eventing.Configuration;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.Diagnostics;

/// <summary>
/// A health check which indicates whether a subscription is successfully processing events.
/// </summary>
/// <typeparam name="TSubscriptionScope">The scope of the subscription.</typeparam>
public class EventStoreSubscriptionHealthCheck<TSubscriptionScope> : IHealthCheck
    where TSubscriptionScope : IEventProcessorScope
{
    /// <summary>
    /// The name prefix of this health check.
    /// The full name of the scope gets appended.
    /// </summary>
    public const string NamePrefix = "EventStoreSubscriptionHealth-";

    private readonly IEventProcessingRetryPolicy<TSubscriptionScope> _retryPolicy;
    private readonly EventStoreConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreSubscriptionHealthCheck{TSubscriptionScope}"/> class.
    /// </summary>
    /// <param name="retryPolicy">The event processing retry policy.</param>
    /// <param name="config">The configuration.</param>
    public EventStoreSubscriptionHealthCheck(IEventProcessingRetryPolicy<TSubscriptionScope> retryPolicy, EventStoreConfig config)
    {
        _retryPolicy = retryPolicy;
        _config = config;
    }

    /// <inheritdoc/>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return _retryPolicy.FailureCount >= _config.MaxEventProcessingFailureCount
            ? Task.FromResult(HealthCheckResult.Unhealthy())
            : Task.FromResult(HealthCheckResult.Healthy());
    }
}
