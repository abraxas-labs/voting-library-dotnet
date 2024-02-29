// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Voting.Lib.Common.Extensions;

namespace Voting.Lib.Common;

/// <summary>
/// Util to incremental build hashes.
/// </summary>
public class HashBuilder : IDisposable
{
    private const byte Delimiter = (byte)'|';
    private static readonly ArrayPool<byte> ByteArrayPool = ArrayPool<byte>.Shared;
    private readonly IncrementalHash _hasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="HashBuilder"/> class.
    /// </summary>
    /// <param name="hashAlgorithmName">The name of the hash algorithm to use.</param>
    public HashBuilder(HashAlgorithmName hashAlgorithmName)
    {
        _hasher = IncrementalHash.CreateHash(hashAlgorithmName);
    }

    /// <summary>
    /// Appends an enumerable of <see cref="byte"/> array value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(IEnumerable<byte[]?>? value)
    {
        if (value == null)
        {
            return this;
        }

        foreach (var v in value)
        {
            Append(v);
        }

        return this;
    }

    /// <summary>
    /// Appends an enumerable of <see cref="byte"/> array value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(IEnumerable<byte[]?>? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="byte"/> array value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(byte[]? value)
    {
        if (value == null)
        {
            return this;
        }

        _hasher.AppendData(value);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="byte"/> array value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(byte[]? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="string"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(string? value)
    {
        if (value == null)
        {
            return this;
        }

        var length = Encoding.UTF8.GetByteCount(value);
        var data = ByteArrayPool.Rent(length);
        var dataView = data.AsSpan(..length);
        try
        {
            Encoding.UTF8.GetBytes(value, dataView);
            _hasher.AppendData(dataView);
        }
        finally
        {
            ByteArrayPool.Return(data);
        }

        return this;
    }

    /// <summary>
    /// Appends a <see cref="string"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(string? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(Guid value)
    {
        Span<byte> buffer = stackalloc byte[GuidExtensions.GuidByteLength];
        value.WriteBytesAsRfc4122(buffer);
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="Guid"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(Guid value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(Guid? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="Guid"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(Guid? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(bool value)
    {
        ReadOnlySpan<byte> buffer = stackalloc[] { Convert.ToByte(value) };
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="bool"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(bool value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(bool? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="bool"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(bool? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(byte value)
    {
        ReadOnlySpan<byte> buffer = stackalloc[] { value };
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="byte"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(byte value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="byte"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(byte? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="byte"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(byte? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(int value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="int"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(int value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(int? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="int"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(int? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(long value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="long"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(long value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(long? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="long"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(long? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(DateTime value)
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
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(DateTime value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(DateTime? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="DateTime"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(DateTime? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(DateOnly value)
        => Append(value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc));

    /// <summary>
    /// Appends a <see cref="DateOnly"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(DateOnly value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends a nullable <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(DateOnly? value)
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends a nullable <see cref="DateOnly"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(DateOnly? value)
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends an enum value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder Append<T>(T value)
        where T : struct, Enum
        => Append(value.ToString());

    /// <summary>
    /// Appends an enum value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited<T>(T value)
        where T : struct, Enum
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Appends the string representation of a given nullable enum.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder Append<T>(T? value)
        where T : struct, Enum
        => value.HasValue ? Append(value.Value) : this;

    /// <summary>
    /// Appends the string representation of a given nullable enum with a trailing delimiter.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited<T>(T? value)
        where T : struct, Enum
        => Append(value).Append(Delimiter);

    /// <summary>
    /// Returns the computed hash and resets the internal state.
    /// </summary>
    /// <returns>The computed hash.</returns>
    public byte[] GetHashAndReset()
        => _hasher.GetHashAndReset();

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public void Dispose()
    {
        _hasher.Dispose();
        GC.SuppressFinalize(this);
    }
}
