// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Common.Net;

/// <summary>
/// Validates pinned certificates.
/// One pin can either pin intermediate/root certificates, the certificate itself or both.
/// A pin is considered valid if the following rules apply:
/// * If there are certificate validation errors provided by dotnet the certificate is considered invalid unless DangerouslyAcceptAnyCertificate is set to true.
///   If there are certificate validation errors provided by dotnet but DangerouslyAcceptAnyCertificate is set to true, no further checks are applied and the cert is accepted.
/// * A pin without a certificate pin and with no chain pins is considered invalid unless AllowWithoutAnyPins is set to true.
/// * If there are public keys pinned for the certificate itself,
///   the certificate must match one of the pinned public keys for the certificate.
///   If none of the pinned public keys is matched the entire pinning is considered invalid.
///   If no public key is pinned, this check is skipped.
/// * If there are public key sets pinned for the chain validation,
///   for each key set the following rules are applied.
///   The chain validation is considered valid if all key sets are considered valid.
///   If no key sets for the chain are pinned, this check is skipped.
///   * If ANY certificate in the chain matches ANY public key in the key set, this key set is considered valid.
/// </summary>
public class CertificatePinningHandler
{
    private readonly ILogger<CertificatePinningHandler> _logger;
    private readonly IReadOnlyDictionary<string, DomainCertificatePinning> _pinByAuthority;
    private readonly bool _requirePinningOnAllAuthorities;

    /// <summary>
    /// Initializes a new instance of the <see cref="CertificatePinningHandler"/> class.
    /// </summary>
    /// <param name="config">The certificate pinning configuration.</param>
    /// <param name="logger">The logger.</param>
    public CertificatePinningHandler(CertificatePinningConfig config, ILogger<CertificatePinningHandler> logger)
    {
        _logger = logger;
        _requirePinningOnAllAuthorities = config.RequirePinningForAllAuthorities;
        _pinByAuthority = config.Pins
            .SelectMany(x => x.Authorities.Select(d => (Authority: d, Pins: x)))
            .ToDictionary(x => x.Authority, x => x.Pins);
    }

    /// <summary>
    /// Handles a certificate validation callback.
    /// </summary>
    /// <param name="message">The HTTP request message.</param>
    /// <param name="certificate">The certificate.</param>
    /// <param name="chain">The certificate chain.</param>
    /// <param name="errors">The certificate errors.</param>
    /// <returns>Returns true if the certificate is valid.</returns>
    public bool HandleValidationCallback(
        HttpRequestMessage message,
        X509Certificate2? certificate,
        X509Chain? chain,
        SslPolicyErrors errors)
    {
        if (!ValidateHasChainAndCertificate(chain, certificate, message))
        {
            return false;
        }

        if (message.RequestUri is not { } requestUri)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Received a null RequestUri on a http request...");
            return false;
        }

        if (!_pinByAuthority.TryGetValue(requestUri.Authority, out var pin))
        {
            var level = _requirePinningOnAllAuthorities
                ? LogLevel.Error
                : LogLevel.Debug;
            _logger.Log(level, SecurityLogging.SecurityEventId, "No pinning configured for authority {Authority}", requestUri.Authority);
            return !_requirePinningOnAllAuthorities && errors == SslPolicyErrors.None;
        }

        if (errors != SslPolicyErrors.None)
        {
            return ValidateSslErrors(pin, errors, message);
        }

        if (!pin.HasPublicKeys && !pin.HasChainPublicKeys)
        {
            var level = pin.AllowWithoutAnyPins
                ? LogLevel.Debug
                : LogLevel.Error;
            _logger.Log(
                level,
                SecurityLogging.SecurityEventId,
                "Pinning for {Authority} configured without any pinned certificate public keys or chain certificate public keys",
                requestUri.Authority);
            return pin.AllowWithoutAnyPins;
        }

