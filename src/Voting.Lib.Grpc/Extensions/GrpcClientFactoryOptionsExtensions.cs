// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Grpc.Core.Interceptors;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Voting.Lib.Grpc.Extensions;

/// <summary>
/// Extension methods for <see cref="GrpcClientFactoryOptions"/>.
/// </summary>
public static class GrpcClientFactoryOptionsExtensions
{
    /// <summary>
    /// Adds a new client scoped <see cref="InterceptorRegistration"/>
    /// which resolves the interceptor via <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="options">The options to add the interceptor to.</param>
    /// <typeparam name="T">The type of the interceptor.</typeparam>
    public static void AddClientInterceptor<T>(this GrpcClientFactoryOptions options)
        where T : Interceptor
        => options.AddInterceptor<T>(InterceptorScope.Client);

    /// <summary>
    /// Adds a new channel scoped <see cref="InterceptorRegistration"/>
    /// which resolves the interceptor via <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="options">The options to add the interceptor to.</param>
    /// <typeparam name="T">The type of the interceptor.</typeparam>
    public static void AddChannelInterceptor<T>(this GrpcClientFactoryOptions options)
        where T : Interceptor
        => options.AddInterceptor<T>(InterceptorScope.Channel);

    /// <summary>
    /// Adds a new <see cref="InterceptorRegistration"/>
    /// which resolves the interceptor via <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="options">The options to add the interceptor to.</param>
    /// <param name="scope">The scope of the interceptor.</param>
    /// <typeparam name="T">The type of the interceptor.</typeparam>
    public static void AddInterceptor<T>(this GrpcClientFactoryOptions options, InterceptorScope scope)
        where T : Interceptor
    {
        options.InterceptorRegistrations.Add(new InterceptorRegistration(scope, sp => sp.GetRequiredService<T>()));
    }
}
