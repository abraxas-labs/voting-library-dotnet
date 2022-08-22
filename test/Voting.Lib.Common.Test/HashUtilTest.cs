// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class HashUtilTest
{
    [Theory]
    [InlineData("abcdefg987", "f59b643419bc0518284d4890f75f7156f74bba3d73e3296cc9575a3138a97fbe")]
    [InlineData("test-value-@#55", "651e834d62ea18eaea4f37a62572d7db8244b8b8cd98749d5b22960d89b236a0")]
    [InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
    public void Test(string value, string expected)
    {
        HashUtil.GetSHA256Hash(value).Should().Be(expected);
    }
}
