// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.OnBehalfToken;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for the <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class OnBehalfTokenHttpClientBuilderExtensions
{
    private static readonly ObjectFactory HandlerFactory =
        ActivatorUtilities.CreateFactory(typeof(TokenHttpMessageHandler), [typeof(ITokenHandler)]);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the default configuration and service account.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddDefaultSecureConnectOnBehalfToken(this IHttpClientBuilder builder)
        => builder.AddSecureConnectOnBehalfToken(Options.Options.DefaultName);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the http client name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(this IHttpClientBuilder builder)
        => builder.AddSecureConnectOnBehalfToken(builder.Name);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the provided configuration name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configName">The name of the <see cref="SecureConnectServiceAccountOptions"/> to use.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(this IHttpClientBuilder builder, string configName)
        => builder.AddSecureConnectOnBehalfToken(configName, configName);

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the provided configuration name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configName">The name of the <see cref="SecureConnectOnBehalfOptions"/> to use.</param>
    /// <param name="serviceAccountConfigName">The name of the <see cref="SecureConnectServiceAccountOptions"/> to use.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(this IHttpClientBuilder builder, string configName, string serviceAccountConfigName)
    {
        var tokenClientName = builder.Name + SecureConnectOnBehalfOptions.TokenClientSuffix;
        builder.Services.AddSecureConnectOnBehalfTokenHandling();
        builder.Services.AddHttpClient(tokenClientName).AddSecureConnectServiceToken(serviceAccountConfigName);
        return builder.AddHttpMessageHandler(sp => BuildMessageHandler(sp, configName, serviceAccountConfigName, tokenClientName));
    }

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the http client name for the config name.
    /// Uses the default service account.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The on-behalf options configure.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(
        this IHttpClientBuilder builder,
        Action<SecureConnectOnBehalfOptions> options)
    {
        builder.Services.AddSecureConnectOnBehalfToken(builder.Name, options);
        return builder.AddSecureConnectOnBehalfToken(builder.Name, Options.Options.DefaultName);
    }

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the http client name for the config name.
    /// Uses the default service account.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The on-behalf options configure.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(
        this IHttpClientBuilder builder,
        SecureConnectOnBehalfOptions options)
    {
        return builder.AddSecureConnectOnBehalfToken(o => o.ApplyFrom(options));
    }

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the http client name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The on-behalf options configure.</param>
    /// <param name="serviceAccountOptions">The service account options to use to resolve the on-behalf token.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(
        this IHttpClientBuilder builder,
        Action<SecureConnectOnBehalfOptions> options,
        Action<SecureConnectServiceAccountOptions> serviceAccountOptions)
    {
        builder.Services.AddSecureConnectOnBehalfToken(builder.Name, options);
        builder.Services.AddSecureConnectServiceAccount(builder.Name, serviceAccountOptions);
        return builder.AddSecureConnectOnBehalfToken();
    }

    /// <summary>
    /// Adds the <see cref="TokenHttpMessageHandler"/> which adds on behalf token to requests.
    /// Uses the http client name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="options">The on-behalf options.</param>
    /// <param name="serviceAccountOptions">The service account options to use to resolve the on-behalf token.</param>
    /// <returns>The same builder.</returns>
    public static IHttpClientBuilder AddSecureConnectOnBehalfToken(
        this IHttpClientBuilder builder,
        SecureConnectOnBehalfOptions options,
        SecureConnectServiceAccountOptions serviceAccountOptions)
    {
        return builder.AddSecureConnectOnBehalfToken(
            o => o.ApplyFrom(options),
            o => o.ApplyFrom(serviceAccountOptions));
    }

    private static TokenHttpMessageHandler BuildMessageHandler(
        IServiceProvider serviceProvider,
        string configName,
        string serviceAccountConfigName,
        string clientName)
    {
        var onBehalfTokenHandlerFactory = serviceProvider.GetRequiredService<IOnBehalfTokenHandlerFactory>();
        var onBehalfTokenHandler = onBehalfTokenHandlerFactory.CreateHandler(configName, serviceAccountConfigName, clientName);
        return (TokenHttpMessageHandler)HandlerFactory(serviceProvider, [onBehalfTokenHandler]);
    }
}
