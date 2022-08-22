// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Common;
using Voting.Lib.Cryptography.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Cryptography extension methods for <see cref="IHealthChecksBuilder"/>.
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Add a health check for the PKCS11 device.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <returns>Returns the health checks builder.</returns>
    public static IHealthChecksBuilder AddPkcs11HealthCheck(this IHealthChecksBuilder builder)
        => builder.AddCheck<Pkcs11DeviceHealthCheck>(Pkcs11DeviceHealthCheck.Name, tags: new[] { HealthChecks.Tags.LowPriority });
}
