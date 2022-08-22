// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Common.Net;
using Xunit;

namespace Voting.Lib.Common.Test.Net;

public class CertificatePinningHandlerTest : IDisposable
{
    private const string UnknownPublicKey =
        "040015939A798EF657227E7AC1EC5D5B9F47829228922FBE862102128645F33641F097D395F1FE8B12D5A7EB2A9EE582DCF4D973B7F63D46ECC0CBACC5A1815438DF8A003ADEE797B60A786E6F36EFB2B84EB5FC6B4B6EE23CDE4464885945BDBAC9C8BBB9B5B49FEC374D1354C6BA6861B5413123D174AB24407FF89624D4108DF719B4C3";

    private readonly List<IDisposable> _disposables = new();
    private byte _certCounter;

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public void WithExistingErrorsAndNotPinnedShouldReturnFalse()
    {
        var (cert, chain) = CreateCertificateWithChain();
        CreateHandler(new())
            .HandleValidationCallback(
                new HttpRequestMessage(HttpMethod.Post, new Uri("https://voting.example.com")),
                cert,
                chain,
                SslPolicyErrors.RemoteCertificateChainErrors)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithExistingErrorsAndPinnedShouldReturnFalse()
    {
        var (cert, chain) = CreateCertificateWithChain();
        var config = new CertificatePinningConfig
        {
            Pins =
            {
                new()
                {
                    Authorities = new() { "voting.example.com" },
                    PublicKeys = new() { cert.GetPublicKeyString() },
                },
            },
        };

        CreateHandler(config)
            .HandleValidationCallback(
                new HttpRequestMessage(HttpMethod.Post, new Uri("https://voting.example.com")),
                cert,
                chain,
                SslPolicyErrors.RemoteCertificateChainErrors)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithNullChainShouldReturnFalse()
    {
        var (cert, _) = CreateCertificateWithChain();
        CreateHandler(new())
            .HandleValidationCallback(
                new HttpRequestMessage(HttpMethod.Post, new Uri("https://voting.example.com")),
                cert,
                null,
                SslPolicyErrors.None)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithNullCertificateShouldReturnFalse()
    {
        var (_, chain) = CreateCertificateWithChain();
        CreateHandler(new())
            .HandleValidationCallback(
                new HttpRequestMessage(),
                null,
                chain,
                SslPolicyErrors.None)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithNullRequestUriShouldReturnFalse()
    {
        var (cert, chain) = CreateCertificateWithChain();
        CreateHandler(new())
            .HandleValidationCallback(
                new HttpRequestMessage(),
                cert,
                chain,
                SslPolicyErrors.None)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithEmptyAuthorityConfigShouldReturnFalse()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
        });

        Handle(config, cert, chain)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void PinnedAuthorityWithRequirePinningForAllAuthoritiesShouldReturnFalse()
    {
        var (cert, chain) = CreateCertificateWithChain();
        Handle(new(), cert, chain)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void UnpinnedAuthorityWithoutRequirePinningForAllAuthoritiesShouldReturnTrue()
    {
        var (cert, chain) = CreateCertificateWithChain();
        var config = new CertificatePinningConfig() { RequirePinningForAllAuthorities = false };
        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void UnpinnedAuthorityWithRequirePinningForAllAuthoritiesButAllowListedShouldReturnTrue()
    {
        var (cert, chain) = CreateCertificateWithChain();
        var config = new CertificatePinningConfig
        {
            Pins =
            {
                new()
                {
                    Authorities = new() { "voting.example.com" },
                    AllowWithoutAnyPins = true,
                },
            },
        };
        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PinnedCertificatePublicKeyShouldReturnTrue()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            PublicKeys = new() { cert.GetPublicKeyString() },
        });

        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PinnedCertificateWithAdditionalPublicKeyShouldReturnTrue()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            PublicKeys = new() { UnknownPublicKey, cert.GetPublicKeyString() },
        });

        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PinnedCertificateWithUnknownPublicKeyShouldReturnTrue()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            PublicKeys = new() { UnknownPublicKey },
        });

