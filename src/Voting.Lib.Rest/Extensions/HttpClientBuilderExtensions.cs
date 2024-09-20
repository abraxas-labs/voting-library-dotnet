// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Rest;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Propagates all well known abraxas header names.
    /// <seealso cref="AbraxasHeaderNames"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder instance.</returns>
    public static IHttpClientBuilder AddAbraxasHeaderPropagation(this IHttpClientBuilder builder)
    {
        return builder.AddHeaderPropagation(opts =>
        {
            foreach (var headerName in AbraxasHeaderNames.All)
            {
                opts.Headers.Add(headerName);
            }
        });
    }
}
