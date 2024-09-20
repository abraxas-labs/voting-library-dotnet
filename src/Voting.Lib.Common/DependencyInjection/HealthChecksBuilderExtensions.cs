// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common.Configuration;
using Voting.Lib.Common.HealthChecks;
using Voting.Lib.Common.Net;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IHealthChecksBuilder"/>.
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Add a health check for configured http probes.
    /// Ensure a <see cref="HttpProbesHealthCheckConfig"/> is registered in the <see cref="IServiceCollection"/>
    /// when adding the health check to the <seealso cref="IHealthChecksBuilder"/> without a config.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <returns>Returns the health checks builder.</returns>
    public static IHealthChecksBuilder AddHttpProbesHealthCheck(this IHealthChecksBuilder builder) =>
        builder.AddCheck<HttpProbesHealthCheck>(
            HttpProbesHealthCheck.Name,
            tags: new[] { HealthCheckTags.LowPriority });

    /// <summary>
    /// Add a health check for configured http probes.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <param name="httpProbesHealthCheckConfig">The http probe health checks config.</param>
    /// <returns>Returns the health checks builder.</returns>
    public static IHealthChecksBuilder AddHttpProbesHealthCheck(
        this IHealthChecksBuilder builder,
        HttpProbesHealthCheckConfig httpProbesHealthCheckConfig)
    {
        builder.Services.AddHttpProbeHealthCheckConfig(httpProbesHealthCheckConfig);
        return builder.AddHttpProbesHealthCheck();
    }

    /// <summary>
    /// Add a health check for configured http probes.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <param name="httpProbesHealthCheckConfig">The http probe health checks config.</param>
    /// <param name="certPinningConfig">The certificate pinning options.</param>
    /// <returns>Returns the health checks builder.</returns>
    public static IHealthChecksBuilder AddHttpProbesHealthCheck(
        this IHealthChecksBuilder builder,
        HttpProbesHealthCheckConfig httpProbesHealthCheckConfig,
        CertificatePinningConfig certPinningConfig)
    {
        builder.Services.AddHttpProbeHealthCheckConfig(httpProbesHealthCheckConfig, certPinningConfig);
        return builder.AddHttpProbesHealthCheck();
    }
}
