// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// <see cref="IHttpClientBuilder"/> extensions.
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Forwards the tenant to if none is present in the outgoing request.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="headerName">The name of the header used for the tenant.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder ForwardTenant(this IHttpClientBuilder builder, string headerName = "x-tenant")
    {
        builder.AddHttpMessageHandler(sp => new ForwardHeaderHandler(headerName, sp.GetRequiredService<IHttpContextAccessor>()));
        return builder;
    }

    /// <summary>
    /// Adds a fixed tenant to all requests if none is present.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="tenant">The tenant value.</param>
    /// <param name="headerName">The name of the header used for the tenant.</param>
    /// <returns>The updated builder.</returns>
    public static IHttpClientBuilder WithTenant(this IHttpClientBuilder builder, string tenant, string headerName = "x-tenant")
    {
        builder.AddHttpMessageHandler(() => new ConstantHeaderHandler(headerName, tenant));
        return builder;
    }
}
