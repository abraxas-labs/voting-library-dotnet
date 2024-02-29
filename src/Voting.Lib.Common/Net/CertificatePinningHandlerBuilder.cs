// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Common.Net;

/// <summary>
/// Adds certificate pinning validations to the primary http message handler.
/// </summary>
public class CertificatePinningHandlerBuilder : IHttpMessageHandlerBuilderFilter
{
    private readonly ILogger<CertificatePinningHandlerBuilder> _logger;
    private readonly CertificatePinningHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CertificatePinningHandlerBuilder"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="handler">The certificate pinning handler.</param>
    public CertificatePinningHandlerBuilder(ILogger<CertificatePinningHandlerBuilder> logger, CertificatePinningHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    /// <summary>
    /// Configure the HTTP message handler builder.
    /// </summary>
    /// <param name="next">The next builder action.</param>
    /// <returns>Returns the configuration action.</returns>
    /// <exception cref="InvalidCastException">Throws if the primary handler is not a <see cref="HttpClientHandler"/>.</exception>
    /// <exception cref="InvalidOperationException">Throw if the server certificate custom validation callback is already configured.</exception>
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Attaching certificate pinning to {Name}", builder.Name);

            // we throw if we can't attach our cert validation since it is security related
            // and we don't want the app to run without it.
            if (builder.PrimaryHandler is not HttpClientHandler primaryHandler)
            {
                throw new InvalidCastException($"Can only handle {nameof(HttpClientHandler)} handlers");
            }

            if (primaryHandler.ServerCertificateCustomValidationCallback != null)
            {
                throw new InvalidOperationException(
                    nameof(primaryHandler.ServerCertificateCustomValidationCallback) +
                    " is already set, cannot apply certificate pinning...");
            }

            primaryHandler.ServerCertificateCustomValidationCallback = _handler.HandleValidationCallback;

            next(builder);

            if (primaryHandler.ServerCertificateCustomValidationCallback != _handler.HandleValidationCallback)
            {
                _logger.LogWarning(SecurityLogging.SecurityEventId, "Certificate validation handler got overwritten for {Name}!", builder.Name);
            }
        };
    }
}
