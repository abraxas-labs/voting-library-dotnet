// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class UrlTokenTest
{
    [Fact]
    public void ConstructorShouldThrowForInvalidPrefix()
    {
        var invalid = "invalidToken";
        var act = () => new UrlToken(invalid);
        act.Should().Throw<ValidationException>()
            .WithMessage("*Invalid token format*");
    }

    [Fact]
    public void ConstructorShouldThrowForInvalidBase64()
    {
        var invalidBase64 = "ut.1-@@@";
        Action act = () => _ = new UrlToken(invalidBase64);
        act.Should().Throw<ValidationException>()
            .WithMessage("*Invalid token format*");
    }

    [Fact]
    public void ImplicitConversionToStringShouldWork()
    {
        var tokenValue = UrlToken.New().Value;
        var token = new UrlToken(tokenValue);
        string s = token;
        s.Should().Be(tokenValue);
    }

    [Fact]
    public void ImplicitConversionFromStringShouldWork()
    {
        var tokenValue = UrlToken.New().Value;
        UrlToken token = tokenValue;
        token.Value.Should().Be(tokenValue);
    }

    [Fact]
    public void NewShouldCreateValidToken()
    {
        var token = UrlToken.New();
        token.Value.Should().StartWith(UrlToken.Prefix);
        var base64Part = token.Value.AsSpan()[UrlToken.Prefix.Length..];
        Base64Url.IsBase64Url(base64Part).Should().BeTrue();
        base64Part.Length.Should().Be(86); // 64 bytes of entropy in b64url
    }

    [Fact]
    public void EqualShouldWork()
    {
        var token = UrlToken.New();
        var token2 = new UrlToken(token.Value);
        token.Equals(token2).Should().BeTrue();
        (token == token2).Should().BeTrue();
        (token != token2).Should().BeFalse();
        token.GetHashCode().Should().Be(token2.GetHashCode());
    }

    [Theory]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    public void NewWithCustomSizeShouldCreateValidToken(int size)
    {
        var token = UrlToken.New(size);
        token.Value.Should().StartWith(UrlToken.Prefix);

        var tokenValue = token.Value.AsSpan()[UrlToken.Prefix.Length..];
        Base64Url.IsBase64Url(tokenValue).Should().BeTrue();
        tokenValue.Length.Should().Be(Base64Url.GetLength(size));
    }
}
