// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Voting.Lib.Cryptography.Exceptions;

namespace Voting.Lib.Cryptography.X509;

/// <summary>
/// Utility class for building and verifying X.509 chains.
/// </summary>
public static class X509ChainBuilder
{
    /// <summary>
    /// Validates a collection of certificates, and ensures they form a single unbroken
    /// cryptographic chain from a single leaf certificate to a single custom root certificate.
    /// It uses the root certificate as the custom trust store.
    /// </summary>
    /// <param name="certificates">An unordered collection of certificates.</param>
    /// <param name="revocationMode">The X.509 certificate revocation mode.</param>
    /// <returns>An ordered list of validated certificates starting from the leaf to the root.</returns>
    public static List<X509Certificate2> VerifyLinearValidCertificateChain(X509Certificate2Collection certificates, X509RevocationMode revocationMode)
    {
        var issuers = certificates
            .Select(c => c.IssuerName.RawData)
            .ToList();

        var leafCerts = certificates.Count == 1
            ? new List<X509Certificate2> { certificates[0] }
            : certificates.Where(c => !issuers.Any(issuer => issuer.SequenceEqual(c.SubjectName.RawData))).ToList();

        if (leafCerts.Count != 1)
        {
            throw new X509ChainBuilderException($"Exactly one leaf certificate is expected, but {leafCerts.Count} are provided");
        }

        var rootCerts = certificates
            .Where(IsRootCa)
            .ToList();

        if (rootCerts.Count != 1)
        {
            throw new X509ChainBuilderException($"Exactly one root certificate is expected, but {rootCerts.Count} are provided");
        }

        using var chain = new X509Chain();
        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        chain.ChainPolicy.RevocationMode = revocationMode;

        foreach (var cert in certificates)
        {
            if (IsRootCa(cert))
            {
                chain.ChainPolicy.CustomTrustStore.Add(cert);
            }
            else if (IsIntermediateCa(cert))
            {
                chain.ChainPolicy.ExtraStore.Add(cert);
            }
        }

        var isValidChain = chain.Build(leafCerts[0]);
        if (!isValidChain)
        {
            var errors = chain.ChainStatus.Select(s => $"{s.Status}: {s.StatusInformation}");
            throw new X509ChainBuilderException($"Cryptographic chain validation failed: {string.Join(" | ", errors)}");
        }

        if (chain.ChainElements.Count != certificates.Count)
        {
            throw new X509ChainBuilderException("The provided collection contains foreign or duplicate certificates.");
        }

        return chain.ChainElements.Select(e => e.Certificate).ToList();
    }

    private static bool IsRootCa(X509Certificate2 cert)
        => cert.SubjectName.RawData.SequenceEqual(cert.IssuerName.RawData) && HasCaBasicConstraint(cert);

    private static bool IsIntermediateCa(X509Certificate2 cert)
        => !cert.SubjectName.RawData.SequenceEqual(cert.IssuerName.RawData) && HasCaBasicConstraint(cert);

    private static bool HasCaBasicConstraint(X509Certificate2 cert)
    {
        var basicConstraintsExtension = cert.Extensions
            .OfType<X509BasicConstraintsExtension>()
            .FirstOrDefault();

        // Extension required (End-entity cert by RFC 5280 convention).
        return basicConstraintsExtension?.CertificateAuthority == true;
    }
}
