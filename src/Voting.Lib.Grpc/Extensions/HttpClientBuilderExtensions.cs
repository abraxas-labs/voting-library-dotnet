// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using System.Net.Http;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Grpc.ClientInterceptors;
using Voting.Lib.Grpc.Configuration;

namespace Voting.Lib.Grpc.Extensions;

/// <summary>
/// gRPC related extensions for the <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Configures the primary http message handler depending on the given <see cref="GrpcMode"/>.
    /// Adds the x-grpc-web header if a grpc web mode is provided.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="grpcMode">The grpc mode.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder ConfigureGrpcPrimaryHttpMessageHandler(this IHttpClientBuilder builder, GrpcMode grpcMode)
    {
        builder = builder.ConfigurePrimaryHttpMessageHandler(() => CreateHandler(grpcMode));
        if (grpcMode != GrpcMode.Grpc)
        {
            builder = builder.AddHttpMessageHandler(_ => new GrpcWebClientHandler());
        }

        return builder;
    }

    /// <summary>
    /// Adds the <see cref="GrpcPathPrefixHandler"/> configured with the given path prefix.
    /// </summary>
    /// <param name="builder">The http client builder.</param>
    /// <param name="pathPrefix">The path prefix to add to the request uri.</param>
    /// <returns>The updated http client builder.</returns>
    public static IHttpClientBuilder ConfigureGrpcPathPrefixHandler(this IHttpClientBuilder builder, string pathPrefix)
    {
        return builder.AddHttpMessageHandler(_ => new GrpcPathPrefixHandler(pathPrefix));
    }

    /// <summary>
    /// Configures pass through authentication.
    /// Adds the info from <see cref="Voting.Lib.Iam.Store.IAuth"/> to the call.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="customizer">The options customizer.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder PassThroughUserTokenAndAuthInfo(this IHttpClientBuilder builder, Action<PassThroughCallCredentialsConfig> customizer)
    {
        var config = new PassThroughCallCredentialsConfig();
        customizer(config);
        return PassThroughUserTokenAndAuthInfo(builder, config);
    }

    /// <summary>
    /// Configures pass through authentication.
    /// Adds the info from <see cref="Voting.Lib.Iam.Store.IAuth"/> to the call.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder PassThroughUserTokenAndAuthInfo(this IHttpClientBuilder builder)
        => PassThroughUserTokenAndAuthInfo(builder, new PassThroughCallCredentialsConfig());

    /// <summary>
    /// Configures pass through authentication.
    /// Adds the info from <see cref="Voting.Lib.Iam.Store.IAuth"/> to the call.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="config">The configuration.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder PassThroughUserTokenAndAuthInfo(
        this IHttpClientBuilder builder,
        PassThroughCallCredentialsConfig config)
    {
        var handler = new PassThroughCallCredentialsHandler(config);
        return builder.AddCallCredentials(handler.Handle);
    }

    private static HttpMessageHandler CreateHandler(GrpcMode mode)
    {
        return mode switch
        {
            GrpcMode.Grpc => new HttpClientHandler(),
            GrpcMode.GrpcWeb => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()),
            GrpcMode.GrpcWebText => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, $"{mode} is unknown"),
        };
    }
}
