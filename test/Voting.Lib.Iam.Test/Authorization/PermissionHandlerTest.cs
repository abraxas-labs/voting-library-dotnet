// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Iam.Authorization;
using Voting.Lib.Iam.Store;
using Xunit;

namespace Voting.Lib.Iam.Test.Authorization;

public class PermissionHandlerTest : IAsyncDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly AuthStore _authStore;

    public PermissionHandlerTest()
    {
        _authStore = new AuthStore(NullLogger<AuthStore>.Instance);

        _serviceProvider = new ServiceCollection()
            .AddAuthorization()
            .AddLogging()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddTransient<IAuthorizationHandler, PermissionHandler>()
            .AddTransient<IAuthorizationHandler, AnyPermissionHandler>()
            .AddSingleton<IAuthStore>(_authStore)
            .AddSingleton<IAuth>(_authStore)
            .BuildServiceProvider(true);
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task ShouldWorkWithMatchingPermission()
    {
        var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();
        _authStore.SetValues("token", "user-id", "tenant-id", new[] { "my-role" }, new[] { "my-permission" });
        var result = await authorizationService.AuthorizeAsync(new ClaimsPrincipal(), "Permission.my-permission");
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldFailOnMissingPermissions()
    {
        var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();
        _authStore.SetValues("token", "user-id", "tenant-id", new[] { "my-role" });
        var result = await authorizationService.AuthorizeAsync(new ClaimsPrincipal(), "Permission.my-permission");
        result.Succeeded.Should().BeFalse();
    }
}
