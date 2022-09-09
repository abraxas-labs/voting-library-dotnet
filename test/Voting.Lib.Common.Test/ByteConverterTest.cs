// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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
        var stringValue = "Hello World";
        var dateTimeValue = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var guidValue = new Guid("0fab0d6a-59f2-4f30-806a-2e8fdb799fb5");
        var bytesValue = new byte[] { 3, 2, 1 };

        var result = ByteConverter.Concat(byteValue, intValue, longValue, stringValue, dateTimeValue, guidValue, bytesValue);
        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("BQAADLIAAAAAAAn78UhlbGxvIFdvcmxkAAABb15m6AAwZmFiMGQ2YS01OWYyLTRmMzAtODA2YS0yZThmZGI3OTlmYjUDAgE=");
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
