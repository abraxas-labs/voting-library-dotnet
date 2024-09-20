// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Prometheus;

/// <summary>
/// Attribute to add prometheus metric labels.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class PrometheusMetricLabelAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrometheusMetricLabelAttribute"/> class.
    /// </summary>
    /// <param name="key">The label key.</param>
    /// <param name="value">The label value.</param>
    public PrometheusMetricLabelAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the label key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the label value.
    /// </summary>
    public string Value { get; }
}
