// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace Voting.Lib.Common;

/// <summary>
/// A helper class to convert objects into their byte representation.
/// </summary>
public static class ByteConverter
{
    /// <summary>
    /// Converts all objects to byte arrays which will be concatenated in the passing order.
    /// </summary>
    /// <param name="objects">Objects to convert to byte arrays.</param>
    /// <returns>The concatenated byte array.</returns>
    /// <exception cref="ArgumentException">If any object has a type which the converter does not support.</exception>
    public static byte[] Concat(params object[] objects)
    {
        var result = new List<byte>();

        foreach (var obj in objects)
        {
            var bytes = obj switch
            {
                byte byteValue => Convert(byteValue),
                int intValue => Convert(intValue),
                long longValue => Convert(longValue),
                Guid guidValue => Convert(guidValue),
                DateTime dateTimeValue => Convert(dateTimeValue),
                string stringValue => Convert(stringValue),
                byte[] byteArrValue => byteArrValue,
                _ => throw new ArgumentException($"{obj.GetType()} is not supported and cannot be concatenated to the byte array."),
            };
            result.AddRange(bytes);
        }

        return result.ToArray();
    }

    private static byte[] Convert(byte value) => new byte[] { value };

    private static byte[] Convert(int value)
    {
        var buffer = new byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        return buffer;
    }

    private static byte[] Convert(long value)
    {
        var buffer = new byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        return buffer;
    }

    private static byte[] Convert(string value) => Encoding.UTF8.GetBytes(value);

    // A Guid.ToString() always contains lower case characters.
    private static byte[] Convert(Guid value) => Convert(value.ToString());

    private static byte[] Convert(DateTime value) => Convert(new DateTimeOffset(value).ToUnixTimeMilliseconds());
}
