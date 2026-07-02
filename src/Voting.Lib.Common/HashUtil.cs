// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Buffers;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Voting.Lib.Common;

/// <summary>
/// A helper class to calculate hashes.
/// </summary>
public static class HashUtil
{
    private const int StackAllocBytesThreshold = 512;

    /// <summary>
    /// Calculates the SHA256 hash of the input.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>Returns the SHA256 hash of the input as a hex string.</returns>
    public static string GetSHA256Hash(string input)
    {
        Span<byte> hash = stackalloc byte[SHA256.HashSizeInBytes];
        GetSHA256(input, hash);
        return ToHexString(hash);
    }

    /// <summary>
    /// Calculates the SHA256 hash of the input.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>Returns the SHA256 hash of the input.</returns>
    public static byte[] GetSHA256HashBytes(string input)
    {
        var hash = new byte[SHA256.HashSizeInBytes];
        GetSHA256(input, hash);
        return hash;
    }

    /// <summary>
    /// Calculates the SHA256 hash of the input.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <param name="output">The output span.</param>
    public static void GetSHA256(string input, Span<byte> output)
    {
        var byteCount = Encoding.UTF8.GetByteCount(input);

        if (byteCount <= StackAllocBytesThreshold)
        {
            Span<byte> inputBytes = stackalloc byte[byteCount];
            Encoding.UTF8.GetBytes(input, inputBytes);
            SHA256.HashData(inputBytes, output);
            return;
        }

        var buffer = ArrayPool<byte>.Shared.Rent(byteCount);
        try
        {
            var utf8Bytes = buffer.AsSpan(0, byteCount);
            Encoding.UTF8.GetBytes(input, utf8Bytes);
            SHA256.HashData(utf8Bytes, output);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Converts a byte sequence into a hex string.
    /// </summary>
    /// <param name="data">The byte sequence.</param>
    /// <returns>The hex string.</returns>
    public static string ToHexString(ReadOnlySpan<byte> data)
    {
        var sb = new StringBuilder(data.Length * 2);
        foreach (var t in data)
        {
            sb.Append(t.ToString("x2", CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }
}
