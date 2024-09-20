// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Voting.Lib.Common.Configuration;
using Voting.Lib.Common.HealthChecks;
using Voting.Lib.Common.Net;
using Xunit;

namespace Voting.Lib.Common.Test;

public class HttpProbesHealthCheckTest
{
    [Theory]
    [InlineData("https", "example.host.ch", null, "https://example.host.ch/")]
    [InlineData("https", "example.host.ch", "", "https://example.host.ch/")]
    [InlineData("http", "example.host.ch", "healthz", "http://example.host.ch/healthz")]
    [InlineData("https", "example.host.ch:1111", "/healthz", "https://example.host.ch:1111/healthz")]
    public void RequestUri_WhenConfigIsValid_ReturnsAsExpected(string scheme, string host, string? path, string expectedUri)
    {
        var httpProbeConfig = new HttpProbeConfig
        {
            Scheme = scheme,
            Host = host,
            Path = path,
        };

        httpProbeConfig.RequestUri?.ToString().Should().Be(expectedUri);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("https", null, "/healthz")]
    [InlineData("https", "", "/healthz")]
    [InlineData(null, "example.host.ch", "/")]
    [InlineData("", "example.host.ch", "/")]
    public void RequestUri_WhenConfigIsInvalid_ReturnsNull(string? scheme, string? host, string? path)
    {
        var httpProbeConfig = new HttpProbeConfig
        {
            Scheme = scheme!,
            Host = host!,
            Path = path,
        };

        httpProbeConfig.RequestUri.Should().BeNull();
    }

    [Fact]
    public void AddHttpProbesHealthCheckConfig_AddsHttpProbesHealthCheckConfigToServiceCollection()
    {
        // Arrange
        var httpHealthCheckCfg = new HttpProbesHealthCheckConfig();
        var services = new ServiceCollection();

        // Act
        services.AddHttpProbeHealthCheckConfig(httpHealthCheckCfg);
        var provider = services.BuildServiceProvider();
        var resolvedHealthCheckConfig = provider.GetRequiredService<HttpProbesHealthCheckConfig>();

        // Assert
        resolvedHealthCheckConfig.Should().Be(httpHealthCheckCfg);
    }

    [Fact]
    public void AddHttpProbesHealthCheckConfig_WhenNull_AddsAnEmptyConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpProbeHealthCheckConfig(null!, null!);
        var provider = services.BuildServiceProvider();
        var resolvedHealthCheckConfig = provider.GetRequiredService<HttpProbesHealthCheckConfig>();

