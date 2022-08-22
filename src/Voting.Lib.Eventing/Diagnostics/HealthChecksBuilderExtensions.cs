// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common;
using Voting.Lib.Eventing.Diagnostics;
using Voting.Lib.Eventing.Subscribe;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IHealthChecksBuilder"/>.
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Adds an event store health check.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder instance.</returns>
    public static IHealthChecksBuilder AddEventStore(this IHealthChecksBuilder builder)
        => builder.AddCheck<EventStoreHealthCheck>(
            EventStoreHealthCheck.Name,
            tags: new[] { EventStoreHealthCheck.Tag });

    /// <summary>
    /// Adds an event store subscription catch up health check for the transient subscription.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder instance.</returns>
    public static IHealthChecksBuilder AddEventStoreTransientSubscriptionCatchUp(this IHealthChecksBuilder builder)
        => builder.AddEventStoreSubscriptionCatchUp<TransientEventProcessorScope>();

    /// <summary>
    /// Adds an event store subscription catch up health check.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <typeparam name="TSubscriptionScope">The scope of the subscription.</typeparam>
    /// <returns>The builder instance.</returns>
    public static IHealthChecksBuilder AddEventStoreSubscriptionCatchUp<TSubscriptionScope>(this IHealthChecksBuilder builder)
        where TSubscriptionScope : IEventProcessorScope
        => builder.AddCheck<EventStoreSubscriptionCatchUpHealthCheck<TSubscriptionScope>>(
            EventStoreSubscriptionCatchUpHealthCheck<TSubscriptionScope>.NamePrefix + typeof(TSubscriptionScope).FullName,
            tags: new[] { EventStoreHealthCheck.Tag, HealthChecks.Tags.Readiness });
}
