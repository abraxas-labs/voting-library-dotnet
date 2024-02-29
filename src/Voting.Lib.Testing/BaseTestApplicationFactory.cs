// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Voting.Lib.Iam.Testing.AuthenticationScheme;
using Voting.Lib.Testing.Utils;

namespace Voting.Lib.Testing;

/// <summary>
/// The base test application factory for tests.
/// </summary>
/// <typeparam name="TStartup">The type of startup to use.</typeparam>
public class BaseTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    private readonly int _port = NetworkUtil.FindFreePort();

    /// <summary>
    /// Creates a new grpc channel.
    /// </summary>
    /// <param name="authorize">If true the user is authorized.</param>
    /// <param name="tenant">The tenant id to be used.</param>
    /// <param name="userId">The user id to be used.</param>
    /// <param name="roles">The roles, which should be assigned to this session/user.</param>
    /// <returns>The created grpc channel.</returns>
    public GrpcChannel CreateGrpcChannel(
        bool authorize,
        string? tenant,
        string? userId,
        string[] roles)
    {
        var httpClient = CreateHttpClient(authorize, tenant, userId, roles);

        return GrpcChannel.ForAddress(
            $"http://localhost:{_port}",
            new GrpcChannelOptions { HttpClient = httpClient });
    }

    /// <summary>
    /// Creates a new http client.
    /// </summary>
    /// <param name="authorize">If true the user is authorized.</param>
    /// <param name="tenant">The tenant id to be used.</param>
    /// <param name="userId">The user id to be used.</param>
    /// <param name="roles">The roles, which should be assigned to this session/user.</param>
    /// <returns>The created http client.</returns>
    public virtual HttpClient CreateHttpClient(
        bool authorize,
        string? tenant,
        string? userId,
        string[]? roles)
    {
        // The ResponseVersionHandler class is needed because of the test server not being
        // able to deliver http2: https://github.com/grpc/grpc-dotnet/issues/648
        var httpClient = CreateDefaultClient(new Uri($"http://localhost:{_port}"), new ResponseVersionHandler());

        if (authorize)
        {
            httpClient.DefaultRequestHeaders.Add(SecureConnectTestDefaults.AuthHeader, "true");
        }

        if (tenant != null)
        {
            httpClient.DefaultRequestHeaders.Add(SecureConnectTestDefaults.TenantHeader, tenant);
        }

        if (userId != null)
        {
            httpClient.DefaultRequestHeaders.Add(SecureConnectTestDefaults.UserHeader, userId);
        }

        if (roles != null)
        {
            httpClient.DefaultRequestHeaders.Add(SecureConnectTestDefaults.RolesHeader, roles);
        }

        return httpClient;
    }

    /// <inheritdoc />
    protected override IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder
                    .UseStartup<TStartup>()
                    .ConfigureKestrel(
                        o => o.ListenAnyIP(_port, listen => listen.Protocols = HttpProtocols.Http1AndHttp2)));

    private class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            response.Version = request.Version;

            return response;
        }
    }
}