        Handle(config, cert, chain)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void EmptyPinnedChainShouldReturnDefault()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            ChainPublicKeys = new(),
        });

        Handle(config, cert, chain)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void PinnedChainPublicKeyShouldReturnTrue()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            ChainPublicKeys = new()
            {
                new() { PublicKeys = new() { chain.ChainElements[1].Certificate.GetPublicKeyString() } },
            },
        });

        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PinnedChainPublicKeyWithUnknownPublicKeysShouldReturnFalse()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            ChainPublicKeys = new()
            {
                new()
                {
                    PublicKeys = new()
                    {
                        UnknownPublicKey,
                    },
                },
            },
        });

        Handle(config, cert, chain)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithExistingErrorsAndNotRequiredPinningAndNoPinShouldReturnFalse()
    {
        var (cert, chain) = CreateCertificateWithChain();
        var config = new CertificatePinningConfig { RequirePinningForAllAuthorities = false };
        CreateHandler(config)
            .HandleValidationCallback(
                new(HttpMethod.Post, new Uri("https://voting.example.com")),
                cert,
                chain,
                SslPolicyErrors.RemoteCertificateChainErrors)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void WithExistingErrorsAndPinnedButAcceptAnyShouldReturnTrue()
    {
        var (cert, chain) = CreateCertificateWithChain();
        var config = new CertificatePinningConfig
        {
            Pins =
            {
                new()
                {
                    Authorities = new() { "voting.example.com" },
                    PublicKeys = new() { cert.GetPublicKeyString() },
                    DangerouslyAcceptAnyCertificate = true,
                },
            },
        };

        CreateHandler(config)
            .HandleValidationCallback(
                new HttpRequestMessage(HttpMethod.Post, new Uri("https://voting.example.com")),
                cert,
                chain,
                SslPolicyErrors.RemoteCertificateChainErrors)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void PinnedChainPublicKeyWithAdditionalPublicKeyShouldReturnTrue()
    {
        var config = new CertificatePinningConfig();
        var (cert, chain) = CreateCertificateWithChain();

        config.Pins.Add(new DomainCertificatePinning
        {
            Authorities = new() { "voting.example.com" },
            ChainPublicKeys = new()
            {
                new()
                {
                    PublicKeys = new()
                    {
                        UnknownPublicKey,
                        chain.ChainElements[1].Certificate.GetPublicKeyString(),
                    },
                },
            },
        });

        Handle(config, cert, chain)
            .Should()
            .BeTrue();
    }

    private bool Handle(CertificatePinningConfig config, X509Certificate2 cert, X509Chain chain)
    {
        return CreateHandler(config)
            .HandleValidationCallback(
                new HttpRequestMessage(HttpMethod.Put, new Uri("https://voting.example.com")),
                cert,
                chain,
                SslPolicyErrors.None);
    }

    private CertificatePinningHandler CreateHandler(CertificatePinningConfig config) =>
        new(config, NullLogger<CertificatePinningHandler>.Instance);

    private (X509Certificate2 Cert, X509Chain Chain) CreateCertificateWithChain()
    {
        var rootCert = CreateCertificate("CN=root.voting.example.com", true, null);
        var interCert = CreateCertificate("CN=inter.voting.example.com", true, rootCert);
        var cert = CreateCertificate("CN=voting.example.com", false, interCert);
        return (cert, CreateChain(cert, interCert, rootCert));
    }

    private X509Chain CreateChain(X509Certificate2 cert, params X509Certificate2[] certs)
    {
        var chain = new X509Chain
        {
            ChainPolicy =
            {
                RevocationMode = X509RevocationMode.NoCheck,
                VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority,
            },
        };
        _disposables.Add(chain);

        foreach (var parentCert in certs)
        {
            chain.ChainPolicy.ExtraStore.Add(parentCert);
        }

        chain.Build(cert)
            .Should()
            .BeTrue();
        return chain;
    }

    private X509Certificate2 CreateCertificate(string subjectName, bool addKeyCertSignExtensions, X509Certificate2? signingCert)
    {
        using var ecdsa = ECDsa.Create();
        var req = new CertificateRequest(subjectName, ecdsa, HashAlgorithmName.SHA256);

        if (addKeyCertSignExtensions)
        {
            req.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, -1, false));
            req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign, true));
        }

        if (signingCert == null)
        {
            return req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
        }

        var cert = req.Create(
            signingCert,
            signingCert.NotBefore,
            signingCert.NotAfter,
            new[] { _certCounter++ });
        _disposables.Add(cert);

        if (!addKeyCertSignExtensions)
        {
            return cert;
        }

        cert = cert.CopyWithPrivateKey(ecdsa);
        _disposables.Add(cert);
        return cert;
    }
}
