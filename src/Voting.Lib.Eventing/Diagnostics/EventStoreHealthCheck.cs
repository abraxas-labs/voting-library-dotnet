// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Voting.Lib.Eventing.Read;

namespace Voting.Lib.Eventing.Diagnostics;

/// <summary>
/// Health check for the EventStore.
/// </summary>
public class EventStoreHealthCheck : IHealthCheck
{
    /// <summary>
    /// The name of this health check.
    /// </summary>
    public const string Name = "EventStore";

    /// <summary>
    /// The name of the health check tag which is applied to all event-store tags.
    /// </summary>
    public const string Tag = "event-store";

    private readonly IEventReader _reader;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreHealthCheck"/> class.
    /// </summary>
    /// <param name="reader">The EventStore reader.</param>
    public EventStoreHealthCheck(IEventReader reader)
    {
        _reader = reader;
    }

    /// <summary>
    /// Checks the health of the EventStore connection.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _reader.GetLatestEventPosition(WellKnownStreams.All, cancellationToken).ConfigureAwait(false);
            return HealthCheckResult.Healthy();
        }
        catch (Exception)
        {
            return HealthCheckResult.Degraded();
        }
    }
}
