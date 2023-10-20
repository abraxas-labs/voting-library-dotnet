// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Voting.Lib.Common.HealthChecks;

namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Extension methods for <see cref="IEndpointRouteBuilder"/>.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Adds the voting health checks (/healthz, /healthz/low-prio and /readiness).
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <param name="lowPrioHealthChecks">
    /// A list of health check names which are considered low-priority.
    /// If <c>null</c> is provided, all health checks with contain a <see cref="HealthCheckRegistration.Tags"/> with the value of <see cref="HealthCheckTags.LowPriority"/>
    /// are considered low-priority.
    /// </param>
    /// <returns>The builder instance.</returns>
    public static IEndpointRouteBuilder MapVotingHealthChecks(this IEndpointRouteBuilder endpoints, IReadOnlySet<string>? lowPrioHealthChecks = null)
    {
        bool IsReadinessHealthCheck(HealthCheckRegistration healthCheck)
            => healthCheck.Tags.Contains(HealthCheckTags.Readiness);

        Func<HealthCheckRegistration, bool> isLowPrioHealthCheck = lowPrioHealthChecks == null
            ? x => x.Tags.Contains(HealthCheckTags.LowPriority)
            : x => lowPrioHealthChecks.Contains(x.Name);

        // /healthz contains all mission critical health checks (the service is completely unusable if one of them is unhealthy)
        endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = x => !isLowPrioHealthCheck(x) && !IsReadinessHealthCheck(x),
        });

        // /healthz/low-prio contains all non-mission critical (low-prio) health checks
        // (the service is still usable if one of them is unhealthy but some features may not work)
        endpoints.MapHealthChecks("/healthz/low-prio", new HealthCheckOptions
        {
            Predicate = x => isLowPrioHealthCheck(x) && !IsReadinessHealthCheck(x),
        });

        // /readiness indicates whether a service is ready to accept traffic
        endpoints.MapHealthChecks("/readiness", new HealthCheckOptions
        {
            Predicate = IsReadinessHealthCheck,
        });

        return endpoints;
    }
}
