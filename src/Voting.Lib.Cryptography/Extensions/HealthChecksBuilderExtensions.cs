// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Common.HealthChecks;
using Voting.Lib.Cryptography.HealthChecks;

namespace Voting.Lib.Cryptography.Extensions;

/// <summary>
/// Cryptography extension methods for <see cref="IHealthChecksBuilder"/>.
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Add a health check for the crypto provider.
    /// </summary>
    /// <param name="builder">The health checks builder.</param>
    /// <param name="name">The name of the health check.</param>
    /// <returns>Returns the health checks builder.</returns>
    public static IHealthChecksBuilder AddCryptoProviderHealthCheck(this IHealthChecksBuilder builder, string name = CryptoProviderHealthCheck.Name)
        => builder.AddCheck<CryptoProviderHealthCheck>(name, tags: [HealthCheckTags.LowPriority]);
}
