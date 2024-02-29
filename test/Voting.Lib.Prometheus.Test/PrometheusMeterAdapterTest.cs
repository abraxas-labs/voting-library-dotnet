// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

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
}
