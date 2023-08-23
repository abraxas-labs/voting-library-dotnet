// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
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
        DateTimeKind enumUnknownValue = (DateTimeKind)99;
        var booleanValue = true;

        var result = ByteConverter.Concat(byteValue, intValue, longValue, longNullableValue, stringValue, dateTimeValue, dateTimeNullableValue, dateOnlyValue, guidValue, bytesValue, enumValue, enumNullValue, enumUnknownValue, byteArrayEnumerableValue, booleanValue);

        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("BQAADLIAAAAAAAn78UhlbGxvIFdvcmxkAAABb15m6AAAAAFvXmboAA+rDWpZ8k8wgGouj9t5n7UDAgFVdGM5OSAgAQ==");
    }

    [Fact]
    public void TestConcatDelimited()
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
        DateTimeKind enumUnknownValue = (DateTimeKind)99;
        var booleanValue = true;

        var result = ByteConverter.ConcatDelimited(byteValue, intValue, longValue, longNullableValue, stringValue, dateTimeValue, dateTimeNullableValue, dateOnlyValue, guidValue, bytesValue, enumValue, enumNullValue, enumUnknownValue, byteArrayEnumerableValue, booleanValue);

        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("BXwAAAyyfAAAAAAACfvxfEhlbGxvIFdvcmxkfAAAAW9eZugAfAAAAW9eZugAfA+rDWpZ8k8wgGouj9t5n7V8AwIBfFV0Y3w5OXwgIHwBfA==");
    }

    [Fact]
    public void TestAppend()
    {
        using var byteConverter = new ByteConverter();

        var result = byteConverter
            .Append(new byte[] { 1, 2, 3 })
            .Append((byte[]?)null)
            .Append(Enumerable.Repeat(new byte[] { 1, 2, 3 }, 3))
            .Append((IEnumerable<byte[]>?)null)
            .Append("fooBar")
            .Append((string?)null)
            .Append(true)
            .Append(false)
            .Append((bool?)null)
            .Append(Guid.Parse("c102b334-4842-40b2-8973-a4df1c7227fc"))
            .Append((Guid?)null)
            .Append((byte)3)
            .Append((byte?)null)
            .Append(10)
            .Append((int?)null)
            .Append(20L)
            .Append((long?)null)
            .Append(new DateOnly(2010, 12, 3))
            .Append((DateOnly?)null)
            .Append(new DateTime(2010, 11, 12, 13, 14, 15, DateTimeKind.Utc))
            .Append((DateTime?)null)
            .Append(DateTimeKind.Utc)
            .Append((DateTimeKind?)null)
            .GetBytes();

        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("AQIDAQIDAQIDAQIDZm9vQmFyAQDBArM0SEJAsolzpN8ccif8AwAAAAoAAAAAAAAAFAAAASyphzgAAAABLEA41FhVdGM=");
    }

    [Fact]
    public void TestAppendDelimited()
    {
        using var byteConverter = new ByteConverter();

        var result = byteConverter
            .AppendDelimited(new byte[] { 1, 2, 3 })
            .AppendDelimited((byte[]?)null)
            .AppendDelimited(Enumerable.Repeat(new byte[] { 1, 2, 3 }, 3))
            .AppendDelimited((IEnumerable<byte[]>?)null)
            .AppendDelimited("fooBar")
            .AppendDelimited((string?)null)
            .AppendDelimited(true)
            .AppendDelimited(false)
            .AppendDelimited((bool?)null)
            .AppendDelimited(Guid.Parse("c102b334-4842-40b2-8973-a4df1c7227fc"))
            .AppendDelimited((Guid?)null)
            .AppendDelimited((byte)3)
            .AppendDelimited((byte?)null)
            .AppendDelimited(10)
            .AppendDelimited((int?)null)
            .AppendDelimited(20L)
            .AppendDelimited((long?)null)
            .AppendDelimited(new DateOnly(2010, 12, 3))
            .AppendDelimited((DateOnly?)null)
            .AppendDelimited(new DateTime(2010, 11, 12, 13, 14, 15, DateTimeKind.Utc))
            .AppendDelimited((DateTime?)null)
            .AppendDelimited(DateTimeKind.Utc)
            .AppendDelimited((DateTimeKind?)null)
            .GetBytes();

        var b64Result = Convert.ToBase64String(result);
        b64Result.Should().Be("AQIDfHwBAgMBAgMBAgN8fGZvb0Jhcnx8AXwAfHzBArM0SEJAsolzpN8ccif8fHwDfHwAAAAKfHwAAAAAAAAAFHx8AAABLKmHOAB8fAAAASxAONRYfHxVdGN8fA==");
    }

    [Fact]
    public void TestAppendWithGrow()
    {
        var converter = new ByteConverter(0);

        // Uses an input string resulting in no base64 padding to be comparable with the resulting string.
        var b64ResultSingle = Convert.ToBase64String(converter.Append("fooBar").GetSpan());

        var iterations = 1_000;
        for (var i = 1; i < iterations; i++)
        {
            converter.Append("fooBar");
        }

        var b64Result = Convert.ToBase64String(converter.GetSpan());
        var expectedB64Result = string.Concat(Enumerable.Repeat(b64ResultSingle, iterations));
        b64Result.Should().Be(expectedB64Result);
    }

    [Fact]
    public void TestAppendDelimitedWithGrow()
    {
        var converter = new ByteConverter(0);

        // Uses an input string resulting in no base64 padding to be comparable with the resulting string.
        var b64ResultSingle = Convert.ToBase64String(converter.AppendDelimited("foBar").GetSpan());

        var iterations = 1_000;
        for (var i = 1; i < iterations; i++)
        {
            converter.AppendDelimited("foBar");
        }

        var b64Result = Convert.ToBase64String(converter.GetSpan());
        var expectedB64Result = string.Concat(Enumerable.Repeat(b64ResultSingle, iterations));
        b64Result.Should().Be(expectedB64Result);
    }

    [Fact]
    public void TestConcatUnsupportedTypeShouldThrow()
    {
        var objectValue = new object();

        var action = () => ByteConverter.Concat(objectValue);
        action.Should().Throw<ArgumentException>()
            .WithMessage("System.Object is not supported and cannot be concatenated to the byte array.");
    }

    [Fact]
    public void TestConcatDelimitedUnsupportedTypeShouldThrow()
    {
        var objectValue = new object();

        var action = () => ByteConverter.ConcatDelimited(objectValue);
        action.Should().Throw<ArgumentException>()
            .WithMessage("System.Object is not supported and cannot be concatenated to the byte array.");
    }
}
