// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for the <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class ServiceTokenHttpClientBuilderExtensions
{
    private static readonly ObjectFactory HandlerFactory =
        ActivatorUtilities.CreateFactory(typeof(TokenHttpMessageHandler), [typeof(ITokenHandler)]);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds service token to requests.
    /// Uses the default configuration.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddDefaultSecureConnectServiceToken(this IHttpClientBuilder builder)
        => builder.AddSecureConnectServiceToken(Options.Options.DefaultName);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds service token to requests.
    /// Uses the http client name as the configuration name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectServiceToken(this IHttpClientBuilder builder)
        => builder.AddSecureConnectServiceToken(builder.Name);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds service token to requests.
    /// Uses the provided configuration name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configName">The name of the <see cref="SecureConnectServiceAccountOptions"/> to use.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectServiceToken(this IHttpClientBuilder builder, string configName)
        => builder.AddHttpMessageHandler(sp => BuildMessageHandler(sp, configName));

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds service token to requests.
    /// Uses the http client name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The service account options configurer.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectServiceToken(
        this IHttpClientBuilder builder,
        Action<SecureConnectServiceAccountOptions> options)
    {
        builder.Services.AddSecureConnectServiceAccount(builder.Name, options);
        return builder.AddSecureConnectServiceToken();
    }

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds service token to requests.
    /// Uses the http client name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The service account options.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectServiceToken(
        this IHttpClientBuilder builder,
        SecureConnectServiceAccountOptions options)
    {
        return builder.AddSecureConnectServiceToken(o => o.ApplyFrom(options));
    }

    /// <summary>
    /// Adds the <see cref="SecureConnectAppHandler"/> which adds app header name and value to the request headers.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectAppHandler(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler<SecureConnectAppHandler>();

    private static TokenHttpMessageHandler BuildMessageHandler(
        IServiceProvider serviceProvider,
        string configName)
    {
        var serviceTokenHandlerFactory = serviceProvider.GetRequiredService<IServiceTokenHandlerFactory>();
        var handler = serviceTokenHandlerFactory.CreateHandler(configName);
        return (TokenHttpMessageHandler)HandlerFactory(serviceProvider, [handler]);
    }
}
