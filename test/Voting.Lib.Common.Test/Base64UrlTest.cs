// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class Base64UrlTest
{
    [Theory]
    [InlineData("", "")]
    [InlineData("f", "Zg")]
    [InlineData("fo", "Zm8")]
    [InlineData("foo", "Zm9v")]
    [InlineData("foob", "Zm9vYg")]
    [InlineData("fooba", "Zm9vYmE")]
    [InlineData("foobar", "Zm9vYmFy")]
    [InlineData("hello world", "aGVsbG8gd29ybGQ")]
    [InlineData("Test+Slash/", "VGVzdCtTbGFzaC8")]
    public void EncodeStringShouldReturnExpectedBase64UrlString(string input, string expected)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        EncodeBytesShouldReturnExpectedBase64UrlString(bytes, expected);
    }

    [Theory]
    [InlineData(new byte[] { 0xFB }, "-w")]
    [InlineData(new byte[] { 0xFF }, "_w")]
    [InlineData(new byte[] { 0xFB, 0xFF }, "-_8")]
    public void EncodeBytesShouldReturnExpectedBase64UrlString(byte[] bytes, string expected)
    {
        var actual = Base64Url.Encode(bytes);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("abcd", true)]
    [InlineData("ABC123-_", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("abc+", false)]
    [InlineData("abc/", false)]
    [InlineData("abc=", false)]
    [InlineData("abc def", false)]
    [InlineData("abc\tdef", false)]
    public void IsBase64UrlShouldReturnExpected(string? input, bool expected)
    {
        var span = input == null ? ReadOnlySpan<char>.Empty : input.AsSpan();
        var result = Base64Url.IsBase64Url(span);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(8, 11)]
    [InlineData(64, 86)]
    [InlineData(100, 134)]
    public void GetLength_ShouldReturnExpectedLength(int byteLength, int expectedLength)
    {
        var actual = Base64Url.GetLength(byteLength);
        actual.Should().Be(expectedLength);
    }
}
