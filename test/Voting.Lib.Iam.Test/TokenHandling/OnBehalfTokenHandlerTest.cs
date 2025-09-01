// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RichardSzalay.MockHttp;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.TokenHandling.OnBehalfToken;
using Voting.Lib.Iam.TokenHandling.ServiceToken;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.TokenHandling;

public class OnBehalfTokenHandlerTest
{
    [Fact]
    public async Task GetTokenShouldWork()
    {
        var options = new SecureConnectOnBehalfOptions
        {
            Resource = "App1",
        };

        var serviceAccountOptions = new SecureConnectServiceAccountOptions
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
        httpClientFactoryMock.Setup(x => x.CreateClient(serviceAccountOptions.ServiceTokenClientName)).Returns(httpMessageHandler.ToHttpClient);

        var postConfigureOptions = new SecureConnectServiceAccountPostConfigureOptions(httpClientFactoryMock.Object);
        postConfigureOptions.PostConfigure("test", serviceAccountOptions);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext
            {
                Request =
                {
                    Headers = { new KeyValuePair<string, StringValues>("Authorization", CreateSubjectToken()) },
                },
            });

        var clock = MockedClock.CreateFakeTimeProvider();
        var handler = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, options, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient());

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

    private string CreateSubjectToken()
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sub", "12345") }),
            Claims = new Dictionary<string, object>
            {
                { SecureConnectTokenClaimTypes.TokenType, SecureConnectTokenTypes.AccessToken },
            },
        };
        return jwtHandler.CreateJwtSecurityToken(tokenDescriptor).RawData;
    }
}
