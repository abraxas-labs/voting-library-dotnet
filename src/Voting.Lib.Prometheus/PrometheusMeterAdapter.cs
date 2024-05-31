// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Voting.Lib.Prometheus;

/// <summary>
/// Monitors all .NET Meters and exposes them as Prometheus counters and gauges.
/// We use a custom implementation, since the default implementation of prometheus-net doesn't support any labels.
/// Should be removed as soon as all voting applications switched to OpenTelemetry / OpenMetrics.
/// </summary>
public sealed partial class PrometheusMeterAdapter : BackgroundService
{
    // We use separate listeners for counter-style meters and gauge-style meters.
    // This way we can easily predetermine the type at instrument creation time and not worry about it later.
    private readonly MeterListener _countersListener = new();
    private readonly MeterListener _gaugesListener = new();
    private readonly MeterListener _histogramListener = new();

    // We use this to poll observable metrics once per second.
    private readonly PeriodicTimer _timer;

    private readonly ConcurrentDictionary<string, Counter> _counters = new();
    private readonly ConcurrentDictionary<string, Gauge> _gauges = new();
    private readonly ConcurrentDictionary<string, Histogram> _histograms = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PrometheusMeterAdapter"/> class.
    /// </summary>
    /// <param name="config">The configuration.</param>
    public PrometheusMeterAdapter(PrometheusMeterAdapterConfig config)
    {
        _countersListener.InstrumentPublished += OnCounterInstrumentPublished;
        _countersListener.SetMeasurementEventCallback<byte>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<short>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<int>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<long>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<float>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<double>(OnCounterMeasurement);
        _countersListener.SetMeasurementEventCallback<decimal>(OnCounterMeasurement);

        _countersListener.Start();

        _gaugesListener.InstrumentPublished += OnGaugeInstrumentPublished;
        _gaugesListener.SetMeasurementEventCallback<byte>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<short>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<int>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<long>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<float>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<double>(OnGaugeMeasurement);
        _gaugesListener.SetMeasurementEventCallback<decimal>(OnGaugeMeasurement);

        _gaugesListener.Start();

        _histogramListener.InstrumentPublished += OnHistogramInstrumentPublished;
        _histogramListener.SetMeasurementEventCallback<byte>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<short>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<int>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<long>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<float>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<double>(OnHistogramMeasurement);
        _histogramListener.SetMeasurementEventCallback<decimal>(OnHistogramMeasurement);

        _histogramListener.Start();

        _timer = new(config.Interval);
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _timer.Dispose();
        _countersListener.Dispose();
        _gaugesListener.Dispose();
        _histogramListener.Dispose();
        base.Dispose();
    }

    internal void RecordInstruments()
    {
        _countersListener.RecordObservableInstruments();
        _gaugesListener.RecordObservableInstruments();
        _histogramListener.RecordObservableInstruments();
    }

    /// <summary>
    /// Starts the prometheus meter adapter until the stoppingToken is cancelled.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            RecordInstruments();
        }
    }

    [GeneratedRegex("^[a-zA-Z_][a-zA-Z0-9_]*$")]
    private static partial Regex ValidMetricNameExpression();

    [GeneratedRegex("[^a-zA-Z0-9_]")]
    private static partial Regex NegotiatedMetricNameExpression();

    private static bool IsGenericType(Type givenType, Type genericType)
    {
        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        return baseType != null && IsGenericType(baseType, genericType);
    }

    private void OnCounterInstrumentPublished(Instrument instrument, MeterListener listener)
    {
        if (IsGenericType(instrument.GetType(), typeof(Counter<>))
            || IsGenericType(instrument.GetType(), typeof(ObservableCounter<>)))
        {
            listener.EnableMeasurementEvents(instrument);
        }
    }

    private void OnHistogramInstrumentPublished(Instrument instrument, MeterListener listener)
    {
        if (IsGenericType(instrument.GetType(), typeof(Histogram<>)))
        {
            listener.EnableMeasurementEvents(instrument);
        }
    }

    private void OnGaugeInstrumentPublished(Instrument instrument, MeterListener listener)
    {
        if (IsGenericType(instrument.GetType(), typeof(ObservableGauge<>)))
        {
            listener.EnableMeasurementEvents(instrument);
        }
    }

    private void OnCounterMeasurement(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        var tagsDict = BuildTagsDictionary(tags);
        var instrumentKey = BuildKey(instrument, tags);
        var counter = _counters.GetOrAdd(
            instrumentKey,
            _ => Metrics.CreateCounter(NormalizeMetricsKey(instrument.Name), instrument.Description ?? string.Empty, tagsDict.Keys.ToArray()));
        counter.WithLabels(tagsDict.Values.ToArray()).Inc(measurement);
    }

    private void OnCounterMeasurement(Instrument instrument, byte measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnCounterMeasurement(Instrument instrument, short measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnCounterMeasurement(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnCounterMeasurement(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnCounterMeasurement(Instrument instrument, float measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnCounterMeasurement(Instrument instrument, decimal measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnCounterMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        var tagsDict = BuildTagsDictionary(tags);
        var instrumentKey = BuildKey(instrument, tags);
        var gauge = _gauges.GetOrAdd(
            instrumentKey,
            _ => Metrics.CreateGauge(NormalizeMetricsKey(instrument.Name), instrument.Description ?? string.Empty, tagsDict.Keys.ToArray()));
        gauge.WithLabels(tagsDict.Values.ToArray()).Set(measurement);
    }

    private void OnGaugeMeasurement(Instrument instrument, byte measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, short measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, float measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnGaugeMeasurement(Instrument instrument, decimal measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnGaugeMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        var tagsDict = BuildTagsDictionary(tags);
        var instrumentKey = BuildKey(instrument, tags);
        var histogram = _histograms.GetOrAdd(
            instrumentKey,
            _ => Metrics.CreateHistogram(NormalizeMetricsKey(instrument.Name), instrument.Description ?? string.Empty, tagsDict.Keys.ToArray()));
        histogram.WithLabels(tagsDict.Values.ToArray()).Observe(measurement);
    }

    private void OnHistogramMeasurement(Instrument instrument, byte measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, short measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, float measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private void OnHistogramMeasurement(Instrument instrument, decimal measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        => OnHistogramMeasurement(instrument, (double)measurement, tags, state);

    private string BuildKey(Instrument instrument, ReadOnlySpan<KeyValuePair<string, object?>> tags)
    {
        // this is not really high performance, but should be sufficient for our needs.
        const string sep = "::";
        var sb = new StringBuilder();
        sb.Append(instrument.Name);
        sb.Append(sep);
        foreach (var (tagName, _) in tags)
        {
            sb.Append(tagName);
        }

        return sb.ToString();
    }

    private IReadOnlyDictionary<string, string> BuildTagsDictionary(ReadOnlySpan<KeyValuePair<string, object?>> tags)
    {
        // this is not really high performance, but should be sufficient for our needs.
        var dict = new Dictionary<string, string>(tags.Length);
        foreach (var (tagName, tagValue) in tags)
        {
            dict.Add(NormalizeMetricsKey(tagName), tagValue?.ToString() ?? string.Empty);
        }

        return dict;
    }

    /// <summary>
    /// Replaces invalid non-compliant characters from the metric name with an underline character.
    /// </summary>
    /// <param name="name">The metrics name.</param>
    /// <returns>The normalized metric name.</returns>
    private string NormalizeMetricsKey(string name)
        => ValidMetricNameExpression().IsMatch(name) ? name : NegotiatedMetricNameExpression().Replace(name, "_");
}
