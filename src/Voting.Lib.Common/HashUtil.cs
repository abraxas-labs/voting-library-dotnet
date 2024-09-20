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
    private const int Sha256Length = 256 / 8;

    /// <summary>
    /// Calculates the SHA256 hash of the input.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>Returns the SHA256 hash of the input.</returns>
    public static string GetSHA256Hash(string input)
    {
        var length = Encoding.UTF8.GetByteCount(input);
        var data = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            var dataView = data.AsSpan(..length);
            Encoding.UTF8.GetBytes(input, dataView);
            Span<byte> hash = stackalloc byte[Sha256Length];
            SHA256.HashData(dataView, hash);
            return ToHexString(hash);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    /// <summary>
    /// Converts a byte sequence into a hex string.
    /// </summary>
    /// <param name="data">The byte sequence.</param>
    /// <returns>The hex string.</returns>
    public static string ToHexString(ReadOnlySpan<byte> data)
    {
        var sb = new StringBuilder(data.Length);
        foreach (var t in data)
        {
            sb.Append(t.ToString("x2", CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }
}
