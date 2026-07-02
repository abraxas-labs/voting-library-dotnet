// (c) Copyright by Abraxas Informatik AG
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

    // Threshold (in bytes) below which transient UTF-8 buffers are taken from the stack
    // instead of being rented from the shared pool.
    private const int StackallocByteThreshold = 256;

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

        AppendUtf8(value, appendDelimiter: false);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="string"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(string? value)
    {
        if (value == null)
        {
            return Append(Delimiter);
        }

        AppendUtf8(value, appendDelimiter: true);
        return this;
    }

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
    {
        Span<byte> buffer = stackalloc byte[GuidExtensions.GuidByteLength + 1];
        value.WriteBytesAsRfc4122(buffer);
        buffer[GuidExtensions.GuidByteLength] = Delimiter;
        _hasher.AppendData(buffer);
        return this;
    }

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(bool value)
    {
        ReadOnlySpan<byte> buffer = [value ? (byte)1 : (byte)0];
        _hasher.AppendData(buffer);
        return this;
    }

    /// <summary>
    /// Appends a <see cref="bool"/> value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(bool value)
    {
        ReadOnlySpan<byte> buffer = [value ? (byte)1 : (byte)0, Delimiter];
        _hasher.AppendData(buffer);
        return this;
    }

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

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
    {
        ReadOnlySpan<byte> buffer = [value, Delimiter];
        _hasher.AppendData(buffer);
        return this;
    }

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

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
    {
        Span<byte> buffer = stackalloc byte[sizeof(int) + 1];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        buffer[sizeof(int)] = Delimiter;
        _hasher.AppendData(buffer);
        return this;
    }

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

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
    {
        Span<byte> buffer = stackalloc byte[sizeof(long) + 1];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        buffer[sizeof(long)] = Delimiter;
        _hasher.AppendData(buffer);
        return this;
    }

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

    /// <summary>
    /// Appends a <see cref="DateTime"/> value with millisecond precision.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder Append(DateTime value)
        => Append(ToUnixTimeMilliseconds(value));

    /// <summary>
    /// Appends a <see cref="DateTime"/> value with millisecond precision and a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited(DateTime value)
        => AppendDelimited(ToUnixTimeMilliseconds(value));

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

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
        => AppendDelimited(value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc));

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

    /// <summary>
    /// Appends the string representation of an enum value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder Append<T>(T value)
        where T : struct, Enum
        => Append(value.ToString());

    /// <summary>
    /// Appends the string representation of an enum value with a trailing delimiter.
    /// </summary>
    /// <param name="value">The value to append.</param>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>This instance.</returns>
    public HashBuilder AppendDelimited<T>(T value)
        where T : struct, Enum
        => AppendDelimited(value.ToString());

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
        => value.HasValue ? AppendDelimited(value.Value) : Append(Delimiter);

    /// <summary>
    /// Returns the computed hash and resets the internal state.
    /// </summary>
    /// <returns>The computed hash.</returns>
    public byte[] GetHashAndReset()
        => _hasher.GetHashAndReset();

    /// <summary>
    /// Writes the hash to the <paramref name="destination" /> and resets the internal state.
    /// </summary>
    /// <param name="destination">The buffer to receive the hash or HMAC value.</param>
    /// <returns>The number of bytes written to <paramref name="destination" />.</returns>
    public int GetHashAndReset(Span<byte> destination)
        => _hasher.GetHashAndReset(destination);

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public void Dispose()
    {
        _hasher.Dispose();
        GC.SuppressFinalize(this);
    }

    private static long ToUnixTimeMilliseconds(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new InvalidOperationException("Only date times with kind UTC are supported");
        }

        return new DateTimeOffset(value).ToUnixTimeMilliseconds();
    }

    private void AppendUtf8(string value, bool appendDelimiter)
    {
        // Use the cheap upper-bound estimate to decide between stack and pool.
        // the actual written length comes from GetBytes.
        var maxByteCount = Encoding.UTF8.GetMaxByteCount(value.Length);
        var maxRequired = maxByteCount + (appendDelimiter ? 1 : 0);

        if (maxRequired <= StackallocByteThreshold)
        {
            Span<byte> stackBuffer = stackalloc byte[StackallocByteThreshold];
            var written = Encoding.UTF8.GetBytes(value, stackBuffer);
            if (appendDelimiter)
            {
                stackBuffer[written++] = Delimiter;
            }

            _hasher.AppendData(stackBuffer[..written]);
            return;
        }

        var rented = ByteArrayPool.Rent(maxRequired);
        try
        {
            var written = Encoding.UTF8.GetBytes(value, rented);
            if (appendDelimiter)
            {
                rented[written++] = Delimiter;
            }

            _hasher.AppendData(rented.AsSpan(0, written));
        }
        finally
        {
            ByteArrayPool.Return(rented);
        }
    }
}
