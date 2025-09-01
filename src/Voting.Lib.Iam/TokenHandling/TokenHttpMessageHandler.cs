// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;

namespace Voting.Lib.Iam.TokenHandling;

/// <summary>
/// If no authorization header is present, the token is added.
/// Currently only implemented for async operations.
/// </summary>
public class TokenHttpMessageHandler : DelegatingHandler
{
    private readonly ITokenHandler _handler;
    private readonly ILogger<TokenHttpMessageHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="handler">The token handler.</param>
    /// <param name="logger">The logger.</param>
    public TokenHttpMessageHandler(ITokenHandler handler, ILogger<TokenHttpMessageHandler> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogWarning(SecurityLogging.SecurityEventId, "The token HTTP message handler does not support sync operations");
        return base.Send(request, cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null)
        {
            var token = await _handler.GetToken(cancellationToken).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Added token to request");
        }
        else
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Request authorization was already set, did not add token to request");
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
