// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.Diagnostics;

/// <summary>
/// A health check which indicates whether a subscription has caught up.
/// </summary>
/// <typeparam name="TSubscriptionScope">The scope of the subscription.</typeparam>
public class EventStoreSubscriptionCatchUpReadinessCheck<TSubscriptionScope> : IHealthCheck
    where TSubscriptionScope : IEventProcessorScope
{
    /// <summary>
    /// The name prefix of this health check.
    /// The full name of the scope gets appended.
    /// </summary>
    public const string NamePrefix = "EventStoreSubscriptionCatchUpReadiness-";

    private readonly Subscription<TSubscriptionScope> _subscription;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreSubscriptionCatchUpReadinessCheck{TSubscriptionScope}"/> class.
    /// </summary>
    /// <param name="subscription">The subscription.</param>
    public EventStoreSubscriptionCatchUpReadinessCheck(Subscription<TSubscriptionScope> subscription)
    {
        _subscription = subscription;
    }

    /// <inheritdoc/>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return _subscription.IsCatchUp
            ? Task.FromResult(HealthCheckResult.Degraded())
            : Task.FromResult(HealthCheckResult.Healthy());
    }
}
