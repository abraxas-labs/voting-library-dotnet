// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Iam.ServiceTokenHandling;
using Xunit;

namespace Voting.Lib.Iam.Test.ServiceTokenHandling;

public class ServiceTokenHttpMessageHandlerTest
{
    [Fact]
    public async Task ShouldAddToken()
    {
        var serviceTokenHandlerMock = new Mock<IServiceTokenHandler>();
        serviceTokenHandlerMock.Setup(x => x.GetServiceToken()).Returns(Task.FromResult("my-token"));

        var mockHandler = new MockHttpMessageHandler();

        var handler = new ExposingServiceTokenHttpMessageHandler(serviceTokenHandlerMock.Object, NullLogger<ExposingServiceTokenHttpMessageHandler>.Instance);
        handler.InnerHandler = mockHandler;
        await handler.SendAsyncExposed(new(), CancellationToken.None);
        var request = mockHandler.Requests.Single();
        request.Headers.Authorization.Should().NotBeNull();
        request.Headers.Authorization!.Scheme.Should().Be("Bearer");
        request.Headers.Authorization!.Parameter.Should().Be("my-token");
    }

    [Fact]
    public async Task ShouldNotAddTokenIfAlreadySet()
    {
        var serviceTokenHandlerMock = new Mock<IServiceTokenHandler>();
        serviceTokenHandlerMock.Setup(x => x.GetServiceToken()).Returns(Task.FromResult("my-token2"));

        var mockHandler = new MockHttpMessageHandler();

        var handler = new ExposingServiceTokenHttpMessageHandler(serviceTokenHandlerMock.Object, NullLogger<ExposingServiceTokenHttpMessageHandler>.Instance);
        handler.InnerHandler = mockHandler;
        await handler.SendAsyncExposed(new() { Headers = { Authorization = new("Bearer", "my-token") } }, CancellationToken.None);
        var request = mockHandler.Requests.Single();
        request.Headers.Authorization.Should().NotBeNull();
        request.Headers.Authorization!.Scheme.Should().Be("Bearer");
        request.Headers.Authorization!.Parameter.Should().Be("my-token");
    }

    private class ExposingServiceTokenHttpMessageHandler : ServiceTokenHttpMessageHandler
    {
        public ExposingServiceTokenHttpMessageHandler(
            IServiceTokenHandler handler,
            ILogger<ExposingServiceTokenHttpMessageHandler> logger)
            : base(handler, logger)
        {
        }

        public Task<HttpResponseMessage> SendAsyncExposed(HttpRequestMessage request, CancellationToken cancellationToken)
            => SendAsync(request, cancellationToken);
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = new();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            return Task.FromResult(new HttpResponseMessage());
        }
    }
}
