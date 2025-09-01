// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RichardSzalay.MockHttp;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Models;
using Xunit;

namespace Voting.Lib.Iam.Test.AuthenticationScheme;

public class RoleTokenHandlerTest : IDisposable
{
    private readonly SecureConnectOptions _options;
    private readonly MockHttpMessageHandler _httpHandlerMock;
    private readonly IRoleTokenHandler _roleTokenHandler;
    private readonly ECDsaSecurityKey _securityKey;
    private readonly ECDsa _key;
    private readonly JsonWebKeySet _keySet;
    private readonly string _subjectToken;

    public RoleTokenHandlerTest()
    {
        _options = new SecureConnectOptions
        {
            Authority = "https://example.com",
            Audience = "TEST-CLIENT",
            ServiceAccount = "foo",
            ServiceAccountPassword = "bar",
        };

        _key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        _securityKey = new ECDsaSecurityKey(_key) { KeyId = "foo-bar-key" };

        _keySet = new JsonWebKeySet();
        _keySet.Keys.Add(JsonWebKeyConverter.ConvertFromSecurityKey(_securityKey));

        _httpHandlerMock = new MockHttpMessageHandler(BackendDefinitionBehavior.Always);
        AddMockedBackends();

        _subjectToken = CreateSubjectToken();

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(SecureConnectDefaults.BackchannelHttpClientName)).Returns(_httpHandlerMock.ToHttpClient);

        var postConfigureOptions = new SecureConnectPostConfigureOptions(httpClientFactoryMock.Object);
        postConfigureOptions.PostConfigure(SecureConnectDefaults.AuthenticationScheme, _options);

        var optionsMonitorMock = new Mock<IOptionsMonitor<SecureConnectOptions>>();
        optionsMonitorMock
            .Setup(x => x.Get(SecureConnectDefaults.AuthenticationScheme))
            .Returns(_options);

