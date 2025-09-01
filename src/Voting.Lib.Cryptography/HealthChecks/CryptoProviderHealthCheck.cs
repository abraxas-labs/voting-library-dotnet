// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Voting.Lib.Cryptography.HealthChecks;

/// <summary>
/// Health check for the crypto provider.
/// </summary>
public class CryptoProviderHealthCheck : IHealthCheck
{
    /// <summary>
    /// The name of this health check.
    /// </summary>
    public const string Name = "CryptoProvider";

    private readonly ICryptoProvider _cryptoProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CryptoProviderHealthCheck"/> class.
    /// </summary>
    /// <param name="cryptoProvider">The PKCS11 device adapter.</param>
    public CryptoProviderHealthCheck(ICryptoProvider cryptoProvider)
    {
        _cryptoProvider = cryptoProvider;
    }

    /// <summary>
    /// Check the health of the crypto provider device connection.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await _cryptoProvider.IsHealthy()
            ? HealthCheckResult.Healthy("Crypto provider connection is healthy")
            : HealthCheckResult.Unhealthy("Crypto provider connection is unhealthy");
    }
}
