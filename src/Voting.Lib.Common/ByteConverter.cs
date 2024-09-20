// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using Voting.Lib.Common.Extensions;

namespace Voting.Lib.Common;

/// <summary>
/// A helper class to convert objects into their byte representation.
/// </summary>
public class ByteConverter : IDisposable
{
    private const byte Delimiter = (byte)'|';
    private const int InitialLength = 256;
    private const double GrowthFactor = 2;

    private static readonly ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

    /// <summary>
    /// The current used length of <see cref="_data"/>.
    /// This is usually smaller than <c>_data.Length</c>,
    /// as not the entire data buffer should be filled.
    /// If the data buffer is too small for an incoming append call,
    /// a new larger data buffer is rented from the pool,
    /// data is copied to the new buffer and the old one is returned.
    /// </summary>
    private int _length;
    private byte[] _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteConverter"/> class.
    /// </summary>
    /// <param name="initialCapacity">The initial internal capacity.</param>
    public ByteConverter(int initialCapacity)
    {
        _data = Pool.Rent(initialCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteConverter"/> class.
    /// </summary>
    public ByteConverter()
        : this(InitialLength)
    {
    }

    /// <summary>
    /// Converts all objects to byte arrays which will be concatenated in the passing order.
    /// If performance is critical, and the type of all passed parameters is known at compile time,
    /// use <seealso cref="ByteConverter"/> new(), Append and <seealso cref="GetBytes()"/> or <seealso cref="GetSpan()"/> instead.
    /// </summary>
    /// <param name="objects">Objects to convert to byte arrays.</param>
    /// <returns>The concatenated byte array.</returns>
    /// <exception cref="ArgumentException">If any object has a type which the converter does not support.</exception>
    public static byte[] Concat(params object?[] objects)
    {
        using var data = new ByteConverter();

        foreach (var obj in objects)
        {
            switch (obj)
            {
                case null:
                    break;

                case byte byteValue:
                    data.Append(byteValue);
                    break;

                case int intValue:
                    data.Append(intValue);
                    break;

                case long longValue:
                    data.Append(longValue);
                    break;

                case Guid guidValue:
                    data.Append(guidValue);
                    break;

                case DateTime dateTimeValue:
                    data.Append(dateTimeValue);
                    break;

                case DateOnly dateOnlyValue:
                    data.Append(dateOnlyValue);
                    break;

                case bool boolValue:
                    data.Append(boolValue);
                    break;

                case string stringValue:
                    data.Append(stringValue);
                    break;

                case Enum enumValue:
                    data.Append(enumValue);
                    break;

                case byte[] byteArrValue:
                    data.Append(byteArrValue);
                    break;

                case IEnumerable<byte[]> byteArrEnumerableValue:
                    data.Append(byteArrEnumerableValue);
                    break;

                default:
                    throw new ArgumentException($"{obj.GetType()} is not supported and cannot be concatenated to the byte array.");
            }
        }

        return data.GetBytes();
    }

    /// <summary>
    /// Converts all objects to byte arrays which will be concatenated in the passing order each suffixed with a trailing delimiter.
    /// If performance is critical, and the type of all passed parameters is known at compile time,
    /// use <seealso cref="ByteConverter"/> new(), AppendDelimited and <seealso cref="GetBytes()"/> or <seealso cref="GetSpan()"/> instead.
    /// </summary>
    /// <param name="objects">Objects to convert to byte arrays.</param>
    /// <returns>The concatenated byte array.</returns>
    /// <exception cref="ArgumentException">If any object has a type which the converter does not support.</exception>
    public static byte[] ConcatDelimited(params object?[] objects)
    {
        using var data = new ByteConverter();

        foreach (var obj in objects)
        {
            switch (obj)
            {
                case null:
                    break;

                case byte byteValue:
                    data.AppendDelimited(byteValue);
                    break;

                case int intValue:
                    data.AppendDelimited(intValue);
                    break;

                case long longValue:
                    data.AppendDelimited(longValue);
                    break;

                case Guid guidValue:
                    data.AppendDelimited(guidValue);
                    break;

                case DateTime dateTimeValue:
                    data.AppendDelimited(dateTimeValue);
                    break;

                case DateOnly dateOnlyValue:
                    data.AppendDelimited(dateOnlyValue);
                    break;

                case bool boolValue:
                    data.AppendDelimited(boolValue);
                    break;

                case string stringValue:
                    data.AppendDelimited(stringValue);
                    break;

                case Enum enumValue:
                    data.AppendDelimited(enumValue);
                    break;

                case byte[] byteArrValue:
                    data.AppendDelimited(byteArrValue);
                    break;

                case IEnumerable<byte[]> byteArrEnumerableValue:
                    data.AppendDelimited(byteArrEnumerableValue);
                    break;

                default:
                    throw new ArgumentException($"{obj.GetType()} is not supported and cannot be concatenated to the byte array.");
            }
        }

        return data.GetBytes();
    }

    /// <summary>
    /// Appends a sequence of byte arrays.
    /// </summary>
    /// <param name="value">The byte arrays.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(IEnumerable<byte[]?>? value)
    {
        if (value == null)
        {
            return this;
        }

        foreach (var b in value)
        {
            Append(b);
        }

        return this;
    }

    /// <summary>
    /// Appends a sequence of byte arrays with a trailing delimiter.
    /// </summary>
    /// <param name="value">The byte arrays.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(IEnumerable<byte[]?>? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a byte array.
    /// </summary>
    /// <param name="value">The byte array.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(byte[]? value)
    {
        if (value == null)
        {
            return this;
        }

        var data = GetSpan(value.Length);
        value.CopyTo(data);
        _length += value.Length;
        return this;
    }

    /// <summary>
    /// Appends a byte array from a readonly span.
    /// </summary>
    /// <param name="value">The byte array.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(ReadOnlySpan<byte> value)
    {
        var data = GetSpan(value.Length);
        value.CopyTo(data);
        _length += value.Length;
        return this;
    }

    /// <summary>
    /// Appends a byte array with a trailing delimiter.
    /// </summary>
    /// <param name="value">The byte array.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(byte[]? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="string"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(string? value)
    {
        if (value == null)
        {
            return this;
        }

        var data = GetSpan(Encoding.UTF8.GetByteCount(value));
        _length += Encoding.UTF8.GetBytes(value, data);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="string"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(string? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="Guid"/> value in its raw byte representation.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(Guid value)
    {
        var data = GetSpan(GuidExtensions.GuidByteLength);
        value.WriteBytesAsRfc4122(data);
        _length += data.Length;
        return this;
    }

    /// <summary>
    /// Appends a <see cref="Guid"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(Guid value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(Guid? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="Guid"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(Guid? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(bool value)
        => Append(Convert.ToByte(value));

    /// <summary>
    /// Appends a <see cref="bool"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(bool value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(bool? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="bool"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(bool? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(byte value)
    {
        var data = GetSpan(sizeof(byte));
        data[0] = value;
        _length++;
        return this;
    }

    /// <summary>
    /// Appends a <see cref="byte"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(byte value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(byte? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="byte"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(byte? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends an <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(int value)
    {
        var data = GetSpan(sizeof(int));
        BinaryPrimitives.WriteInt32BigEndian(data, value);
        _length += sizeof(int);
        return this;
    }

    /// <summary>
    /// Appends an <see cref="int"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(int value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(int? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="int"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(int? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(long value)
    {
        var data = GetSpan(sizeof(long));
        BinaryPrimitives.WriteInt64BigEndian(data, value);
        _length += sizeof(long);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="long"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(long value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends nullable a <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(long? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends nullable a <see cref="long"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(long? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new InvalidOperationException("Only date times with kind UTC are supported");
        }

        return Append(new DateTimeOffset(value).ToUnixTimeMilliseconds());
    }

    /// <summary>
    /// Appends a <see cref="DateTime"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(DateTime value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(DateTime? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="DateTime"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(DateTime? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(DateOnly value)
        => Append(value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc));

    /// <summary>
    /// Appends a <see cref="DateOnly"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(DateOnly value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter Append(DateOnly? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="DateOnly"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited(DateOnly? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends the string representation of a given enum.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public ByteConverter Append<T>(T value)
        where T : Enum
        => Append(value.ToString());

    /// <summary>
    /// Appends the string representation of a given enum with a trailing delimiter.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited<T>(T value)
        where T : Enum
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends the string representation of a given nullable enum.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public ByteConverter Append<T>(T? value)
        where T : struct, Enum
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends the string representation of a given nullable enum with a trailing delimiter.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public ByteConverter AppendDelimited<T>(T? value)
        where T : struct, Enum
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Returns the buffer.
    /// This is only valid as long as this object is not disposed.
    /// </summary>
    /// <returns>The buffer.</returns>
    public ReadOnlySpan<byte> GetSpan()
        => _data.AsSpan(.._length);

    /// <summary>
    /// Returns a copy of the buffer.
    /// </summary>
    /// <returns>A copy of the internal buffer.</returns>
    public byte[] GetBytes()
        => _data[.._length];

    /// <summary>
    /// Disposes all rented arrays.
    /// </summary>
    public void Dispose()
    {
        Pool.Return(_data);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Returns a span of the given length of writeable data.
    /// After the data is successfully written to the span, ensure <see cref="_length"/> is increased by the written data length.
    /// </summary>
    /// <param name="length">The length to be written.</param>
    /// <returns>A span to be written to.</returns>
    private Span<byte> GetSpan(int length)
    {
        if (_length + length > _data.Length)
        {
            EnsureSize(length);
        }

        return _data.AsSpan(_length..(_length + length));
    }

    /// <summary>
    /// Ensures the array in use has a given length of free space.
    /// If this condition is not given,
    /// a new array is rented with according minimal size,
    /// the content of the old one is copied over,
    /// and the old one is returned.
    /// </summary>
    /// <param name="length">The minimal required free space.</param>
    private void EnsureSize(int length)
    {
        var newSize = Math.Max(_data.Length + length, (int)(_data.Length * GrowthFactor));
        var newData = Pool.Rent(newSize);
        Buffer.BlockCopy(_data, 0, newData, 0, _length);
        Pool.Return(_data);
        _data = newData;
    }
}
