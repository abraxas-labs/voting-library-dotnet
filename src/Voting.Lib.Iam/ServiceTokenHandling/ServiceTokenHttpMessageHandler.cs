// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;

namespace Voting.Lib.Iam.ServiceTokenHandling;

/// <summary>
/// If no authorization header is present, the service token is added.
/// Currently only implemented for async operations.
/// </summary>
public class ServiceTokenHttpMessageHandler : DelegatingHandler
{
    private readonly IServiceTokenHandler _handler;
    private readonly ILogger<ServiceTokenHttpMessageHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceTokenHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="handler">The service token handler.</param>
    /// <param name="logger">The logger.</param>
    public ServiceTokenHttpMessageHandler(IServiceTokenHandler handler, ILogger<ServiceTokenHttpMessageHandler> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogWarning(SecurityLogging.SecurityEventId, "The service token HTTP message handler does not support sync operations");
        return base.Send(request, cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null)
        {
            var serviceToken = await _handler.GetServiceToken().ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceToken);
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Added service token to request");
        }
        else
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Request authorization was already set, did not add service token to request");
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
