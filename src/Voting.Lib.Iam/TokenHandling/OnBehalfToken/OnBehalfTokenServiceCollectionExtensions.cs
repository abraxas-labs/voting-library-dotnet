// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.TokenHandling.OnBehalfToken;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for IAM.
/// </summary>
public static class OnBehalfTokenServiceCollectionExtensions
{
    /// <summary>
    /// Configures secure connect on behalf token handling.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectOnBehalfTokenHandling(this IServiceCollection services)
    {
        services.TryAddSingleton<IOnBehalfTokenHandlerFactory, DefaultOnBehalfTokenHandlerFactory>();
        services.TryAddSingleton<IPostConfigureOptions<SecureConnectOnBehalfOptions>, SecureConnectOnBehalfPostConfigureOptions>();
        services.AddHttpContextAccessor();
        return services;
    }

    /// <summary>
    /// Configures an on-behalf token based on a <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="name">The name of the config.</param>
    /// <param name="options">The options builder.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectOnBehalfToken(
        this IServiceCollection services,
        string name,
        Action<SecureConnectOnBehalfOptions> options)
    {
        services.AddSecureConnectOnBehalfTokenHandling();
        return services.Configure(name, options);
    }

    /// <summary>
    /// Configures an on-behalf token based on a <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="name">The name of the config.</param>
    /// <param name="options">The options builder.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectOnBehalfToken(
        this IServiceCollection services,
        string name,
        SecureConnectOnBehalfOptions options)
    {
        return services.AddSecureConnectOnBehalfToken(name, o => o.ApplyFrom(options));
    }
}
