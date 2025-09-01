// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using FluentAssertions;
using Voting.Lib.Cryptography.Mocks;
using Xunit;

namespace Voting.Lib.Cryptography.Test;

public class AesGcmEncryptionMockTest
{
    [Fact]
    public void EncryptDecryptShouldWork()
    {
        var expectedPlainText = "Hello World."u8.ToArray();

        var cipherText = AesGcmEncryptionMock.Encrypt(expectedPlainText, "fooBar");
        cipherText.Should().NotBeEquivalentTo(expectedPlainText, o => o.WithStrictOrdering());

        var plainText = AesGcmEncryptionMock.Decrypt(cipherText, "fooBar");
        cipherText.Length.Should().Be(AesGcmEncryptionMock.TagSize + expectedPlainText.Length + AesGcmEncryptionMock.NonceSize);
        plainText.Should().BeEquivalentTo(expectedPlainText);
    }

    [Fact]
    public void DecryptWithOtherKeyIdShouldFail()
    {
        var expectedPlainText = "Hello World."u8.ToArray();

        var cipherText = AesGcmEncryptionMock.Encrypt(expectedPlainText, "fooBar");
        Assert.Throws<AuthenticationTagMismatchException>(() => AesGcmEncryptionMock.Decrypt(cipherText, "fooBar2"));
    }
}