        return Validate(requestUri.Authority, pin, certificate)
            && ValidateChain(requestUri.Authority, pin, chain);
    }

    /// <summary>
    /// Validates required public keys are found in the chain.
    /// </summary>
    /// <param name="authority">The authority of the request.</param>
    /// <param name="pin">The pinning configuration to use.</param>
    /// <param name="chain">The certificate chain to validate.</param>
    /// <returns>True if the chain is considered valid or no checks were applied, false if it is considered invalid.</returns>
    private bool ValidateChain(
        string authority,
        DomainCertificatePinning pin,
        X509Chain chain)
    {
        if (pin.ChainPublicKeys == null || pin.ChainPublicKeys.Count == 0)
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "No certificate chain public key set configured for {Authority}", authority);
            return true;
        }

        var chainPks = chain.ChainElements.Select(x => x.Certificate.GetPublicKeyString()).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // for each chain public key set at least one public key must be present in the chain
        if (pin.ChainPublicKeys.All(publicKeySet => publicKeySet.PublicKeys.Any(pk => chainPks.Contains(pk))))
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Validated pinned certificate chain for {Authority}", authority);
            return true;
        }

        _logger.LogError(
            SecurityLogging.SecurityEventId,
            "Could not find any pinned public key in certificate chain for {Authority}: {ReceivedCertChainPins}",
            authority,
            string.Join(", ", chainPks));

        return false;
    }

    /// <summary>
    /// Validates the provided certificate has a whitelisted public key if a whitelist exists for this authority.
    /// </summary>
    /// <param name="authority">The authority of the request.</param>
    /// <param name="pin">The certificate pinning configuration.</param>
    /// <param name="certificate">The certificate to validate.</param>
    /// <returns>True if the certificate is considered valid or no checks were applied, false if it is considered invalid.</returns>
    private bool Validate(
        string authority,
        DomainCertificatePinning pin,
        X509Certificate2 certificate)
    {
        // certificate pinning .NET impl similar to how owasp proposes:
        // https://owasp.org/www-community/controls/Certificate_and_Public_Key_Pinning
        if (pin.PublicKeys == null || pin.PublicKeys.Count == 0)
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "No certificate public key configured for {Authority}", authority);
            return true;
        }

        if (pin.PublicKeys.Contains(certificate.GetPublicKeyString()))
        {
            _logger.LogDebug(SecurityLogging.SecurityEventId, "Validated pinned certificate for {Authority}", authority);
            return true;
        }

        _logger.LogError(
            SecurityLogging.SecurityEventId,
            "Invalid public key received for {Authority}: {ReceivedCertPin}",
            authority,
            certificate.GetPublicKeyString());

        return false;
    }

    /// <summary>
    /// Validates that there are no ssl policy errors or <see cref="DomainCertificatePinning.DangerouslyAcceptAnyCertificate"/>
    /// is set to <c>true</c>.
    /// </summary>
    /// <param name="pin">The domain pin configuration.</param>
    /// <param name="errors">The ssl policy errors.</param>
    /// <param name="message">The http request message.</param>
    /// <returns>Whether the validation succeeded.</returns>
    private bool ValidateSslErrors(DomainCertificatePinning pin, SslPolicyErrors errors, HttpRequestMessage message)
    {
        if (errors == SslPolicyErrors.None)
        {
            return true;
        }

        if (pin.DangerouslyAcceptAnyCertificate)
        {
            _logger.LogWarning(
                SecurityLogging.SecurityEventId,
                "Accepting certificate with errors {Errors} for {Authority} since DangerouslyAcceptAnyCertificate is set to true",
                errors,
                message.RequestUri?.Authority);
            return true;
        }

        _logger.LogWarning(SecurityLogging.SecurityEventId, "Received certificate errors {Errors} for {Authority}", errors, message.RequestUri?.Authority);
        return false;
    }

    /// <summary>
    /// Validates the chain and the certificate are not <c>null</c>.
    /// </summary>
    /// <param name="chain">The chain.</param>
    /// <param name="certificate">The certificate.</param>
    /// <param name="message">The http request message.</param>
    /// <returns>Whether the chain and the certificate are not <c>null</c>.</returns>
    private bool ValidateHasChainAndCertificate([NotNullWhen(true)] X509Chain? chain, [NotNullWhen(true)] X509Certificate2? certificate, HttpRequestMessage message)
    {
        if (chain == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Received a null certificate chain on request to {Authority}", message.RequestUri?.Authority);
            return false;
        }

        if (certificate == null)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, "Received a null certificate on request to {Authority}", message.RequestUri?.Authority);
            return false;
        }

        return true;
    }
}
