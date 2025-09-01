// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using RichardSzalay.MockHttp;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.TokenHandling.ServiceToken;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.TokenHandling;

public class ServiceTokenHandlerTest
{
    [Fact]
    public async Task GetTokenShouldWork()
    {
        var options = new SecureConnectServiceAccountOptions
        {
            Authority = "https://example.com",
            UserName = "foo",
            Password = "bar",
            ClientIdScopes = new List<string> { "Scope1", "Scope2" },
        };

        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler
            .Expect("/.well-known/openid-configuration")
            .Respond(JsonContent.Create(new OpenIdConnectConfiguration { TokenEndpoint = "https://example.com/token" }, options: SecureConnectDefaults.JsonOptions));

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(SecureConnectDefaults.BackchannelHttpClientName)).Returns(httpMessageHandler.ToHttpClient);
        httpClientFactoryMock.Setup(x => x.CreateClient(options.ServiceTokenClientName)).Returns(httpMessageHandler.ToHttpClient);

        var postConfigureOptions = new SecureConnectServiceAccountPostConfigureOptions(httpClientFactoryMock.Object);
        postConfigureOptions.PostConfigure("test", options);

        var clock = MockedClock.CreateFakeTimeProvider();
        var handler = new ServiceTokenHandler(NullLogger<ServiceTokenHandler>.Instance, options, clock, httpClientFactoryMock.Object);

        // fetch the token the first time
        // should fetch the configuration and the token
        ExpectTokenFetch(httpMessageHandler, "foo-bar-access-token");
        var token = await handler.GetToken(CancellationToken.None);
        token.Should().Be("foo-bar-access-token");
        httpMessageHandler.VerifyNoOutstandingExpectation();

        // fetch the token again, should be cached
        token = await handler.GetToken(CancellationToken.None);
        token.Should().Be("foo-bar-access-token");
        httpMessageHandler.VerifyNoOutstandingExpectation();

        // seek the time ahead 1 hour
        // token should be expired and a new one should be fetched
        clock.Advance(TimeSpan.FromHours(1));
        ExpectTokenFetch(httpMessageHandler, "foo-bar-access-token2");
        token = await handler.GetToken(CancellationToken.None);
        token.Should().Be("foo-bar-access-token2");
        httpMessageHandler.VerifyNoOutstandingExpectation();

        // seek the time slightly before the token expiry
        // token should be assumed as expired and a new one should be fetched.
        clock.Advance(TimeSpan.FromSeconds(599));
        ExpectTokenFetch(httpMessageHandler, "foo-bar-access-token3");
        token = await handler.GetToken(CancellationToken.None);
        token.Should().Be("foo-bar-access-token3");
        httpMessageHandler.VerifyNoOutstandingExpectation();
    }

    private void ExpectTokenFetch(MockHttpMessageHandler handler, string token)
    {
        handler
            .Expect(HttpMethod.Post, "/token")
            .Respond(JsonContent.Create(new TokenResponse { ExpiresIn = 600, AccessToken = token }, options: SecureConnectDefaults.JsonOptions));
    }
}
