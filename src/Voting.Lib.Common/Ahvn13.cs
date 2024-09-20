// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace Voting.Lib.Common;

/// <summary>
/// Represent an AHVN13. See https://www.zas.admin.ch/zas/de/home/partenaires-et-institutions-/navs13.html.
/// </summary>
public class Ahvn13
{
    private const int Ahvn13DigitCount = 13;
    private static readonly Regex Ahvn13Regex = new(@"^(756)\.([0-9]{4})\.([0-9]{4})\.([0-9]{2})$", RegexOptions.Compiled);
    private readonly long _value;

    private Ahvn13(long value)
    {
        _value = value;
    }

    /// <summary>
    /// Tries to parse the supplied number as an AHVN13.
    /// </summary>
    /// <param name="number">The number to parse.</param>
    /// <param name="parsed">The parsed AHNV13 if the supplied number was valid.</param>
    /// <returns>A value indicating whether the supplied number is a valid AHVN13.</returns>
    public static bool TryParse(long number, [MaybeNullWhen(false)] out Ahvn13 parsed)
    {
        parsed = default;

        if (!IsValid(number))
        {
            return false;
        }

        parsed = new Ahvn13(number);
        return true;
    }

    /// <summary>
    /// Parses the supplied number as an AHVN13.
    /// </summary>
    /// <param name="number">The number to parse.</param>
    /// <returns>The parsed AHVN13.</returns>
    /// <exception cref="FormatException">When the supplied number is not a valid AHVN13.</exception>
    public static Ahvn13 Parse(long number)
    {
        if (TryParse(number, out var parsed))
        {
            return parsed;
        }

        throw new FormatException("Invalid AHVN13 number");
    }

    /// <summary>
    /// Tries to parse the supplied string as an AHVN13.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="parsed">The parsed AHNV13 if the supplied string was valid.</param>
    /// <returns>A value indicating whether the supplied string is a valid AHVN13.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Ahvn13 parsed)
    {
        if (!TryGetNumericValue(s, out var numberValue) || !IsValid(numberValue))
        {
            parsed = default;
            return false;
        }

        parsed = new Ahvn13(numberValue);
        return true;
    }

    /// <summary>
    /// Parses the supplied string as an AHVN13.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The parsed AHVN13.</returns>
    /// <exception cref="FormatException">When the supplied string is not a valid AHVN13.</exception>
    public static Ahvn13 Parse(string s)
    {
        if (TryParse(s, out var parsed))
        {
            return parsed;
        }

        throw new FormatException("Invalid AHVN13 number");
    }

    /// <summary>
    /// Checks whether the supplied number is a valid AHVN13.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Whether the number is a valid AHNV13.</returns>
    public static bool IsValid(long number)
    {
        if (number <= 0)
        {
            return false;
        }

        var actualCheckDigit = CalculateChecksum(number);

        // Last digit is the check digit
        var expectedCheckDigit = number % 10;
        return expectedCheckDigit == actualCheckDigit;
    }

    /// <summary>
    /// Checks whether the supplied string is a valid AHVN13.
    /// </summary>
    /// <param name="s">The string to validate.</param>
    /// <returns>Whether the string is a valid AHNV13.</returns>
    public static bool IsValid([NotNullWhen(true)] string? s)
    {
        return
            TryGetNumericValue(s, out var numberValue)
            && IsValid(numberValue);
    }

    /// <summary>
    /// Calculates the checksum of an ahv number.
    /// Ignores the last digit, but the input needs to be a full ahvN13.
    /// </summary>
    /// <param name="ahvN13">A full 13 digit long ahv number.</param>
    /// <returns>The calculated checksum for the provided ahvN13.</returns>
    public static int CalculateChecksum(long ahvN13)
    {
        var digits = GetDigitsOfNumber(ahvN13);
        if (digits.Length != Ahvn13DigitCount || digits[0] != 7 || digits[1] != 5 || digits[2] != 6)
        {
            return -1;
        }

        // For information on how to calculate the checksum, see
        // https://en.wikipedia.org/wiki/International_Article_Number#Calculation_of_checksum_digit
        var calculatedChecksum = 0;

        // Sum the first 12 digits, multiplying alternatively with 1 and 3
        for (var i = 0; i < Ahvn13DigitCount - 1; i++)
        {
            var modifier = i % 2 == 0 ? 1 : 3;
            calculatedChecksum += digits[i] * modifier;
        }

        // Calculate the difference to the next number divisible by ten
        return (10 - (calculatedChecksum % 10)) % 10;
    }

    /// <summary>
    /// Get the AHVN13 as a number, for example 7561234567897.
    /// </summary>
    /// <returns>The AHVN13 as a number.</returns>
    public long ToNumber()
        => _value;

    /// <summary>
    /// Get the AHVN13 as a string, for example 756.1234.5678.97.
    /// </summary>
    /// <returns>The AHVN13 as a string.</returns>
    public override string ToString()
        => _value.ToString(@"###\.####\.####\.##");

    private static bool TryGetNumericValue([NotNullWhen(true)] string? s, out long number)
    {
        if (s == null || !Ahvn13Regex.IsMatch(s))
        {
            number = default;
            return false;
        }

        // Since the regex matches, we can be sure that the format is correct and only digits remain after removing the periods.
        number = long.Parse(s.Replace(".", string.Empty));
        return true;
    }

    private static int[] GetDigitsOfNumber(long number)
        => GetReversedDigitsOfNumber(number).Reverse().ToArray();

    private static IEnumerable<int> GetReversedDigitsOfNumber(long number)
    {
        while (number > 0)
        {
            yield return (int)(number % 10);
            number /= 10;
        }
    }
}
