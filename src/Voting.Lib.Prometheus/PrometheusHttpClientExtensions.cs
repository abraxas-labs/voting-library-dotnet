// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fennel.CSharp;

namespace System.Net.Http;

/// <summary>
/// Prometheus HTTP client extensions.
/// </summary>
public static class PrometheusHttpClientExtensions
{
    /// <summary>
    /// Gets raw prometheus information.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the raw prometheus information.</returns>
    public static async Task<IEnumerable<ILine>> GetPrometheusAsync(
        this HttpClient client,
        string requestUri = "/metrics",
        CancellationToken ct = default)
    {
        using var response = await client.GetAsync(requestUri, ct).ConfigureAwait(false);
        return await response.Content.ReadFromPrometheusAsync(ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets prometheus metrics.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the parsed prometheus metrics.</returns>
    public static async Task<IEnumerable<Metric>> GetPrometheusMetricsAsync(
        this HttpClient client,
        string requestUri = "/metrics",
        CancellationToken ct = default)
    {
        using var response = await client.GetAsync(requestUri, ct).ConfigureAwait(false);
        return await response.Content.ReadMetricsFromPrometheusAsync(ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets prometheus metrics.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the parsed prometheus metrics.</returns>
    /// <typeparam name="T">The type of the prometheus metrics.</typeparam>
    public static async Task<T> GetPrometheusMetricsAsync<T>(
        this HttpClient client,
        string requestUri = "/metrics",
        CancellationToken ct = default)
        where T : class, new()
    {
        using var response = await client.GetAsync(requestUri, ct).ConfigureAwait(false);
        return await response.Content.ReadMetricsFromPrometheusAsync<T>(ct).ConfigureAwait(false);
    }
}
