// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Voting.Lib.Iam.Configuration;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Store;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.Store;

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
public class AuthStoreTest
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    private readonly AuthStoreConfig _config = new();
    private readonly AuthStore _authStore;
    private readonly MockLogger<AuthStore> _logger = new();

    public AuthStoreTest()
    {
        _authStore = new AuthStore(_logger, _config);
    }

    [Fact]
    public void SetValuesShouldWork()
    {
        _authStore.IsAuthenticated.Should().BeFalse();
        _authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" });
        _authStore.Roles.Should().BeEquivalentTo("Role1", "Role2");
        _authStore.Tenant.Name.Should().Be("TenantName");
        _authStore.User.Firstname.Should().Be("firstName");
        _authStore.IsAuthenticated.Should().BeTrue();
        _authStore.Permissions.Should().BeEmpty();
    }

    [Fact]
    public void UnauthenticatedShouldThrowOnValueAccess()
    {
        _authStore.IsAuthenticated.Should().BeFalse();
        Assert.Throws<NotAuthenticatedException>(() => _authStore.Roles);
        Assert.Throws<NotAuthenticatedException>(() => _authStore.User);
        Assert.Throws<NotAuthenticatedException>(() => _authStore.Tenant);
        Assert.Throws<NotAuthenticatedException>(() => _authStore.Permissions);
    }

    [Fact]
    public void SetValuesTwiceShouldThrow()
    {
        _authStore.IsAuthenticated.Should().BeFalse();
        _authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" });

        Assert.Throws<AlreadyAuthenticatedException>(() => _authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" }));
    }

    [Fact]
    public void StartLogScopeShouldWork()
    {
        var roles = new[] { "Role1", "Role2" };
        _authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName", Loginid = "123456789" }, new() { Id = "TenantId1", Name = "TenantName" }, roles);
        using (_authStore.StartLogScope())
        {
            _logger.ActiveScopes.Should().HaveCount(1);
            var loggerScope = _logger.ActiveScopes.Cast<IReadOnlyDictionary<string, object>>().Single();
            loggerScope.Should().HaveCount(2);
            loggerScope.Should().Contain("TenantId", "TenantId1");
            loggerScope.TryGetValue("Roles", out var scopeRoles).Should().BeTrue();
            loggerScope.ContainsKey("UserId").Should().BeFalse();
            scopeRoles.Should().BeEquivalentTo(roles);
        }

        _logger.ActiveScopes.Should().BeEmpty();
    }

    [Fact]
    public void StartLogScopeShouldWorkWhenLoggingUserInformation()
    {
        var roles = new[] { "Role1", "Role2" };
        _config.EnableUserInformationLogging = true;
        _authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName", Loginid = "123456789" }, new() { Id = "TenantId1", Name = "TenantName" }, roles);
        using (_authStore.StartLogScope())
        {
            _logger.ActiveScopes.Should().HaveCount(1);
            var loggerScope = _logger.ActiveScopes.Cast<IReadOnlyDictionary<string, object>>().Single();
            loggerScope.Should().HaveCount(3);
            loggerScope.Should().Contain("TenantId", "TenantId1");
            loggerScope.TryGetValue("Roles", out var scopeRoles).Should().BeTrue();
            loggerScope.Should().Contain("UserId", "123456789");
            scopeRoles.Should().BeEquivalentTo(roles);
        }

        _logger.ActiveScopes.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSetPermissions()
    {
        _authStore.SetValues(
            "mock-token",
            new() { Firstname = "firstName", Lastname = "lastName" },
            new() { Name = "TenantName" },
            new[] { "Role1", "Role2" },
            new[] { "p1", "p2", "p3", "p12", "p13" });
        _authStore.Roles.Should().BeEquivalentTo("Role1", "Role2");
        _authStore.Permissions.Should().BeEquivalentTo("p1", "p2", "p3", "p12", "p13");
    }
}
