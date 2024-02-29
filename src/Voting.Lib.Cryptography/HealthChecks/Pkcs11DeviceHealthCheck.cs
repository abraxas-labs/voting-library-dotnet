// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Voting.Lib.Cryptography.Asymmetric;

namespace Voting.Lib.Cryptography.HealthChecks;

/// <summary>
/// Health check for the PKCS11 device.
/// </summary>
public class Pkcs11DeviceHealthCheck : IHealthCheck
{
    /// <summary>
    /// The name of this health check.
    /// </summary>
    public const string Name = "Pkcs11";

    private readonly IPkcs11DeviceAdapter _pkcs11DeviceAdapter;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pkcs11DeviceHealthCheck"/> class.
    /// </summary>
    /// <param name="pkcs11DeviceAdapter">The PKCS11 device adapter.</param>
    public Pkcs11DeviceHealthCheck(IPkcs11DeviceAdapter pkcs11DeviceAdapter)
    {
        _pkcs11DeviceAdapter = pkcs11DeviceAdapter;
    }

    /// <summary>
    /// Check the health of the PKCS#11 device connection.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the health check result.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_pkcs11DeviceAdapter.IsHealthy()
            ? HealthCheckResult.Healthy("PKCS#11 connection is healthy")
            : HealthCheckResult.Unhealthy("PKCS#11 connection is unhealthy"));
    }
}