        _roleTokenHandler = new RoleTokenHandler(optionsMonitorMock.Object, _httpHandlerMock.ToHttpClient(), NullLogger<RoleTokenHandler>.Instance);
    }

    [Fact]
    public async Task GetRolesLimitedToSingleAppShouldWork()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleTokenApps = new[] { "App1", "App2" };

        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1" });
        roles.Should().BeEquivalentTo("App1::Role1", "App1::Role2");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesLimitedToSingleAppWithCustomSeparatorShouldWork()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleAppsSeparator = "_";
        _options.RoleTokenApps = new[] { "App1", "App2" };

        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1" });
        roles.Should().BeEquivalentTo("App1_Role1", "App1_Role2");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesLimitedToMultipleAppsShouldWork()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleTokenApps = new[] { "App1", "App2" };

        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1", "App2" });
        roles.Should().BeEquivalentTo("App1::Role1", "App1::Role2", "App2::Role5");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesNotLimitedToAppsShouldWork()
    {
        _options.LimitRolesToAppHeaderApps = false;
        _options.RoleTokenApps = new[] { "App1", "App2" };

        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEquivalentTo("App1::Role1", "App1::Role2", "App2::Role5");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesNotLimitedToAppsShouldFallbackToAudience()
    {
        _options.LimitRolesToAppHeaderApps = false;
        _options.RoleTokenApps = null;
        _options.Audience = "App1";

        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEquivalentTo("Role1", "Role2");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesLimitedToAppsNoAppsShouldReturnEmpty()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleTokenApps = null;

        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRolesNoAudienceShouldReturnEmpty()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleTokenApps = null;
        _options.Audience = null;

        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRolesNoSuccessCodeShouldReturnEmpty()
    {
        _options.LimitRolesToAppHeaderApps = false;

        _httpHandlerMock
            .Expect("/token/roles/subject")
            .Respond(HttpStatusCode.BadRequest);
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEmpty();
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithSameKidButOtherKeyShouldReturnEmpty()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var securityKey = new ECDsaSecurityKey(key) { KeyId = "foo-bar-key" };
        ExpectTokenFetch(t => t.SigningCredentials = new(securityKey, SecurityAlgorithms.EcdsaSha256));
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1" });
        roles.Should().BeEmpty();
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithUnknownKidShouldReturnEmpty()
    {
        var securityKey = new ECDsaSecurityKey(_key) { KeyId = "foo-bar-key-unknown" };
        ExpectTokenFetch(t => t.SigningCredentials = new(securityKey, SecurityAlgorithms.EcdsaSha256));
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1" });
        roles.Should().BeEmpty();
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithKnownKidAfterRefreshShouldWork()
    {
        _options.LimitRolesToAppHeaderApps = false;
        _options.RoleTokenApps = null;
        _options.Audience = "App1";

        var configManager = (ConfigurationManager<OpenIdConnectConfiguration>)_options.ConfigurationManager!;
        configManager.RefreshInterval = ConfigurationManager<OpenIdConnectConfiguration>.MinimumRefreshInterval;

        // call GetRoles to ensure existing keys are loaded
        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEquivalentTo("Role1", "Role2");

        // await refresh interval
        await Task.Delay(configManager.RefreshInterval);

        // add new key
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var securityKey = new ECDsaSecurityKey(key) { KeyId = "foo-bar-key2" };
        _keySet.Keys.Clear();
        _keySet.Keys.Add(JsonWebKeyConverter.ConvertFromSecurityKey(securityKey));
        AddMockedBackends();

        // call GetRoles again
        ExpectTokenFetch(t => t.SigningCredentials = new(securityKey, SecurityAlgorithms.EcdsaSha256));
        roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1");
        roles.Should().BeEquivalentTo("Role1", "Role2");

        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithExpiredTokenShouldReturnEmpty()
    {
        ExpectTokenFetch(t => t.Expires = DateTime.UtcNow.AddMinutes(-1));
        var roles = await _roleTokenHandler.GetRoles(_subjectToken, "12345", "Tenant1", new[] { "App1" });
        roles.Should().BeEmpty();
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithInvalidSubjectShouldReturnEmpty()
    {
        _options.RoleTokenApps = ["App1", "App2"];

        var subjectToken = CreateSubjectToken();
        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(subjectToken, "9999", "Tenant1", ["App1"]);
        roles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRolesWithMissingTokenTypeShouldReturnEmpty()
    {
        var subjectToken = CreateSubjectToken(t =>
        {
            t.Claims.Clear();
            t.Subject = new ClaimsIdentity([new Claim("sub", "99999")]);
        });
        ExpectTokenFetch();
        var roles = await _roleTokenHandler.GetRoles(subjectToken, "12345", "Tenant1", ["App1"]);
        roles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRolesLimitedToSingleAppShouldWorkWhenUsingOnBehalfOfToken()
    {
        _options.LimitRolesToAppHeaderApps = true;
        _options.RoleTokenApps = ["App1", "App2"];

        var subjectToken = CreateSubjectToken(t =>
        {
            t.Claims[SecureConnectTokenClaimTypes.TokenType] = SecureConnectTokenTypes.OnBehalfOfToken;
            t.Claims[ClaimTypes.Actor] = """{"act": {"sub": "999"}}""";
        });
        ExpectTokenFetch(onBehalfToken: true);
        var roles = await _roleTokenHandler.GetRoles(subjectToken, "12345", "Tenant1", ["App1"]);
        roles.Should().BeEquivalentTo("App1::Role1", "App1::Role2");
        _httpHandlerMock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRolesWithInvalidSubjectShouldReturnEmptyWhenUsingOnBehalfOfToken()
    {
        _options.RoleTokenApps = ["App1", "App2"];

        var subjectToken = CreateSubjectToken(t => t.Claims[SecureConnectTokenClaimTypes.TokenType] = SecureConnectTokenTypes.OnBehalfOfToken);
        ExpectTokenFetch(onBehalfToken: true);
        var roles = await _roleTokenHandler.GetRoles(subjectToken, "9999", "Tenant1", ["App1"]);
        roles.Should().BeEmpty();
    }

    public void Dispose()
    {
        _httpHandlerMock.Dispose();
        _key.Dispose();
        GC.SuppressFinalize(this);
    }

    private void ExpectTokenFetch(Action<SecurityTokenDescriptor>? modifier = null, bool onBehalfToken = false)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sub", "12345") }),
            Audience = "TEST-CLIENT",
            Issuer = "https://example.com",
            Claims = new Dictionary<string, object>
            {
                ["permissions"] = new Dictionary<string, object>
                {
                    ["App1"] = new Dictionary<string, string[]>
                    {
                        ["Tenant1"] = new[] { "Role1", "Role2" },
                        ["Tenant2"] = new[] { "Role3", "Role4" },
                    },
                    ["App2"] = new Dictionary<string, string[]>
                    {
                        ["Tenant1"] = new[] { "Role5" },
                        ["Tenant2"] = new[] { "Role6" },
                    },
                    ["App3"] = new Dictionary<string, string[]>
                    {
                        ["Tenant1"] = new[] { "Role7" },
                        ["Tenant2"] = new[] { "Role8" },
                    },
                },
            },

            // since Microsoft.IdentityModel.Tokens.Validators.ValidateLifetime does not use the ISystemClock, we cannot use mocked dates
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(1),
            NotBefore = DateTime.UtcNow.AddMinutes(-1),
            SigningCredentials = new(_securityKey, SecurityAlgorithms.EcdsaSha256),
        };

        modifier?.Invoke(tokenDescriptor);
        var token = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
        _httpHandlerMock
            .Expect(HttpMethod.Post, "/token/roles/subject")
            .Respond(JsonContent.Create(new TokenResponse { ExpiresIn = 60, Token = token.RawData }, options: SecureConnectDefaults.JsonOptions));
    }

    private void AddMockedBackends()
    {
        var openidConfig = new OpenIdConnectConfiguration
        {
            Issuer = "https://example.com",
            TokenEndpoint = "https://example.com/token",
            JwksUri = "https://example.com/keys",
        };

        _httpHandlerMock.ResetBackendDefinitions();
        _httpHandlerMock.AddBackendDefinition(
            new MockedRequest("/.well-known/openid-configuration")
                .Respond(JsonContent.Create(openidConfig, options: SecureConnectDefaults.JsonOptions)));
        _httpHandlerMock.AddBackendDefinition(
            new MockedRequest("/keys")
                .Respond(JsonContent.Create(_keySet, options: SecureConnectDefaults.JsonOptions)));
    }

    private string CreateSubjectToken(Action<SecurityTokenDescriptor>? modifier = null)
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
        modifier?.Invoke(tokenDescriptor);
        return jwtHandler.CreateJwtSecurityToken(tokenDescriptor).RawData;
    }
}
