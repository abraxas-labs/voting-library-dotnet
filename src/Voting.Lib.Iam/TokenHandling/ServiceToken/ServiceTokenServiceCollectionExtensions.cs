// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for service tokens.
/// </summary>
public static class ServiceTokenServiceCollectionExtensions
{
    /// <summary>
    /// Configures secure connect service token handling.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectServiceTokenHandling(this IServiceCollection services)
    {
        services.TryAddSingleton<IPostConfigureOptions<SecureConnectServiceAccountOptions>, SecureConnectServiceAccountPostConfigureOptions>();
        services.TryAddSingleton<IServiceTokenHandlerFactory, DefaultServiceTokenHandlerFactory>();
        services.TryAddTransient<ITokenHandler, ServiceTokenHandler>();
        return services;
    }

    /// <summary>
    /// Configures a service account based on a <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="name">The name of the config.</param>
    /// <param name="options">The options builder.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectServiceAccount(
        this IServiceCollection services,
        string name,
        Action<SecureConnectServiceAccountOptions> options)
    {
        services.AddSecureConnectServiceTokenHandling();
        return services.Configure(name, options);
    }

    /// <summary>
    /// Configures a service account based on a <see cref="SecureConnectServiceAccountOptions"/> object.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="name">The name of the config.</param>
    /// <param name="options">The options.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectServiceAccount(
        this IServiceCollection services,
        string name,
        SecureConnectServiceAccountOptions options)
    {
        services.AddSecureConnectServiceTokenHandling();
        return services.Configure<SecureConnectServiceAccountOptions>(name, o => o.ApplyFrom(options));
    }

    /// <summary>
    /// Configures a service account based on a <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="name">The name of the config.</param>
    /// <param name="config">The config.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectServiceAccount(
        this IServiceCollection services,
        string name,
        IConfiguration config)
    {
        services.AddSecureConnectServiceTokenHandling();
        return services.Configure<SecureConnectServiceAccountOptions>(name, config);
    }
}
