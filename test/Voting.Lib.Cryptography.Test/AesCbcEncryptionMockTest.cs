// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using FluentAssertions;
using Voting.Lib.Cryptography.Mocks;
using Xunit;

namespace Voting.Lib.Cryptography.Test;

public class AesCbcEncryptionMockTest
{
    [Fact]
    public void AesCbcEncryptionMock_EncryptDecrypt_ShouldWork()
    {
        var aes = Aes.Create();
        aes.GenerateKey();
        var key = aes.Key;

        var encrypted = AesCbcEncryptionMock.Encrypt(key);
        var decrypted = AesCbcEncryptionMock.Decrypt(encrypted);

        decrypted.Should().BeEquivalentTo(key);
        encrypted.Length.Should().Be(key.Length);
    }
}
