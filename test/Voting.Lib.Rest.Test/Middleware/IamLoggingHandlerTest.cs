// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Configuration;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Store;
using Voting.Lib.Rest.Middleware;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Rest.Test.Middleware;

public class IamLoggingHandlerTest
{
    [Fact]
    public async Task ShouldStartAndDisposeLogScope()
    {
        var loggerProvider = new MockLoggerProvider();
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services
            .AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddProvider(loggerProvider);
            })
            .AddSingleton(new AuthStoreConfig())
            .AddForwardRefScoped<IAuthStore, AuthStore>();

        await using var app = builder.Build();
        app.Use((ctx, next) =>
        {
            ctx.RequestServices.GetRequiredService<AuthStore>().SetValues("mock-token", new User(), new Tenant { Id = "123" }, new[] { "Role1" });
            return next(ctx);
        });
        app.UseMiddleware<IamLoggingHandler>();

        var responseStartedTaskCompletionSource = new TaskCompletionSource();
        var responseCompletedTaskCompletionSource = new TaskCompletionSource<string>();
        app.MapGet("/test", () =>
        {
            responseStartedTaskCompletionSource.SetResult();
            return responseCompletedTaskCompletionSource.Task;
        });

        await app.StartAsync();

        loggerProvider.GetSingleLogger(typeof(AuthStore).FullName!).Should().BeNull();
        var responseTask = app.GetTestClient().GetStringAsync("/test");
        await responseStartedTaskCompletionSource.Task;

        var logger = loggerProvider.GetSingleLogger(typeof(AuthStore).FullName!);
        logger.Should().NotBeNull();
        logger!.ActiveScopes.Should().HaveCount(1);

        var loggerScope = logger.ActiveScopes.OfType<Dictionary<string, object>>().Single();
        loggerScope.Should().Contain("TenantId", "123");

        responseCompletedTaskCompletionSource.SetResult("ok");
        var response = await responseTask;
        response.Should().Be("ok");

        logger.ActiveScopes.Should().BeEmpty();
    }
}
