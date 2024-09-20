// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Grpc.ClientInterceptors;

/// <summary>
/// Delegation handler to add a path prefix to the request uri.
/// </summary>
public class GrpcPathPrefixHandler : DelegatingHandler
{
    private readonly string _pathPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="GrpcPathPrefixHandler"/> class.
    /// </summary>
    /// <param name="pathPrefix">The path prefix to add to the request uri.</param>
    public GrpcPathPrefixHandler(string pathPrefix)
    {
        _pathPrefix = pathPrefix;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_pathPrefix))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var originalUri = request.RequestUri
            ?? throw new UriFormatException("The request uri must not be null");

        var modifiedUriBuilder = new UriBuilder(originalUri)
        {
            Path = _pathPrefix + originalUri.PathAndQuery,
        };

        request.RequestUri = modifiedUriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}
