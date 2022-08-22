// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Prometheus;

/// <summary>
/// Attribute for prometheus metric name.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PrometheusMetricAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrometheusMetricAttribute"/> class.
    /// </summary>
    /// <param name="metricName">The metric name.</param>
    public PrometheusMetricAttribute(string metricName)
    {
        MetricName = metricName;
    }

    /// <summary>
    /// Gets the metric name.
    /// </summary>
    public string MetricName { get; }
}
