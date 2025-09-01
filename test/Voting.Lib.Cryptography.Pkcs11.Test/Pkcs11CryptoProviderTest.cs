// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Net.Pkcs11Interop.Common;
using Voting.Lib.Cryptography.Pkcs11.Exceptions;
using Voting.Lib.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Voting.Lib.Cryptography.Pkcs11.Test;

[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Disposed in IAsyncLifeTime implementation.")]
public class Pkcs11CryptoProviderTest : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;

    private Pkcs11CryptoProvider _testee = null!; // initialized during InitializeAsync
    private IContainer _hsmSimulatorContainer = null!; // initialized during InitializeAsync

    public Pkcs11CryptoProviderTest(ITestOutputHelper output)
    {
        _output = output;
        new ServiceCollection().AddVotingLibPkcs11(Pkcs11Config.Instance);
    }

    public virtual async Task InitializeAsync()
    {
        _hsmSimulatorContainer = await HsmSimulatorTestContainer.StartNew(new XUnitLogger<Pkcs11CryptoProviderTest>(_output));
        _testee = new Pkcs11CryptoProvider(NullLogger<Pkcs11CryptoProvider>.Instance, Pkcs11Config.Instance);
    }

    [Fact]
    public async Task CreateEcdsaSha384Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world.");
        var signature = await _testee.CreateEcdsaSha384Signature(rawData, Pkcs11Config.CkaLabelEcdsaPrivate);
        var isValidSignature = await _testee.VerifyEcdsaSha384Signature(rawData, signature, Pkcs11Config.CkaLabelEcdsaPublic);

        isValidSignature.Should().BeTrue();
    }

    [Fact]
    public async Task BulkCreateEcdsaSha384Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawDataSets = new[] { "Hello", "world", "." }.Select(ConvertUtils.Utf8StringToBytes).ToArray();
        var signature = await _testee.BulkCreateEcdsaSha384Signature(rawDataSets, Pkcs11Config.CkaLabelEcdsaPrivate);

        for (int i = 0; i < rawDataSets.Length; i++)
        {
            var isValidSignature = await _testee.VerifyEcdsaSha384Signature(rawDataSets[i], signature[i], Pkcs11Config.CkaLabelEcdsaPublic);
            isValidSignature.Should().BeTrue();
        }
    }

    [Fact]
    public async Task CreateRsaSha512Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world.");
        var signature = await _testee.CreateSignature(rawData, Pkcs11Config.CkaLabelRsaPrivate);
        var isValidSignature = await _testee.VerifySignature(rawData, signature, Pkcs11Config.CkaLabelRsaPublic);

        isValidSignature.Should().BeTrue();
    }

    [Fact]
    public async Task BulkCreateRsaSha512Signature_WhenCalled_ThenShouldVerifySuccessfully()
    {
        var rawDataSets = new[] { "Hello", "world", "." }.Select(ConvertUtils.Utf8StringToBytes).ToArray();
        var signature = await _testee.BulkCreateSignature(rawDataSets, Pkcs11Config.CkaLabelRsaPrivate);

        for (int i = 0; i < rawDataSets.Length; i++)
        {
            var isValidSignature = await _testee.VerifySignature(rawDataSets[i], signature[i], Pkcs11Config.CkaLabelRsaPublic);
            isValidSignature.Should().BeTrue();
        }
    }

    [Fact]
    public async Task EncryptPlaintextWithAesGcm_WhenCalled_ThenShouldEncryptAndDecryptCipherSuccessfully()
    {
        var rawData = ConvertUtils.Utf8StringToBytes("Hello world");
        var cipher = await _testee.EncryptAesGcm(rawData, Pkcs11Config.CkaLabelAes);
        var plainText = await _testee.DecryptAesGcm(cipher, Pkcs11Config.CkaLabelAes);

        rawData.Should().Equal(plainText);
    }

    [Fact]
    public async Task EncryptEmptyPlaintextWithAesGcm_WhenCalled_ThenShouldThrowPkcs11Exception()
    {
        var rawData = ConvertUtils.Utf8StringToBytes(string.Empty);
        await Assert.ThrowsAsync<Exceptions.Pkcs11Exception>(() => _testee.EncryptAesGcm(rawData, Pkcs11Config.CkaLabelAes));
    }

    [Fact]
    public async Task CustomAesSecretKey_Roundtrip_ShouldWork()
    {
        var aesSecretKeyLabel = "aes-secret-key";
        await _testee.GenerateAesSecretKey(aesSecretKeyLabel);
        await Assert.ThrowsAsync<Pkcs11KeyAlreadyExistsException>(() => _testee.GenerateAesSecretKey(aesSecretKeyLabel));

        var plainText = new byte[] { 121, 12, 67, 55, 14, 37, 12, 15 };

        var cipherText = await _testee.EncryptAesGcm(plainText, aesSecretKeyLabel);
        var decryptedCipherText = await _testee.DecryptAesGcm(cipherText, aesSecretKeyLabel);

        plainText.SequenceEqual(decryptedCipherText).Should().BeTrue();

        await _testee.DeleteAesSecretKey(aesSecretKeyLabel);
        await Assert.ThrowsAsync<Exceptions.Pkcs11Exception>(() => _testee.DeleteAesSecretKey(aesSecretKeyLabel));
    }

    [Fact]
    public async Task CustomHmacSecretKey_Roundtrip_ShouldWork()
    {
        var hmacSecretKeyLabel = "hmac-secret-key";

        await _testee.GenerateMacSecretKey(hmacSecretKeyLabel);
        await Assert.ThrowsAsync<Pkcs11KeyAlreadyExistsException>(() => _testee.GenerateMacSecretKey(hmacSecretKeyLabel));

        var plainText = new byte[] { 17, 12, 21, 51, 53, 5, 9 };

        var hash = await _testee.CreateHmacSha256(plainText, hmacSecretKeyLabel);
        (await _testee.VerifyHmacSha256(plainText, hash, hmacSecretKeyLabel)).Should().BeTrue();

        await _testee.DeleteMacSecretKey(hmacSecretKeyLabel);
        await Assert.ThrowsAsync<Exceptions.Pkcs11Exception>(() => _testee.DeleteMacSecretKey(hmacSecretKeyLabel));
    }

    public virtual async Task DisposeAsync()
    {
        _testee.Dispose();
        await _hsmSimulatorContainer.DisposeAsync();
    }
}
