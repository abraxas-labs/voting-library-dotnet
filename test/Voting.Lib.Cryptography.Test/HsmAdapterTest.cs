// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Net.Pkcs11Interop.Common;
using Voting.Lib.Cryptography.Asymmetric;
using Voting.Lib.Cryptography.Configuration;
using Xunit;
using Pkcs11Exception = Voting.Lib.Cryptography.Exceptions.Pkcs11Exception;

namespace Voting.Lib.Cryptography.Test;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Disposed in IAsyncLifeTime implementation.")]
public class HsmAdapterTest : IAsyncLifetime
{
    private readonly HsmAdapter _testee;
    private readonly Mock<ILogger<HsmAdapter>> _loggerMock = new();

    private IContainer _hsmSimulatorContainer = null!; // initialized during InitializeAsync

    public HsmAdapterTest()
    {
        _testee = new HsmAdapter(_loggerMock.Object, new Pkcs11Config());
        new ServiceCollection().AddVotingLibPkcs11(HsmUtil.GetPkcs11Config());
    }

    public virtual async Task InitializeAsync()
    {
        _hsmSimulatorContainer = await HsmSimulatorTestContainer.StartNew();
    }

    [Fact]
    public void CreateEcdsaSha384Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world.");
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithEcdsa();
        var signature = _testee.CreateEcdsaSha384Signature(rawData, pkcs11Config);
        var isValidSignature = _testee.VerifyEcdsaSha384Signature(rawData, signature, pkcs11Config);

        isValidSignature.Should().BeTrue();
    }

    [Fact]
    public void BulkCreateEcdsaSha384Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawDataSets = new[] { "Hello", "world", "." }.Select(d => ConvertUtils.Utf8StringToBytes(d));
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithEcdsa();
        var signature = _testee.BulkCreateEcdsaSha384Signature(rawDataSets, pkcs11Config);

        for (int i = 0; i < rawDataSets.Count(); i++)
        {
            var isValidSignature = _testee.VerifyEcdsaSha384Signature(rawDataSets.ElementAt(i), signature.ElementAt(i), pkcs11Config);
            isValidSignature.Should().BeTrue();
        }
    }

    [Fact]
    public void CreateRsaSha512Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world.");
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithRsa();
        var signature = _testee.CreateSignature(rawData, pkcs11Config);
        var isValidSignature = _testee.VerifySignature(rawData, signature, pkcs11Config);

        isValidSignature.Should().BeTrue();
    }

    [Fact]
    public void BulkCreateRsaSha512Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawDataSets = new[] { "Hello", "world", "." }.Select(d => ConvertUtils.Utf8StringToBytes(d));
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithRsa();
        var signature = _testee.BulkCreateSignature(rawDataSets, pkcs11Config);

        for (int i = 0; i < rawDataSets.Count(); i++)
        {
            var isValidSignature = _testee.VerifySignature(rawDataSets.ElementAt(i), signature.ElementAt(i), pkcs11Config);
            isValidSignature.Should().BeTrue();
        }
    }

    [Fact]
    public void EncryptPlaintextWithAesGcm_WhenCalled_ThenShouldEncryptAndDecryptCipherSuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world");
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithAes();
        var cipher = _testee.EncryptAes(rawData, pkcs11Config);
        var plainText = _testee.DecryptAes(cipher, pkcs11Config);

        rawData.Should().Equal(plainText);
    }

    [Fact]
    public void EncryptEmptyPlaintextWithAesGcm_WhenCalled_ThenShouldThrowPkcs11Exception()
    {
        var rawData = ConvertUtils.Utf8StringToBytes(string.Empty);
        var pkcs11Config = HsmUtil.GetPkcs11Config().WithAes();

        var ex = Assert.Throws<Pkcs11Exception>(() => _testee.EncryptAes(rawData, pkcs11Config));
    }

    public virtual async Task DisposeAsync()
    {
        _testee.Dispose();
        await _hsmSimulatorContainer.DisposeAsync();
    }
}
