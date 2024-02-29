// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Voting.Lib.Common.Json;
using Xunit;

namespace Voting.Lib.Common.Test.Json;

public class JsonSnakeCaseNamingPolicyTest
{
    [Theory]
    [InlineData("simple", "simple", "simple")]
    [InlineData("upper start", "Upper", "upper")]
    [InlineData("two words", "FooBar", "foo_bar")]
    public void ShouldWork(string testName, string input, string expected)
    {
        JsonSnakeCaseNamingPolicy.Instance.ConvertName(input)
            .Should()
            .Be(expected, testName);
    }
}
