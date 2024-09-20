// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text;
using FluentAssertions;
using Voting.Lib.Cryptography.Mocks;
using Xunit;

namespace Voting.Lib.Cryptography.Test;

public class AesGcmEncryptionMockTest
{
    [Fact]
    public void AesGcmEncryptionMock_EncryptDecrypt_ShouldWork()
    {
        var expectedPlainText = Encoding.UTF8.GetBytes("Hello World.");

        var cipherText = AesGcmEncryptionMock.Encrypt(expectedPlainText);
        var plainText = AesGcmEncryptionMock.Decrypt(cipherText);

        cipherText.Length.Should().Be(AesGcmEncryptionMock.TagSize + expectedPlainText.Length + AesGcmEncryptionMock.NonceSize);
        plainText.Should().BeEquivalentTo(expectedPlainText);
    }
}
