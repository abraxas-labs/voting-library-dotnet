// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fennel.CSharp;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Voting.Lib.Prometheus.Test;

public class PrometheusHttpClientExtensionsTest
{
    private const string Host = "http://localhost";

    [Fact]
    public async Task GetPrometheusShouldWork()
    {
        var client = MockPrometheus();
        var lines = (await client.GetPrometheusAsync()).ToList();
        lines.Should().HaveCount(5);
        lines.OfType<Blank>().Should().HaveCount(2);
        AssertMetrics(lines.OfType<Metric>().ToList());
    }

    [Fact]
    public async Task GetPrometheusMetricsShouldWork()
    {
        var client = MockPrometheus();
        var metrics = (await client.GetPrometheusMetricsAsync()).ToList();
        AssertMetrics(metrics);
    }

    [Fact]
    public async Task GetPrometheusTypedMetricsShouldWork()
    {
        var client = MockPrometheus();
        var metrics = await client.GetPrometheusMetricsAsync<SimpleMetrics>();
        metrics.SimpleMetric.Should().Be(111);
        metrics.SimpleMetricBar.Should().Be(222);
        metrics.SimpleMetricBaz.Should().Be(333);
    }

    private HttpClient MockPrometheus()
    {
        var mockHttp = new MockHttpMessageHandler();
        var prometheusContent = @"
simple_metric 111
simple_metric_with_label{foo=""bar""} 222
simple_metric_with_label{foo=""baz""} 333
";
        mockHttp
            .When(Host + "/metrics")
            .Respond("text/plain", prometheusContent);
        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri(Host);
        return client;
    }

    private void AssertMetrics(IReadOnlyList<Metric> metrics)
    {
        metrics.Should().HaveCount(3);
        metrics[0].Labels.Should().BeEmpty();
        metrics[0].MetricName.Should().Be("simple_metric");
        metrics[0].MetricValue.Should().Be(111);
        metrics[1].Labels.Should().HaveCount(1);
        metrics[1].Labels.Should().Contain("foo", "bar");
        metrics[1].MetricName.Should().Be("simple_metric_with_label");
        metrics[1].MetricValue.Should().Be(222);
        metrics[2].Labels.Should().HaveCount(1);
        metrics[2].Labels.Should().Contain("foo", "baz");
        metrics[2].MetricName.Should().Be("simple_metric_with_label");
        metrics[2].MetricValue.Should().Be(333);
    }

    private class SimpleMetrics
    {
        [PrometheusMetric("simple_metric")]
        public double SimpleMetric { get; set; }

        [PrometheusMetric("simple_metric_with_label")]
        [PrometheusMetricLabel("foo", "bar")]
        public double SimpleMetricBar { get; set; }

        [PrometheusMetric("simple_metric_with_label")]
        [PrometheusMetricLabel("foo", "baz")]
        public double SimpleMetricBaz { get; set; }
    }
}
