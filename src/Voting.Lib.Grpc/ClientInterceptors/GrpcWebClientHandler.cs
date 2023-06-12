// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Grpc.ClientInterceptors;

internal class GrpcWebClientHandler : DelegatingHandler
{
    private const string GrpcWebMarkerHeaderName = "X-Grpc-Web";
    private const string GrpcWebMarkerHeaderValue = "1";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(GrpcWebMarkerHeaderName, GrpcWebMarkerHeaderValue);
        return await base.SendAsync(request, cancellationToken);
    }
}
