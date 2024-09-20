// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System;

/// <summary>
/// String extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates a string to the max length.
    /// If longer than max length, the string is truncated to maxlength - 1 and … is added at the end.
    /// </summary>
    /// <param name="s">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the string.</param>
    /// <returns>The truncated string.</returns>
    public static string Truncate(this string s, int maxLength)
    {
        if (maxLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), nameof(maxLength) + " needs to be 1 at least");
        }

        return s.Length > maxLength ? s[..(maxLength - 1)] + '…' : s;
    }
}
