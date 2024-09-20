// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Store;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Iam.Test.Store;

public class AuthExtensionsTest
{
    private readonly AuthStore _auth;

    public AuthExtensionsTest()
    {
        _auth = new AuthStore(new MockLogger<AuthStore>());
        _auth.SetValues(
            "mock-token",
            new() { Firstname = "firstName", Lastname = "lastName" },
            new() { Name = "TenantName" },
            new[] { "Role1", "Role2" },
            new[] { "Permission1" });
    }

    [Fact]
    public void HasRoleShouldWork()
    {
        _auth.HasRole("Role1").Should().BeTrue();
        _auth.HasRole("Role0").Should().BeFalse();
    }

    [Fact]
    public void HasAnyRoleShouldWork()
    {
        _auth.HasAnyRole("Role0", "Role1").Should().BeTrue();
        _auth.HasAnyRole("Role0", "Role3").Should().BeFalse();
    }

    [Fact]
    public void EnsureRoleShouldWork()
    {
        _auth.EnsureRole("Role1");
        Assert.Throws<ForbiddenException>(() => _auth.EnsureRole("Role0"));
    }

    [Fact]
    public void EnsureAnyRoleShouldWork()
    {
        _auth.EnsureAnyRole("Role0", "Role1");
        Assert.Throws<ForbiddenException>(() => _auth.EnsureAnyRole("Role0", "Role3"));
    }

    [Fact]
    public void HasPermissionShouldWork()
    {
        _auth.HasPermission("Permission1").Should().BeTrue();
        _auth.HasPermission("Permission5").Should().BeFalse();
    }

    [Fact]
    public void EnsurePermissionShouldWork()
    {
        _auth.EnsurePermission("Permission1");
        Assert.Throws<ForbiddenException>(() => _auth.EnsurePermission("Permission5"));
    }
}
