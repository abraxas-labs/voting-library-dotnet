// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fennel.CSharp;
using FluentAssertions;
using Prometheus;
using Xunit;

namespace Voting.Lib.Prometheus.Test;

public class PrometheusMeterAdapterTest
{
    [Fact]
    public async Task ShouldPublish()
    {
        // start adapter
        using var adapter = new PrometheusMeterAdapter(new());

        // setup metrics
        var meter = new Meter("TestMeter", "1");

        var testCounter = meter.CreateCounter<int>("test_counter");
        testCounter.Add(10);

        meter.CreateObservableGauge<long>("test_gauge", () => 20);

        // record metrics
        adapter.RecordInstruments();

        await using var ms = new MemoryStream();
        await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(ms, CancellationToken.None);
        await ms.FlushAsync();

        var metrics = Fennel.CSharp.Prometheus.ParseText(Encoding.ASCII.GetString(ms.GetBuffer()))
            .OfType<Metric>()
            .GroupBy(x => x.MetricName)
            .Where(x => !x.Skip(1).Any()) // only single entries
            .ToDictionary(x => x.Key, x => x.Single().MetricValue);
        metrics.Should().Contain("test_counter", 10);
        metrics.Should().Contain("test_gauge", 20);
    }

    [Fact]
    public async Task ShouldPublishWithNormalizedMetricNameAndTag()
    {
        const string invalidMetricCounterKey = "test$metric counter.key";
        const string normalizedMetricCounterKey = "test_metric_counter_key";
        const string invalidMetricGaugeKey = "test$metric gauge.key";
        const string normalizedMetricGaugeKey = "test_metric_gauge_key";
        const string invalidMetricTagKey = "test$metric.label_key";
        const string normalizedMetricTagKey = "test_metric_label_key";
        const string validMetricTagValue = "test$metric.label_value";

        // start adapter
        using var adapter = new PrometheusMeterAdapter(new());

        // setup metrics
        var meter = new Meter("Test$Meter", "1");

        var testCounter = meter.CreateCounter<int>(invalidMetricCounterKey);
        testCounter.Add(10, new KeyValuePair<string, object?>(invalidMetricTagKey, validMetricTagValue));

        var testGague = meter.CreateObservableGauge<long>(invalidMetricGaugeKey, () => 20);

        // record metrics
        adapter.RecordInstruments();

        await using var ms = new MemoryStream();
        await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(ms, CancellationToken.None);
        await ms.FlushAsync();

        var metrics = Fennel.CSharp.Prometheus.ParseText(Encoding.ASCII.GetString(ms.GetBuffer()))
            .OfType<Metric>()
            .GroupBy(x => x.MetricName)
            .Where(x => !x.Skip(1).Any()) // only single entries
            .ToDictionary(x => x.Key, x => x.Single());

        metrics.Keys.Should().Contain(normalizedMetricCounterKey);
        metrics[normalizedMetricCounterKey].MetricValue.Should().Be(10);
        metrics[normalizedMetricCounterKey].Labels.Keys.Should().Contain(normalizedMetricTagKey);
        metrics[normalizedMetricCounterKey].Labels[normalizedMetricTagKey].Should().Be(validMetricTagValue);

        metrics.Keys.Should().Contain(normalizedMetricGaugeKey);
        metrics[normalizedMetricGaugeKey].MetricValue.Should().Be(20);
    }
}
