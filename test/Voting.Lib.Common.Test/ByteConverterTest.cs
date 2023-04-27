// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class ByteConverterTest
{
    [Fact]
    public void TestConcat()
    {
        byte byteValue = 5;
        int intValue = 3250;
        long longValue = 654321;
        long? longNullableValue = null;
        var stringValue = "Hello World";
        var dateTimeValue = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime? dateTimeNullableValue = null;
        DateOnly dateOnlyValue = new DateOnly(2020, 1, 1);
        var guidValue = new Guid("0fab0d6a-59f2-4f30-806a-2e8fdb799fb5");
        var bytesValue = new byte[] { 3, 2, 1 };
        var byteArrayEnumerableValue = new List<byte[]> { new byte[] { 0x20, 0x20 } };
        Enum enumValue = DateTimeKind.Utc;
        Enum enumNullValue = null!;
        var booleanValue = true;

        var result = ByteConverter.Concat(byteValue, intValue, longValue, longNullableValue!, stringValue, dateTimeValue, dateTimeNullableValue!, dateOnlyValue, guidValue, bytesValue, enumValue, enumNullValue, byteArrayEnumerableValue, booleanValue);

        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("BQAADLIAAAAAAAn78UhlbGxvIFdvcmxkAAABb15m6AAAAAFvXmboADBmYWIwZDZhLTU5ZjItNGYzMC04MDZhLTJlOGZkYjc5OWZiNQMCAVV0YyAgAQ==");
    }

    [Fact]
    public void TestConcatUnsupportedTypeShouldThrow()
    {
        var objectValue = new object();

        var action = () => ByteConverter.Concat(objectValue);
        action.Should().Throw<ArgumentException>()
            .WithMessage("System.Object is not supported and cannot be concatenated to the byte array.");
    }
}
