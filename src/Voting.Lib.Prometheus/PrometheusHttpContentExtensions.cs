// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Fennel.CSharp;
using Voting.Lib.Prometheus;

namespace System.Net.Http;

/// <summary>
/// Prometheus HTTP content extensions.
/// </summary>
public static class PrometheusHttpContentExtensions
{
    /// <summary>
    /// Read raw prometheus information.
    /// </summary>
    /// <param name="content">The HTTP content to read.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the raw prometheus information.</returns>
    public static async Task<IEnumerable<ILine>> ReadFromPrometheusAsync(
        this HttpContent content,
        CancellationToken ct = default)
    {
        var stringContent = await content.ReadAsStringAsync(ct).ConfigureAwait(false);
        return Fennel.CSharp.Prometheus.ParseText(stringContent)
            ?? Enumerable.Empty<ILine>();
    }

    /// <summary>
    /// Read prometheus metrics.
    /// </summary>
    /// <param name="content">The HTTP content to read.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the prometheus metrics.</returns>
    public static async Task<IEnumerable<Metric>> ReadMetricsFromPrometheusAsync(
        this HttpContent content,
        CancellationToken ct = default)
    {
        var prometheusLines = await content.ReadFromPrometheusAsync(ct).ConfigureAwait(false);
        return prometheusLines
            .Where(x => x.IsMetric)
            .OfType<Metric>();
    }

    /// <summary>
    /// Read prometheus metrics.
    /// </summary>
    /// <param name="content">The HTTP content to read.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the prometheus metrics.</returns>
    /// <typeparam name="T">The type of the prometheus metrics.</typeparam>
    public static async Task<T> ReadMetricsFromPrometheusAsync<T>(
        this HttpContent content,
        CancellationToken ct = default)
        where T : class, new()
    {
        var result = new T();
        var metrics = await content.ReadMetricsFromPrometheusAsync(ct).ConfigureAwait(false);
        var groupedMetrics = metrics.ToDictionary(x => new MetricKey(x));
        var properties = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite);
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<PrometheusMetricAttribute>();
            if (attribute == null)
            {
                continue;
            }

            var labels = property.GetCustomAttributes<PrometheusMetricLabelAttribute>()
                .ToDictionary(x => x.Key, x => x.Value);

            var metricKey = new MetricKey(attribute.MetricName, labels);
            if (!groupedMetrics.TryGetValue(metricKey, out var value) || !value.MetricValue.HasValue)
            {
                continue;
            }

            property.SetValue(result, Convert.ChangeType(value.MetricValue, property.PropertyType));
        }

        return result;
    }

    private sealed class MetricKey
    {
        private readonly string _metricName;
        private readonly IEnumerable<KeyValuePair<string, string>> _labels;

        public MetricKey(Metric metric)
            : this(metric.MetricName, metric.Labels)
        {
        }

        public MetricKey(string metricName, IEnumerable<KeyValuePair<string, string>> labels)
        {
            _metricName = metricName;
            _labels = labels;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj is not MetricKey other)
            {
                return false;
            }

            if (!string.Equals(other._metricName, _metricName, StringComparison.Ordinal))
            {
                return false;
            }

            using var otherLabelEnumerator = other._labels.GetEnumerator();
            foreach (var kvp in _labels)
            {
                if (!otherLabelEnumerator.MoveNext())
                {
                    return false;
                }

                if (!kvp.Key.Equals(otherLabelEnumerator.Current.Key, StringComparison.Ordinal)
                    || !kvp.Value.Equals(otherLabelEnumerator.Current.Value, StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return !otherLabelEnumerator.MoveNext();
        }

        public override int GetHashCode()
            => HashCode.Combine(_metricName);
    }
}
