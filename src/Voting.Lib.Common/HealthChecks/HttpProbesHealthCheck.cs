// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common.Configuration;
using Voting.Lib.Common.Net;

namespace Voting.Lib.Common.HealthChecks;

/// <summary>
/// Health check for configured http probes.
/// </summary>
public class HttpProbesHealthCheck : IHealthCheck
{
    /// <summary>
    /// The name of this health check.
    /// </summary>
    public const string Name = "HttpProbes";
    private readonly HttpProbesHealthCheckConfig _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpProbesHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpProbesHealthCheck"/> class.
    /// </summary>
    /// <param name="config">The http probes health check config.</param>
    /// <param name="httpClientFactory">The http client factory.</param>
    /// <param name="logger">The logger.</param>
    public HttpProbesHealthCheck(
        HttpProbesHealthCheckConfig config,
        IHttpClientFactory httpClientFactory,
        ILogger<HttpProbesHealthCheck> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Check the health of the configured http probes.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (!_config.IsHealthCheckEnabled)
        {
            return HealthCheckResult.Healthy("Http probes health check is disabled and therefore considered healthy.");
        }

        var probeValidationTasks = _config.Probes.Where(probe => probe.IsHealthCheckEnabled).Select(probe => ValidateProbe(probe, cancellationToken));
        var probeValidationResults = await Task.WhenAll(probeValidationTasks).ConfigureAwait(false);

        if (probeValidationResults.Any(isValid => !isValid))
        {
            return HealthCheckResult.Unhealthy("Http probes are unhealthy");
        }

        return HealthCheckResult.Healthy("Http probes are healthy");
    }

    /// <summary>
    /// Sends a request to the configured probe and validates the response accordingly.
    /// The cert pinning is implicitly getting validated within the <see cref="CertificatePinningHandler.HandleValidationCallback"/> for pinned authorities.
    /// If the validation fails the connection will be closed and the request will fail.
    /// </summary>
    /// <param name="probe">The http probe to validate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task returning the validation result, where true indicates valid.</returns>
    private async Task<bool> ValidateProbe(HttpProbeConfig probe, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = probe.RequestTimeout ?? _config.RequestTimeout;

            if (probe.RequestUri == null)
            {
                throw new UriFormatException($"Failed to create URI using the passed arguments. Scheme: {probe.Scheme}, Host: {probe.Host}, Path: {probe.Path}");
            }

            using var requestMessage = new HttpRequestMessage(probe.HttpMethod, probe.RequestUri);
            using var response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            if (!IsResponseStatusValid(response.StatusCode, probe))
            {
                return false;
            }

            if (await IsResponseContentValid(response.Content, probe, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }

            _logger.LogError(
                "Configured status code '{ExpectedResponseStatusCode}' does not match with '{ResponseStatusCode}' for {RequestUri}.",
                probe.ExpectedResponseStatusCode,
                response.StatusCode,
                probe.RequestUri);

            return false;
        }
        catch (Exception ex)
        {
            if (ex.InnerException is AuthenticationException)
            {
                _logger.LogError(ex, "Certificate pinning check failed for {Authority}", probe.Host);
            }
            else
            {
                _logger.LogError(ex, "An error occurred while checking http probe to {Scheme}, {Host}, {Path}", probe.Scheme, probe.Host, probe.Path);
            }
        }

        return false;
    }

    private bool IsResponseStatusValid(HttpStatusCode responseStatusCode, HttpProbeConfig probe)
    {
        if (!probe.IsResponseStatusCheckEnabled || responseStatusCode == probe.ExpectedResponseStatusCode)
        {
            return true;
        }

        _logger.LogError(
                "Configured status code '{ExpectedResponseStatusCode}' does not match with '{ResponseStatusCode}' for {RequestUri}.",
                probe.ExpectedResponseStatusCode,
                responseStatusCode,
                probe.RequestUri);

        return false;
    }

    private async Task<bool> IsResponseContentValid(HttpContent httpContent, HttpProbeConfig probe, CancellationToken cancellationToken)
    {
        if (!probe.IsResponseContentCheckEnabled)
        {
            return true;
        }

        var content = await httpContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (probe.ExpectedResponseContent.Equals(content, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        _logger.LogError(
                "Configured response content '{ExpectedResponseContent}' does not match with '{ResponseContent}' for {RequestUri}.",
                probe.ExpectedResponseContent,
                content,
                probe.RequestUri);

        return false;
    }
}
