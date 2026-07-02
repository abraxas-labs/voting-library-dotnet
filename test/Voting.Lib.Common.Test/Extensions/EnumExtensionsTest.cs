// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Runtime.Serialization;
using FluentAssertions;
using Voting.Lib.Common.Extensions;
using Xunit;

namespace Voting.Lib.Common.Test.Extensions;

public class EnumExtensionsTest
{
    private enum TestEnum
    {
        [EnumMember(Value = "custom_first_value")]
        FirstValue,
        SecondValue, // No EnumMember attribute
    }

    [Fact]
    public void GetEnumMemberValueShouldReturnEnumMemberValueWhenAttributeExists()
    {
        var result = TestEnum.FirstValue.GetEnumMemberValue();
        result.Should().Be("custom_first_value");
    }

    [Fact]
    public void GetEnumMemberValueShouldReturnEnumNameWhenAttributeDoesNotExist()
    {
        var result = TestEnum.SecondValue.GetEnumMemberValue();
        result.Should().Be("SecondValue");
    }
}
