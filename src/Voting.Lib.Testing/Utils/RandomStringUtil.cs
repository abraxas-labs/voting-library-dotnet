// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;
using System.Text;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// Utilities related to testing proto validations.
/// </summary>
public static class RandomStringUtil
{
    private static readonly char[] AlphabeticChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static readonly char[] NumericChars = "0123456789".ToCharArray();
    private static readonly char[] Alphanumeric = [.. AlphabeticChars, .. NumericChars];
    private static readonly char[] AlphanumericWhitespace = [.. Alphanumeric, ' '];
    private static readonly char[] SimpleSingleLineText = [.. AlphanumericWhitespace, '-', '\''];
    private static readonly char[] SimpleMultiLineText = [.. SimpleSingleLineText, '\r', '\n'];
    private static readonly char[] ComplexSingleLineText = [.. SimpleSingleLineText, .. "!?+@,.:()/\"".ToCharArray()];
    private static readonly char[] ComplexMultiLineText = [.. ComplexSingleLineText, '\r', '\n'];

    /// <summary>
    /// Generates an alphabetic string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateAlphabetic(int size)
    {
        return Generate(size, AlphabeticChars);
    }

    /// <summary>
    /// Generates an alphanumeric string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateAlphanumeric(int size)
    {
        return Generate(size, Alphanumeric);
    }

    /// <summary>
    /// Generates an alphanumeric string with whitespaces.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateAlphanumericWhitespace(int size)
    {
        return Generate(size, AlphanumericWhitespace);
    }

    /// <summary>
    /// Generates an numeric string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateNumeric(int size)
    {
        return Generate(size, NumericChars);
    }

    /// <summary>
    /// Generates a simple single line string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateSimpleSingleLineText(int size)
    {
        return GenerateTrimmedText(size, SimpleSingleLineText);
    }

    /// <summary>
    /// Generates a simple multiline line string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateSimpleMultiLineText(int size)
    {
        return GenerateTrimmedText(size, SimpleMultiLineText);
    }

    /// <summary>
    /// Generates a complex single line string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateComplexSingleLineText(int size)
    {
        return GenerateTrimmedText(size, ComplexSingleLineText);
    }

    /// <summary>
    /// Generates a complex multi line string.
    /// </summary>
    /// <param name="size">Size of the generated string.</param>
    /// <returns>Generated string.</returns>
    public static string GenerateComplexMultiLineText(int size)
    {
        return GenerateTrimmedText(size, ComplexMultiLineText);
    }

    /// <summary>
    /// Generates a random https url.
    /// </summary>
    /// <param name="totalLength">The total length including the scheme.</param>
    /// <returns>The built url.</returns>
    public static string GenerateHttpsUrl(int totalLength)
    {
        var url = Uri.UriSchemeHttps + Uri.SchemeDelimiter;
        totalLength -= url.Length;

        if (totalLength <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalLength),
                totalLength,
                "Total length must be greater than scheme length");
        }

        const string tld = ".com";
        const int hostnameMaxLength = 63;
        var hostnameLength = Math.Min(totalLength - tld.Length, hostnameMaxLength - tld.Length);
        url += GenerateAlphabetic(hostnameLength) + tld;
        totalLength -= hostnameLength + tld.Length;
        if (totalLength > 0)
        {
            url += "/" + GenerateAlphabetic(totalLength - 1);
        }

        return url;
    }

    private static string Generate(int size, char[] chars)
    {
        var data = new byte[sizeof(int) * size];
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(size);
        for (var i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * sizeof(int));
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }

    private static string GenerateTrimmedText(int size, char[] chars)
    {
        var text = Generate(size, chars).Trim();
        if (text.Length == size)
        {
            return text;
        }

        return text + GenerateAlphabetic(size - text.Length);
    }
}
