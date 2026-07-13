// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Snapper;
using Voting.Lib.Cryptography.Exceptions;
using Voting.Lib.Cryptography.Test.X509.TestFiles;
using Voting.Lib.Cryptography.X509;
using Xunit;

namespace Voting.Lib.Cryptography.Test.X509;

public class X509ChainBuilderTest
{
    [Fact]
    public void SingleIsolatedRootCertInPemShouldWork()
    {
        var certChain = Build(CertificateTestFiles.IsolatedRootRawPem);
        certChain.ShouldMatchSnapshot();
    }

    [Fact]
    public void SingleIsolatedLeafCertInPemNonRootShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(CertificateTestFiles.IsolatedLeafRawPem));
        ex.Message.Should().Contain("Exactly one root certificate is expected, but 0 are provided");
    }

    [Fact]
    public void MultipleCertsInPemShouldWork()
    {
        var certChain = Build(CertificateTestFiles.FourLevelChainRawPem);
        certChain.ShouldMatchSnapshot();
    }

    [Fact]
    public void DuplicateCertsShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(CertificateTestFiles.FourLevelChainWithDuplicatesRawPem));
        ex.Message.Should().Contain("The provided collection contains foreign or duplicate certificates.");
    }

    [Fact]
    public void MultipleLeafCertsInPemShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(CertificateTestFiles.FourLevelChainWithMultiLeafRawPem));
        ex.Message.Should().Contain("Exactly one leaf certificate is expected, but 2 are provided");
    }

    [Fact]
    public void MultipleCertsInPemMultiRootShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(CertificateTestFiles.FourLevelChainWithMultiRootRawPem));
        ex.Message.Should().Contain("Exactly one root certificate is expected, but 2 are provided");
    }

    [Fact]
    public void EmptyPemShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(string.Empty));
        ex.Message.Should().Contain("Exactly one leaf certificate is expected, but 0 are provided");
    }

    [Fact]
    public void ExpiredCertsInPemShouldThrow()
    {
        var ex = Assert.Throws<X509ChainBuilderException>(() => Build(CertificateTestFiles.ExpiredChainRawPem));
        ex.Message.Should().Contain("Cryptographic chain validation failed: NotTimeValid");
    }

    private List<Certificate> Build(string rawPem)
    {
        var certificateCollection = new X509Certificate2Collection();
        certificateCollection.ImportFromPem(rawPem);

        try
        {
            return X509ChainBuilder.VerifyLinearValidCertificateChain(certificateCollection, X509RevocationMode.NoCheck)
                .ConvertAll(c => new Certificate(
                    c.SubjectName.Name,
                    c.IssuerName.Name,
                    c.Thumbprint,
                    c.NotBefore.ToUniversalTime(),
                    c.NotAfter.ToUniversalTime()));
        }
        finally
        {
            certificateCollection.DisposeCertificates();
        }
    }

    private record Certificate(
        string Subject,
        string Issuer,
        string Thumbprint,
        DateTime NotBefore,
        DateTime NotAfter);
}
