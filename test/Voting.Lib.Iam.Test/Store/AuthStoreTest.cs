// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Store;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.Store;

public class AuthStoreTest
{
    [Fact]
    public void SetValuesShouldWork()
    {
        var logger = new MockLogger<AuthStore>();
        var authStore = new AuthStore(logger);
        authStore.IsAuthenticated.Should().BeFalse();
        authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" });
        authStore.Roles.Should().BeEquivalentTo("Role1", "Role2");
        authStore.Tenant.Name.Should().Be("TenantName");
        authStore.User.Firstname.Should().Be("firstName");
        authStore.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void UnauthenticatedShouldThrowOnValueAccess()
    {
        var logger = new MockLogger<AuthStore>();
        var authStore = new AuthStore(logger);
        authStore.IsAuthenticated.Should().BeFalse();
        Assert.Throws<NotAuthenticatedException>(() => authStore.Roles);
        Assert.Throws<NotAuthenticatedException>(() => authStore.User);
        Assert.Throws<NotAuthenticatedException>(() => authStore.Tenant);
    }

    [Fact]
    public void SetValuesTwiceShouldThrow()
    {
        var logger = new MockLogger<AuthStore>();
        var authStore = new AuthStore(logger);
        authStore.IsAuthenticated.Should().BeFalse();
        authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" });

        Assert.Throws<AlreadyAuthenticatedException>(() => authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Name = "TenantName" }, new[] { "Role1", "Role2" }));
    }

    [Fact]
    public void StartLogScopeShouldWork()
    {
        var logger = new MockLogger<AuthStore>();
        var authStore = new AuthStore(logger);
        var roles = new[] { "Role1", "Role2" };
        authStore.SetValues("mock-token", new() { Firstname = "firstName", Lastname = "lastName" }, new() { Id = "TenantId1", Name = "TenantName" }, roles);
        using (authStore.StartLogScope())
        {
            logger.ActiveScopes.Should().HaveCount(1);
            var loggerScope = logger.ActiveScopes.Cast<IReadOnlyDictionary<string, object>>().Single();
            loggerScope.Should().HaveCount(2);
            loggerScope.Should().Contain("TenantId", "TenantId1");
            loggerScope.TryGetValue("Roles", out var scopeRoles).Should().BeTrue();
            scopeRoles.Should().BeEquivalentTo(roles);
        }

        logger.ActiveScopes.Should().BeEmpty();
    }
}
