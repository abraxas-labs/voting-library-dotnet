// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common;

/// <summary>
/// Provides utility methods for Base64 URL-safe encoding and validation.
/// Base64URL is a variant of Base64 encoding where '+' is replaced with '-',
/// '/' is replaced with '_', and padding '=' characters are removed,
/// as defined in <see href="https://datatracker.ietf.org/doc/html/rfc4648#section-5">RFC 4648 §5</see>.
/// </summary>
public static class Base64Url
{
    /// <summary>
    /// Encodes the specified bytes into a Base64URL string,
    /// as defined in RFC 4648 §5.
    /// </summary>
    /// <param name="bytes">The bytes to encode.</param>
    /// <returns>
    /// A Base64URL string where '+' is replaced by '-', '/' is replaced by '_',
    /// and padding '=' characters are removed.
    /// </returns>
    public static string Encode(ReadOnlySpan<byte> bytes)
    {
        Span<char> base64 = Convert.ToBase64String(bytes).ToCharArray();
        base64.Replace('+', '-');
        base64.Replace('/', '_');
        base64 = base64.TrimEnd('=');
        return new string(base64);
    }

    /// <summary>
    /// Determines whether the specified string is a valid Base64 URL-safe string.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is not null or empty and consists only of
    /// letters, digits, '-' or '_'; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBase64Url(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!(char.IsLetterOrDigit(c) || c == '-' || c == '_'))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calculates the length of a Base64 URL-encoded string for the given number of bytes.
    /// </summary>
    /// <param name="byteLength">The number of bytes to encode.</param>
    /// <returns>The length of the resulting Base64 URL string without padding.</returns>
    public static int GetLength(int byteLength)
    {
        var fullBlocks = byteLength / 3;
        var remainder = byteLength % 3;
        var extra = remainder == 0 ? 0 : remainder + 1;
        return (fullBlocks * 4) + extra;
    }
}
