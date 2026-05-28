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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RichardSzalay.MockHttp;
using Voting.Lib.Common.Cache;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.TokenHandling;
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
        using var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var cache = new Cache<TokenWithExpiration>(memoryCache, new CacheOptions<TokenWithExpiration>());
        var handler = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, options, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient(), cache);

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

    [Fact]
    public async Task GetTokenShouldNotShareTokenAcrossDifferentSubjects()
    {
        var options = new SecureConnectOnBehalfOptions { Resource = "App1" };
        var serviceAccountOptions = CreateServiceAccountOptions();

        var httpMessageHandler = new MockHttpMessageHandler();
        ExpectOpenIdConfigFetch(httpMessageHandler);
        var httpClientFactoryMock = CreateHttpClientFactoryMock(httpMessageHandler, serviceAccountOptions);
        new SecureConnectServiceAccountPostConfigureOptions(httpClientFactoryMock.Object).PostConfigure("test", serviceAccountOptions);

        var subjectTokenUserA = CreateSubjectToken("user-a");
        var subjectTokenUserB = CreateSubjectToken("user-b");
        var currentSubjectToken = subjectTokenUserA;

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(() => new DefaultHttpContext
        {
            Request =
            {
                Headers = { new KeyValuePair<string, StringValues>("Authorization", currentSubjectToken) },
            },
        });

        var clock = MockedClock.CreateFakeTimeProvider();
        using var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var cache = new Cache<TokenWithExpiration>(memoryCache, new CacheOptions<TokenWithExpiration>());
        var handler = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, options, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient(), cache);

        // user A requests an ob_token first and "wins" the cache slot
        ExpectTokenFetch(httpMessageHandler, "ob-token-user-a");
        (await handler.GetToken(CancellationToken.None)).Should().Be("ob-token-user-a");
        httpMessageHandler.VerifyNoOutstandingExpectation();

        // user B comes in with a different subject token: must trigger a fresh fetch, NOT reuse user A's token
        currentSubjectToken = subjectTokenUserB;
        ExpectTokenFetch(httpMessageHandler, "ob-token-user-b");
        (await handler.GetToken(CancellationToken.None)).Should().Be("ob-token-user-b");
        httpMessageHandler.VerifyNoOutstandingExpectation();

        // both subjects should still get their own cached tokens on subsequent calls without further fetches
        (await handler.GetToken(CancellationToken.None)).Should().Be("ob-token-user-b");
        currentSubjectToken = subjectTokenUserA;
        (await handler.GetToken(CancellationToken.None)).Should().Be("ob-token-user-a");
        httpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetTokenShouldNotShareTokenAcrossDifferentHandlerInstances()
    {
        // Two handler instances (e.g. configured for different downstream resources) share the
        // same ICache<CachedToken> singleton. They must never collide on cache keys even if
        // the inbound subject token is identical.
        var optionsA = new SecureConnectOnBehalfOptions { Resource = "App1" };
        var optionsB = new SecureConnectOnBehalfOptions { Resource = "App2" };
        var serviceAccountOptions = CreateServiceAccountOptions();

        var httpMessageHandler = new MockHttpMessageHandler();
        ExpectOpenIdConfigFetch(httpMessageHandler);
        var httpClientFactoryMock = CreateHttpClientFactoryMock(httpMessageHandler, serviceAccountOptions);
        new SecureConnectServiceAccountPostConfigureOptions(httpClientFactoryMock.Object).PostConfigure("test", serviceAccountOptions);

        var subjectToken = CreateSubjectToken("user-a");
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(() => new DefaultHttpContext
        {
            Request =
            {
                Headers = { new KeyValuePair<string, StringValues>("Authorization", subjectToken) },
            },
        });

        var clock = MockedClock.CreateFakeTimeProvider();
        using var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var cache = new Cache<TokenWithExpiration>(memoryCache, new CacheOptions<TokenWithExpiration>());
        var handlerA = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, optionsA, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient(), cache);
        var handlerB = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, optionsB, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient(), cache);

        ExpectTokenFetch(httpMessageHandler, "ob-token-resource-a");
        (await handlerA.GetToken(CancellationToken.None)).Should().Be("ob-token-resource-a");

        ExpectTokenFetch(httpMessageHandler, "ob-token-resource-b");
        (await handlerB.GetToken(CancellationToken.None)).Should().Be("ob-token-resource-b");

        // Both handlers should keep their own cached values across subsequent calls.
        (await handlerA.GetToken(CancellationToken.None)).Should().Be("ob-token-resource-a");
        (await handlerB.GetToken(CancellationToken.None)).Should().Be("ob-token-resource-b");
        httpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetTokenWithConcurrentSameSubjectShouldFetchOnlyOnce()
    {
        // Concurrent callers with the same subject must not all trigger a fetch in parallel.
        // The per-key AsyncLock + double-checked cache lookup must coalesce them into a single fetch.
        var options = new SecureConnectOnBehalfOptions { Resource = "App1" };
        var serviceAccountOptions = CreateServiceAccountOptions();

        var httpMessageHandler = new MockHttpMessageHandler();
        ExpectOpenIdConfigFetch(httpMessageHandler);
        var httpClientFactoryMock = CreateHttpClientFactoryMock(httpMessageHandler, serviceAccountOptions);
        new SecureConnectServiceAccountPostConfigureOptions(httpClientFactoryMock.Object).PostConfigure("test", serviceAccountOptions);

        var subjectToken = CreateSubjectToken("user-a");
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(() => new DefaultHttpContext
        {
            Request =
            {
                Headers = { new KeyValuePair<string, StringValues>("Authorization", subjectToken) },
            },
        });

        var clock = MockedClock.CreateFakeTimeProvider();
        using var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var cache = new Cache<TokenWithExpiration>(memoryCache, new CacheOptions<TokenWithExpiration>());
        var handler = new OnBehalfTokenHandler(NullLogger<OnBehalfTokenHandler>.Instance, options, serviceAccountOptions, clock, httpContextAccessorMock.Object, httpMessageHandler.ToHttpClient(), cache);

        // Only one token fetch is registered with the mock - if more than one caller actually triggers
        // a fetch the mock will throw.
        ExpectTokenFetch(httpMessageHandler, "ob-token");

        var tasks = new Task<string>[16];
        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() => handler.GetToken(CancellationToken.None));
        }

        var tokens = await Task.WhenAll(tasks);
        tokens.Should().AllBe("ob-token");
        httpMessageHandler.VerifyNoOutstandingExpectation();
    }

    private static SecureConnectServiceAccountOptions CreateServiceAccountOptions() => new()
    {
        Authority = "https://example.com",
        UserName = "foo",
        Password = "bar",
        ClientIdScopes = new List<string> { "Scope1", "Scope2" },
    };

    private static void ExpectOpenIdConfigFetch(MockHttpMessageHandler handler)
    {
        handler
            .Expect("/.well-known/openid-configuration")
            .Respond(JsonContent.Create(new OpenIdConnectConfiguration { TokenEndpoint = "https://example.com/token" }, options: SecureConnectDefaults.JsonOptions));
    }

    private static Mock<IHttpClientFactory> CreateHttpClientFactoryMock(MockHttpMessageHandler httpMessageHandler, SecureConnectServiceAccountOptions serviceAccountOptions)
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(SecureConnectDefaults.BackchannelHttpClientName)).Returns(httpMessageHandler.ToHttpClient);
        httpClientFactoryMock.Setup(x => x.CreateClient(serviceAccountOptions.ServiceTokenClientName)).Returns(httpMessageHandler.ToHttpClient);
        return httpClientFactoryMock;
    }

    private void ExpectTokenFetch(MockHttpMessageHandler handler, string token)
    {
        handler
            .Expect(HttpMethod.Post, "/token")
            .Respond(JsonContent.Create(new TokenResponse { ExpiresIn = 600, AccessToken = token }, options: SecureConnectDefaults.JsonOptions));
    }

    private string CreateSubjectToken(string subject = "12345")
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sub", subject) }),
            Claims = new Dictionary<string, object>
            {
                { SecureConnectTokenClaimTypes.TokenType, SecureConnectTokenTypes.AccessToken },
            },
        };
        return jwtHandler.CreateJwtSecurityToken(tokenDescriptor).RawData;
    }
}
