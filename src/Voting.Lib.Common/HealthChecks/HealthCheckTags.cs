// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Common.HealthChecks;

/// <summary>
/// Common voting health check tags.
/// </summary>
public static class HealthCheckTags
{
    /// <summary>
    /// A health check tag which indicates this health check is a readiness check and not a live check.
    /// Eg. the system has built internal caches etc.
    /// </summary>
    public const string Readiness = "readiness";

    /// <summary>
    /// A health check tag which indicates this health check is a low priority check and is not mission critical for the application.
    /// </summary>
    public const string LowPriority = "low-prio";
}
