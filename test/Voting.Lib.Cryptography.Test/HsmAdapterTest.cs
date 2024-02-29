// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Net.Pkcs11Interop.Common;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Configuration;
using Xunit;

namespace Voting.Lib.Cryptography.Test;

public class HsmAdapterTest : IDisposable
{
    private readonly HsmAdapter _testee;
    private readonly Mock<ILogger<HsmAdapter>> _loggerMock = new();

    public HsmAdapterTest()
    {
        _testee = new HsmAdapter(_loggerMock.Object, new Pkcs11Config());
    }

    [Fact(Skip = "For developing purpose only. Makes a connection to a local HSM.")]
    public void CreateEcdsaSha384Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world");

        var pkcs11Config = new Pkcs11Config
        {
            // NOTE: For Linus/Docker: LibraryPath = "./libcs_pkcs11_R3.so"
            LibraryPath = "./cs_pkcs11_R3.dll", // NOTE: For Windows
            SlotId = 0,
            LoginPin = "1234",
            PrivateKeyCkaLabel = "VOSR_ECDSA_PRIVATE_KEY_PRE",
            PublicKeyCkaLabel = "VOSR_ECDSA_PUBLIC_KEY_PRE",
        };

        var signatureEcdsa = _testee.CreateEcdsaSha384Signature(rawData, pkcs11Config);
        var isValidEcdsa = _testee.VerifyEcdsaSha384Signature(rawData, signatureEcdsa, pkcs11Config);

        isValidEcdsa.Should().BeTrue();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _testee.Dispose();
    }
}
