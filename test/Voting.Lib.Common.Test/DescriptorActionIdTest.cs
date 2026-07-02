// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class DescriptorActionIdTest
{
    [Theory]
    [InlineData("publish", 42, "version-1", "b1cb20f31daa7665cd78b20e3cc421e70417b47e7dd16f66436821ef39af45d7")]
    [InlineData("archive", 42, "version-1", "5d4c7400a62dc5f76dd2c4b01cde1c393c8c121ee81969f525fa03d550b72079")]
    public void CreateShouldComputeExpectedHash(string actionName, int entityId, string version, string expectedHash)
    {
        DescriptorActionId.Create(actionName, entityId, version)
            .ComputeHash()
            .Should()
            .Be(expectedHash);
    }

    [Fact]
    public void CreateShouldIncludeParameterOrderInHash()
    {
        var firstHash = DescriptorActionId.Create("publish", 42, "version-1").ComputeHash();
        var secondHash = DescriptorActionId.Create("publish", "version-1", 42).ComputeHash();

        firstHash.Should().NotBe(secondHash);
    }
}
