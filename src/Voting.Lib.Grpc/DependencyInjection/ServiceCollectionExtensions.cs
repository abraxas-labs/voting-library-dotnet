﻿// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Voting.Lib.Grpc.Interceptors;

namespace Voting.Lib.Grpc.DependencyInjection;

/// <summary>
/// Service collection extensions for Voting.Lib.Grpc.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="RequestLoggerInterceptor"/> to the InterceptorCollection,
    /// if the web host environment is development.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="environment">The web host environment.</param>
    /// <returns>The IServiceCollectionReference.</returns>
    public static IServiceCollection AddGrpcRequestLoggerInterceptor(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddGrpc(o => o.Interceptors.Add<RequestLoggerInterceptor>());
        }

        return services;
    }
}