        // Assert
        resolvedHealthCheckConfig.Probes.Count.Should().Be(0);
    }

    [Fact]
    public void AddHttpProbesHealthCheckAndConfig_AddsHttpProbesHealthCheckToBuilderAndServiceCollection()
    {
        // Arrange
        const string host = "example.com";

        Environment.SetEnvironmentVariable("RequestTimeout", "00:00:06");
        Environment.SetEnvironmentVariable("IsHealthcheckEnabled", "true");
        Environment.SetEnvironmentVariable("Probes__0__Scheme", "http");
        Environment.SetEnvironmentVariable("Probes__0__Host", host);
        Environment.SetEnvironmentVariable("Probes__0__Path", " / healthz");
        Environment.SetEnvironmentVariable("Probes__0__Method", "GET");
        Environment.SetEnvironmentVariable("Probes__0__ExpectedResponseStatusCode", "200");
        Environment.SetEnvironmentVariable("Probes__0__ExpectedResponseContent", "OK");
        Environment.SetEnvironmentVariable("Probes__0__RequestTimeout", "00:00:07");
        Environment.SetEnvironmentVariable("Probes__0__IsHealthcheckEnabled", "true");
        Environment.SetEnvironmentVariable("Probes__0__IsResponseStatusCheckEnabled", "true");
        Environment.SetEnvironmentVariable("Probes__0__IsResponseContentCheckEnabled", "true");

        Environment.SetEnvironmentVariable("Pins__0__Authorities__0", host);
        Environment.SetEnvironmentVariable("Pins__0__ChainPublicKeys__0__PublicKeys__0", "test");

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddEnvironmentVariables();

        var cfg = configBuilder.Build();
        var httpHealthCheckCfg = cfg.Get<HttpProbesHealthCheckConfig>()!;
        var certificateCfg = cfg.Get<CertificatePinningConfig>()!;
        var services = new ServiceCollection();

        // Act
        services.AddHealthChecks().AddHttpProbesHealthCheck(httpHealthCheckCfg, certificateCfg);
        var provider = services.BuildServiceProvider();
        var resolvedHealthCheckConfig = provider.GetRequiredService<HttpProbesHealthCheckConfig>();

        // Assert
        httpHealthCheckCfg.RequestTimeout.Should().Be(TimeSpan.FromSeconds(6));
        httpHealthCheckCfg.Probes.Should().HaveCount(1);
        httpHealthCheckCfg.Probes[0].Host.Should().Be(host);
        httpHealthCheckCfg.Probes[0].HttpMethod.Should().Be(HttpMethod.Get);
        httpHealthCheckCfg.Probes[0].RequestTimeout.Should().Be(TimeSpan.FromSeconds(7));

        certificateCfg.Pins.Should().HaveCount(1);
        certificateCfg.Pins[0].Authorities.FirstOrDefault().Should().Be(host);
        certificateCfg.Pins[0].ChainPublicKeys.Should().HaveCount(1);

        resolvedHealthCheckConfig.Probes.Should().HaveCount(2);
    }

    [Fact]
    public async Task CheckHealthAsync_WhenHealthCheckDisabled_ReturnsHealthyResult()
    {
        // Arrange
        var config = new HttpProbesHealthCheckConfig { IsHealthCheckEnabled = false };
        var httpClientFactory = Mock.Of<IHttpClientFactory>();
        var logger = Mock.Of<ILogger<HttpProbesHealthCheck>>();
        var healthCheck = new HttpProbesHealthCheck(config, httpClientFactory, logger);
        var context = new HealthCheckContext();

        // Act
        var result = await healthCheck.CheckHealthAsync(context);

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("Http probes health check is disabled and therefore considered healthy.");
    }

    [Theory]
    [InlineData("https", "example.com", "/probe1", HttpStatusCode.OK, HttpStatusCode.OK, "OK", "OK", HealthStatus.Healthy)]
    [InlineData("https", "example.com", "/probe1", HttpStatusCode.OK, HttpStatusCode.OK, "OK", "NOK", HealthStatus.Unhealthy)]
    [InlineData("https", "example.com", "/probe1", HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable, "OK", "Service Unavailable", HealthStatus.Unhealthy)]
    public async Task CheckHealthAsync_WhenHttpClientReturnsStatus_ReturnsExpectedHealthResult(
        string scheme,
        string host,
        string path,
        HttpStatusCode expectedResponseStatusCode,
        HttpStatusCode mockedResponseStatusCode,
        string expectedResponseContent,
        string mockedResponseContent,
        HealthStatus expectedHealthStatus)
    {
        // Arrange
        var config = GetHttpProbesHealthCheckConfig(scheme, host, path, expectedResponseStatusCode, expectedResponseContent);
        var httpClientFactory = CreateHttpClientFactoryMock(mockedResponseStatusCode, mockedResponseContent);
        var logger = Mock.Of<ILogger<HttpProbesHealthCheck>>();
        var healthCheck = new HttpProbesHealthCheck(config, httpClientFactory, logger);
        var context = new HealthCheckContext();

        // Act
        var result = await healthCheck.CheckHealthAsync(context);

        // Assert
        result.Status.Should().Be(expectedHealthStatus);
    }

    private static HttpProbesHealthCheckConfig GetHttpProbesHealthCheckConfig(
        string scheme,
        string host,
        string path,
        HttpStatusCode expectedResponseStatusCode,
        string expectedResponseContent)
    {
        return new HttpProbesHealthCheckConfig
        {
            IsHealthCheckEnabled = true,
            Probes = new List<HttpProbeConfig>
            {
                new()
                {
                    IsHealthCheckEnabled = true,
                    Method = "GET",
                    Scheme = scheme,
                    Host = host,
                    Path = path,
                    IsResponseStatusCheckEnabled = true,
                    ExpectedResponseStatusCode = expectedResponseStatusCode,
                    IsResponseContentCheckEnabled = true,
                    ExpectedResponseContent = expectedResponseContent,
                },
            },
        };
    }

    private static IHttpClientFactory CreateHttpClientFactoryMock(HttpStatusCode responseStatusCode, string responseContent)
    {
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage _, CancellationToken _) => new HttpResponseMessage(responseStatusCode)
            {
                Content = new StringContent(responseContent),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        return mockHttpClientFactory.Object;
    }
}
