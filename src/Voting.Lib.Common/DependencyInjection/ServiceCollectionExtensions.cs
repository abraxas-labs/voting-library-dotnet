// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Voting.Lib.Common;
using Voting.Lib.Common.Cache;
using Voting.Lib.Common.Net;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for Voting.Lib.Common.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The key of the cors configuration section.
    /// </summary>
    public const string CorsConfigSectionKey = "Cors";

    /// <summary>
    /// Adds the system clock to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddSystemClock(this IServiceCollection services)
    {
        services.TryAddSingleton<IClock, SystemClock>();
        return services;
    }

    /// <summary>
    /// Adds certificate pinning to the service collection
    /// and all created <see cref="HttpClient"/> which are created via the "DefaultHttpMessageHandlerBuilder.Build"
    /// (this includes clients created via the <see cref="IHttpClientFactory"/> contract implemented by "DefaultHttpClientFactory.CreateClient".
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The certificate pinning options.</param>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddCertificatePinning(this IServiceCollection services, CertificatePinningConfig config)
    {
        services.AddHttpClient();
        services.TryAddSingleton(config);
        services.TryAddSingleton<CertificatePinningHandler>();
        services.AddTransient<IHttpMessageHandlerBuilderFilter, CertificatePinningHandlerBuilder>();
        return services;
    }

    /// <summary>
    /// Adds a cache singleton for the specified entry type.
    /// </summary>
    /// <typeparam name="T">Cache entry type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="cacheOptions">The cache options.</param>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddCache<T>(this IServiceCollection services, CacheOptions<T> cacheOptions)
    {
        return services
            .AddMemoryCache()
            .AddSingleton<ICache<T>, Cache<T>>()
            .AddSingleton(cacheOptions);
    }

    /// <summary>
    /// Adds CORS services from <see cref="CorsServiceCollectionExtensions"/>
    /// and registers an <see cref="IOptions{T}"/> instance of type <see cref="CorsConfig"/>
    /// where the appsettings config section with the corresponding <see cref="CorsConfigSectionKey"/> will bind against.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="configuration">The Configuration.</param>
    /// <returns>The IServiceCollectionReference.</returns>
    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors<CorsConfig>(configuration);
        return services;
    }

    /// <summary>
    /// Adds CORS services from <see cref="CorsServiceCollectionExtensions"/>
    /// and registers an <see cref="IOptions{T}"/> instance of passed type TCorsConfig
    /// where the appsettings config section with the corresponding <see cref="CorsConfigSectionKey"/> will bind against.
    /// Use this only if the regular <see cref="CorsConfig"/> needs to be extended.
    /// </summary>
    /// <typeparam name="TCorsConfig">The concrete type of the configuration options to bind to.</typeparam>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="configuration">The Configuration.</param>
    /// <returns>The IServiceCollectionReference.</returns>
    public static IServiceCollection AddCors<TCorsConfig>(this IServiceCollection services, IConfiguration configuration)
        where TCorsConfig : CorsConfig
    {
        services.AddCors();
        services.Configure<TCorsConfig>(configuration.GetSection(CorsConfigSectionKey));
        return services;
    }

    /// <summary>
    /// Adds a service to the service collection as a singleton and adds a forwarding interface reference to the same singleton instance.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddForwardRefSingleton<TService, TImpl>(this IServiceCollection services)
        where TService : class
        where TImpl : class, TService
    {
        services.TryAddSingleton<TImpl>();
        services.AddSingleton<TService>(sp => sp.GetRequiredService<TImpl>());
        return services;
    }

    /// <summary>
    /// Adds a service to the service collection as a scoped and adds a forwarding interface reference to the same scoped instance.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddForwardRefScoped<TService, TImpl>(this IServiceCollection services)
        where TService : class
        where TImpl : class, TService
    {
        services.TryAddScoped<TImpl>();
        services.AddScoped<TService>(sp => sp.GetRequiredService<TImpl>());
        return services;
    }

    /// <summary>
    /// Adds an object pool to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="activator">An activator function to create new instances.</param>
    /// <param name="reset">A reset function to reset an object instance.</param>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddObjectPool<T>(this IServiceCollection services, Func<T> activator, Action<T> reset)
        where T : class
        => services.AddObjectPool(new LambdaPooledObjectPolicy<T>(activator, reset));

    /// <summary>
    /// Adds an object pool with a default policy to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddObjectPool<T>(this IServiceCollection services)
        where T : class, new()
        => services.AddObjectPool(new DefaultPooledObjectPolicy<T>());

    /// <summary>
    /// Adds an object pool to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="policy">The policy used to activate and reset object instances.</param>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddObjectPool<T>(this IServiceCollection services, IPooledObjectPolicy<T> policy)
        where T : class
    {
        services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        services.TryAddSingleton(policy);
        services.TryAddSingleton(static sp =>
        {
            var provider = sp.GetRequiredService<ObjectPoolProvider>();
            var policy = sp.GetRequiredService<IPooledObjectPolicy<T>>();
            return provider.Create(policy);
        });
        return services;
    }

    /// <summary>
    /// Adds a <see cref="HashBuilder"/> pool.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="hashAlgorithmName">The hash algorithm name.</param>
    /// <returns>The same service collection instance.</returns>
    public static IServiceCollection AddHashBuilderPool(this IServiceCollection services, HashAlgorithmName hashAlgorithmName)
        => services.AddObjectPool(() => new HashBuilder(hashAlgorithmName), static x => x.GetHashAndReset());
}
