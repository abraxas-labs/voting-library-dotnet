// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Authorization;
using Voting.Lib.Iam.Configuration;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Services.ApiClient.Identity;
using Voting.Lib.Iam.Services.ApiClient.Permission;
using Voting.Lib.Iam.ServiceTokenHandling;
using Voting.Lib.Iam.Store;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for IAM.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all services provided by this library to the service collection
    /// and sets up the authentication using secure connect.
    /// The scheme needs to be added separately.
    /// Either call <see cref="AddSecureConnectScheme"/>
    /// or Voting.Lib.Iam.Testing.DependencyInjection.AuthenticationBuilderExtensions.AddMockedSecureConnectScheme.
    /// </summary>
    /// <param name="services">The ServiceCollection.</param>
    /// <param name="config">The secure connect api options.</param>
    /// <returns>The AuthenticationBuilder instance.</returns>
    public static AuthenticationBuilder AddVotingLibIam(this IServiceCollection services, SecureConnectApiOptions config)
    {
        return services
            .AddSingleton(sp => sp.GetRequiredService<IOptionsMonitor<SecureConnectServiceAccountOptions>>().CurrentValue)
            .AddSingleton<IPostConfigureOptions<SecureConnectOptions>, SecureConnectPostConfigureOptions>()
            .AddSingleton<IPostConfigureOptions<SecureConnectServiceAccountOptions>, SecureConnectServiceAccountPostConfigureOptions>()
            .AddSingleton<IServiceTokenHandlerFactory, DefaultServiceTokenHandlerFactory>()
            .AddTransient<IServiceTokenHandler, ServiceTokenHandler>()
            .AddTransient<ServiceTokenHttpMessageHandler>()
            .AddHttpClient<IRoleTokenHandler, RoleTokenHandler>().AddDefaultSecureConnectServiceToken().Services
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<ITenantService, TenantService>()
            .AddHttpClient<ISecureConnectPermissionServiceClient, SecureConnectPermissionServiceClient>(x => x.BaseAddress = config.PermissionUrl).AddDefaultSecureConnectServiceToken().Services
            .AddHttpClient<ISecureConnectIdentityServiceClient, SecureConnectIdentityServiceClient>(x => x.BaseAddress = config.IdentityUrl).AddDefaultSecureConnectServiceToken().Services
            .AddForwardRefScoped<IAuth, AuthStore>()
            .AddForwardRefScoped<IAuthStore, AuthStore>()
            .AddCache<User>(new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8) })
            .AddCache<Tenant>(new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8) })
            .AddCache<UserRoles>(new() { AbsoluteExpirationRelativeToNow = config.RoleTokenExpirationTime })
            .AddSecureConnectAuthorization()
            .AddSecureConnectAuthentication();
    }

    /// <summary>
    /// Configures the <see cref="SecureConnectAppHandlerConfig"/> as singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="appHeader">The app header value.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectAppHandler(this IServiceCollection services, string appHeader)
        => services.AddSecureConnectAppHandler(new SecureConnectAppHandlerConfig(appHeader));

    /// <summary>
    /// Configures the <see cref="SecureConnectAppHandlerConfig"/> as singleton and
    /// registers a scoped <see cref="SecureConnectAppHandler"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The config to add as singleton.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSecureConnectAppHandler(this IServiceCollection services, SecureConnectAppHandlerConfig config)
        => services
            .AddSingleton(config)
            .AddScoped<SecureConnectAppHandler>();

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
        => services.Configure(name, options);

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
        => services.Configure<SecureConnectServiceAccountOptions>(name, o => o.ApplyFrom(options));

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
        => services.Configure<SecureConnectServiceAccountOptions>(name, config);

    /// <summary>
    /// Adds the secure connect authentication scheme to the authentication builder.
    /// </summary>
    /// <param name="builder">The instance of the authentication builder.</param>
    /// <param name="configureOptions">Options for the handler.</param>
    /// <returns>Authentication builder instance.</returns>
    public static AuthenticationBuilder AddSecureConnectScheme(
        this AuthenticationBuilder builder,
        Action<SecureConnectOptions>? configureOptions = null)
    {
        configureOptions ??= _ => { };
        builder.AddScheme<SecureConnectOptions, SecureConnectHandler>(
            SecureConnectDefaults.AuthenticationScheme,
            configureOptions);

        // configure the default service account
        builder.Services.Configure<SecureConnectServiceAccountOptions>(Options.Options.DefaultName, o =>
        {
            var opts = new SecureConnectOptions();
            configureOptions(opts);
            o.Authority = opts.Authority;
            o.RequireHttpsMetadata = opts.RequireHttpsMetadata;
            o.MetadataAddress = opts.MetadataAddress;
            o.UserName = opts.ServiceAccount;
            o.Password = opts.ServiceAccountPassword;
            o.ClientIdScopes = opts.ServiceAccountScopes;
            o.RefreshBeforeExpiration = opts.ServiceTokenRefreshBeforeExpiration;
        });

        return builder;
    }

    /// <summary>
    /// Adds the SecureConnect authorization and requires authenticated users.
    /// </summary>
    /// <param name="services">The ServiceCollection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddSecureConnectAuthorization(this IServiceCollection services)
        => services.AddAuthorization()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddTransient<IAuthorizationHandler, PermissionHandler>()
            .AddTransient<IAuthorizationHandler, AnyPermissionHandler>();

    /// <summary>
    /// Adds the secure connect authentication scheme.
    /// </summary>
    /// <param name="services">the service collection.</param>
    /// <returns>Authentication builder instance.</returns>
    public static AuthenticationBuilder AddSecureConnectAuthentication(this IServiceCollection services)
        => services.AddAuthentication(SecureConnectDefaults.AuthenticationScheme);
}
