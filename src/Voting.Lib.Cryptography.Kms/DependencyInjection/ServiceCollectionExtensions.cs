// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Voting.Lib.Cryptography;
using Voting.Lib.Cryptography.Kms;
using Voting.Lib.Cryptography.Kms.Auth;
using Voting.Lib.Cryptography.Kms.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for cryptography.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a KMS (key management system) crypto provider implementation
    /// for the THALES CipherTrust API.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration of the pkcs11 interface.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddVotingLibKms(this IServiceCollection services, KmsConfig config)
    {
        services
            .AddSystemTimeProvider()
            .AddSingleton(config)
            .AddScoped<AuthHandler>()
            .AddSingleton<TokenProvider>()
            .AddHttpClient<TokenClient>(x => SetupHttpClient(x, config)).Services
            .AddHttpClient<ICryptoProvider, KmsCryptoProvider>(x => SetupHttpClient(x, config))
            .AddHttpMessageHandler<AuthHandler>();
        return services;
    }

    private static void SetupHttpClient(HttpClient client, KmsConfig config)
    {
        client.BaseAddress = config.Endpoint ?? throw new InvalidOperationException("KMS endpoint not set.");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
