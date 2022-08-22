// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Voting.Lib.Testing;

/// <summary>
/// Base test class which adds helper methods for testing and MS DI.
/// </summary>
/// <typeparam name="TFactory">the type of the test app factory.</typeparam>
/// <typeparam name="TStartup">the type of the startup class.</typeparam>
public abstract class BaseTest<TFactory, TStartup> : IClassFixture<TFactory>, IAsyncLifetime
    where TFactory : BaseTestApplicationFactory<TStartup>
    where TStartup : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTest{TFactory, TStartup}"/> class.
    /// </summary>
    /// <param name="factory">The test application factory.</param>
    protected BaseTest(BaseTestApplicationFactory<TStartup> factory)
        => Factory = factory;

    /// <summary>
    /// Gets the test app factory.
    /// </summary>
    protected BaseTestApplicationFactory<TStartup> Factory { get; }

    /// <summary>
    /// Called by the test framework to do async initialization work.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public virtual Task InitializeAsync() => Task.CompletedTask;

    /// <summary>
    /// Called by the test framework to do async dispose work.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public virtual Task DisposeAsync() => Task.CompletedTask;

    /// <summary>
    /// Creates a new grpc channel with a default authorized user and the given roles.
    /// </summary>
    /// <param name="roles">Roles, which the user of this channel should have assigned.</param>
    /// <returns>The created grpc channel.</returns>
    protected virtual GrpcChannel CreateGrpcChannel(params string[] roles)
        => CreateGrpcChannel(true, roles: roles);

    /// <summary>
    /// Creates a new grpc channel.
    /// </summary>
    /// <param name="authorize">If true the user is authorized.</param>
    /// <param name="tenant">The tenant id to be used.</param>
    /// <param name="userId">The user id to be used.</param>
    /// <param name="roles">The roles, which should be assigned to this session/user.</param>
    /// <returns>The created grpc channel.</returns>
    protected virtual GrpcChannel CreateGrpcChannel(
        bool authorize = true,
        string? tenant = TestDefaults.TenantId,
        string? userId = TestDefaults.UserId,
        params string[] roles) => Factory.CreateGrpcChannel(authorize, tenant, userId, roles);

    /// <summary>
    /// Creates a new http client with a default authorized user and the given roles.
    /// </summary>
    /// <param name="roles">Roles, which the user of this client should have assigned.</param>
    /// <returns>The created http client.</returns>
    protected virtual HttpClient CreateHttpClient(params string[] roles)
        => CreateHttpClient(true, roles: roles);

    /// <summary>
    /// Creates a new http client.
    /// </summary>
    /// <param name="authorize">If true the user is authorized.</param>
    /// <param name="tenant">The tenant id to be used.</param>
    /// <param name="userId">The user id to be used.</param>
    /// <param name="roles">The roles, which should be assigned to this session/user.</param>
    /// <returns>The created http client.</returns>
    protected virtual HttpClient CreateHttpClient(
        bool authorize = true,
        string? tenant = TestDefaults.TenantId,
        string? userId = TestDefaults.UserId,
        params string[] roles) => Factory.CreateHttpClient(authorize, tenant, userId, roles);

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/> of the current test.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
    protected T GetService<T>()
        where T : notnull
        => Factory.Services.GetRequiredService<T>();

    /// <summary>
    /// Runs an action in a scope by providing a service.
    /// </summary>
    /// <param name="action">The action to be run.</param>
    /// <typeparam name="TService">The service to be resolved.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task RunScoped<TService>(Func<TService, Task> action)
        where TService : notnull
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        await action(service).ConfigureAwait(false);
    }

    /// <summary>
    /// Runs a function in a scope by providing a service.
    /// </summary>
    /// <param name="action">The function to be run.</param>
    /// <typeparam name="TService">The service to be resolved.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation with the result.</returns>
    protected async Task<TResult> RunScoped<TService, TResult>(Func<TService, Task<TResult>> action)
        where TService : notnull
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return await action(service).ConfigureAwait(false);
    }

    /// <summary>
    /// Runs a function in a scope by providing a service.
    /// </summary>
    /// <param name="action">The function to be run.</param>
    /// <typeparam name="TService">The service to be resolved.</typeparam>
    protected void RunScoped<TService>(Action<TService> action)
        where TService : notnull
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        action(service);
    }

    /// <summary>
    /// Runs a function in a scope by providing a service.
    /// </summary>
    /// <param name="action">The function to be run.</param>
    /// <typeparam name="TService">The service to be resolved.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>The result.</returns>
    protected TResult RunScoped<TService, TResult>(Func<TService, TResult> action)
        where TService : notnull
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return action(service);
    }
}
