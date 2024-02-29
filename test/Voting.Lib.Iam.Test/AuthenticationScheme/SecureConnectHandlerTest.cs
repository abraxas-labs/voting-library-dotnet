// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Voting.Lib.Common.Cache;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Authorization;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Store;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.AuthenticationScheme;

public class SecureConnectHandlerTest : IAsyncDisposable
{
    private readonly SecureConnectOptions _options;
    private readonly SecureConnectHandler _secureConnectHandler;
    private readonly ServiceProvider _serviceProvider;
    private readonly MockedJwtBearerHandler _jwtBearerHandler;
    private readonly DefaultHttpContext _httpContext = new();
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ITenantService> _tenantServiceMock;
    private readonly AuthStore _authStore;
    private IPermissionProvider? _permissionProviderMock;

    public SecureConnectHandlerTest()
    {
        _options = new SecureConnectOptions
        {
            Authority = "https://example.com",
            Audience = "TEST-CLIENT",
            ServiceAccount = "foo",
            ServiceAccountPassword = "bar",
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var postConfigureOptions = new SecureConnectPostConfigureOptions(httpClientFactoryMock.Object);
        postConfigureOptions.PostConfigure(SecureConnectDefaults.AuthenticationScheme, _options);

        var optionsMonitorMock = new Mock<IOptionsMonitor<SecureConnectOptions>>();
        optionsMonitorMock
            .Setup(x => x.Get(SecureConnectDefaults.AuthenticationScheme))
            .Returns(_options);

        _authStore = new AuthStore(NullLogger<AuthStore>.Instance);

        var clock = new MockedClock();
        _jwtBearerHandler = new MockedJwtBearerHandler(
            optionsMonitorMock.Object,
            NullLoggerFactory.Instance,
            UrlEncoder.Default,
            clock);

        _userServiceMock = new Mock<IUserService>();
        _tenantServiceMock = new Mock<ITenantService>();

        _serviceProvider = new ServiceCollection()
            .AddSingleton(_userServiceMock.Object)
            .AddSingleton(_tenantServiceMock.Object)
            .AddSingleton<IPermissionProvider>(_ => _permissionProviderMock!)
            .AddCache(new CacheOptions<User>())
            .AddCache(new CacheOptions<Tenant>())
            .AddCache(new CacheOptions<UserRoles>())
            .AddSingleton<IAuthStore>(_ => _authStore)
            .AddSingleton<IAuth>(_ => _authStore)
            .BuildServiceProvider(true);

        var roleTokenHandlerMock = new Mock<IRoleTokenHandler>();
        roleTokenHandlerMock
            .Setup(x => x.GetRoles("subject-token", "Tenant1", It.IsAny<IEnumerable<string>>()))
            .Returns(Task.FromResult<IReadOnlyCollection<string>>(new[] { "Role1", "Role2" }));
        roleTokenHandlerMock
            .Setup(x => x.GetRoles("subject-token-no-roles", "Tenant1", It.IsAny<IEnumerable<string>>()))
            .Returns(Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>()));

        _secureConnectHandler = new SecureConnectHandler(
            optionsMonitorMock.Object,
            NullLoggerFactory.Instance,
            UrlEncoder.Default,
            clock,
            roleTokenHandlerMock.Object,
            _jwtBearerHandler,
            _serviceProvider);
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task DontFetchRoleTokenShouldWork()
    {
        _options.FetchRoleToken = false;
        await Init();
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeTrue();
        ShouldHaveRoles();
        ShouldHavePermissions();
    }

    [Fact]
    public async Task FetchRoleTokenButNoTenantShouldWork()
    {
        _options.FetchRoleToken = true;
        await Init();
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeTrue();
        ShouldHaveRoles();
        ShouldHavePermissions();
    }

    [Fact]
    public async Task FetchRoleTokenWithTenantShouldSetRolesAndFetchUserAndTenant()
    {
        _options.FetchRoleToken = true;

        _tenantServiceMock
            .Setup(x => x.GetTenant("Tenant1", true))
            .Returns(Task.FromResult<Tenant?>(new() { Id = "Tenant1", Name = "tenant 1" }));

        _userServiceMock
            .Setup(x => x.GetUser("User1", true))
            .Returns(Task.FromResult<User?>(new() { Loginid = "User1", Firstname = "user 1" }));

        var permissionProviderMock = new Mock<IPermissionProvider>();
        permissionProviderMock
            .Setup(x => x.GetPermissionsForRoles(It.IsAny<string[]>()))
            .Returns(new[] { "Permission1" });
        _permissionProviderMock = permissionProviderMock.Object;

        await Init("subject-token", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeTrue();
        ShouldHaveRoles("Role1", "Role2");
        ShouldHavePermissions("Permission1");

        _userServiceMock.Verify();
        _tenantServiceMock.Verify();
    }

    [Fact]
    public async Task FetchRoleTokenWithoutPermissionProviderShouldSetRoles()
    {
        _options.FetchRoleToken = true;

        _tenantServiceMock
            .Setup(x => x.GetTenant("Tenant1", true))
            .Returns(Task.FromResult<Tenant?>(new() { Id = "Tenant1", Name = "tenant 1" }));

        _userServiceMock
            .Setup(x => x.GetUser("User1", true))
            .Returns(Task.FromResult<User?>(new() { Loginid = "User1", Firstname = "user 1" }));

        await Init("subject-token", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeTrue();
        ShouldHaveRoles("Role1", "Role2");
        ShouldHavePermissions();
    }

    [Fact]
    public async Task FetchRoleTokenWithAnyRoleRequiredButNoRoleShouldFail()
    {
        _options.FetchRoleToken = true;

        await Init("subject-token-no-roles", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeFalse();
        ShouldHaveRoles();
        ShouldHavePermissions();
        result.Failure!.Message.Should().Be("No roles present in role token");
    }

    [Fact]
    public async Task FetchRoleTokenWithoutAnyRoleRequiredAndNoRoleShouldWork()
    {
        _options.FetchRoleToken = true;
        _options.AnyRoleRequired = false;

        await Init("subject-token-no-roles", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeTrue();
        ShouldHaveRoles();
        ShouldHavePermissions();
    }

    [Fact]
    public async Task NoNameIdentifierShouldFail()
    {
        _options.FetchRoleToken = true;
        _options.AnyRoleRequired = false;

        _jwtBearerHandler.ClearClaims();

        await Init("subject-token", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Be("No sub present in token");
    }

    [Fact]
    public async Task JwtFailureShouldFail()
    {
        _options.FetchRoleToken = true;
        _jwtBearerHandler.ShouldFail = true;

        await Init("subject-token", "Tenant1");
        var result = await _secureConnectHandler.AuthenticateAsync();
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Be("jwt expected failure");
    }

    private void ShouldHaveRoles(params string[] roles)
    {
        _jwtBearerHandler.Identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).Should().BeEquivalentTo(roles);

        if (roles.Length != 0)
        {
            _authStore.Roles.Should().BeEquivalentTo(roles);
        }
    }

    private void ShouldHavePermissions(params string[] permissions)
    {
        if (!_authStore.IsAuthenticated && permissions.Length == 0)
        {
            return;
        }

        _authStore.Permissions.Should().BeEquivalentTo(permissions);
    }

    private async Task Init(string? subjectToken = null, string? tenantId = null)
    {
        if (tenantId != null)
        {
            _httpContext.Request.Headers["Authorization"] = "Bearer " + subjectToken;
        }

        if (subjectToken != null)
        {
            _httpContext.Request.Headers["x-tenant"] = tenantId;
        }

        await _secureConnectHandler.InitializeAsync(new(SecureConnectDefaults.AuthenticationScheme, null, _secureConnectHandler.GetType()), _httpContext);
    }

    private class MockedJwtBearerHandler : JwtBearerHandler
    {
        public MockedJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        internal ClaimsIdentity Identity { get; private set; } = new(claims: new[] { new Claim(ClaimTypes.NameIdentifier, "User1") });

        internal bool ShouldFail { get; set; }

        internal void ClearClaims()
        {
            Identity = new();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(ShouldFail
                ? AuthenticateResult.Fail("jwt expected failure")
                : AuthenticateResult.Success(new(new(Identity), "Jwt")));
        }
    }
}
