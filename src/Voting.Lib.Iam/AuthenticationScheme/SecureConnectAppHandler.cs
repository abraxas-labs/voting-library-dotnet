// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Iam.Configuration;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <summary>
/// A <see cref="DelegatingHandler"/> which attaches the application header to the request headers.
/// </summary>
public class SecureConnectAppHandler : DelegatingHandler
{
    private readonly SecureConnectAppHandlerConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureConnectAppHandler"/> class.
    /// </summary>
    /// <param name="config">The configuration for the IAM app handler.</param>
    public SecureConnectAppHandler(SecureConnectAppHandlerConfig config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AttachAppHeader(request.Headers);
        return await base.SendAsync(request, cancellationToken);
    }

    private void AttachAppHeader(HttpHeaders headers)
        => headers.Add(_config.AppHeaderName, _config.AppHeader);
}
