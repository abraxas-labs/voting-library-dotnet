// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text.RegularExpressions;

namespace Voting.Lib.Validation;

/// <summary>
/// Validations for strings.
/// </summary>
public static partial class StringValidation
{
    /// <summary>
    /// Gets the pattern for validating email addresses.
    /// </summary>
    public const string EmailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?\z";

    /// <summary>
    /// Gets the pattern for validating phone numbers.
    /// </summary>
    public const string PhonePattern = @"^\+?(?:[1-9]\s*){0,3}(\s)?(?:[0-9]\s?){7,14}\z";

    /// <summary>
    /// Gets the pattern for validating numeric values.
    /// </summary>
    public const string NumericPattern = @"^\d+\z";

    /// <summary>
    /// Gets the pattern for validating text.
    /// </summary>
    public const string AlphaPattern = @"^[\p{L}\p{M}]+\z";

    /// <summary>
    /// Gets the pattern for validating text with whitespace.
    /// </summary>
    public const string AlphaWhitePattern = @"^[\p{L}\p{M} ]+\z";

    /// <summary>
    /// Gets the pattern for validating alphanumeric text with whitespace.
    /// </summary>
    public const string AlphaNumWhitePattern = @"^[\p{L}\p{M}\p{Nd} ]+\z";

    /// <summary>
    /// Gets the pattern for validating simple single line text.
    /// </summary>
    public const string SimpleSlTextPattern = @"^[\p{L}\p{M}\p{Nd} \.'-]+\z";

    /// <summary>
    /// Gets the pattern for validating simple multiline text.
    /// </summary>
    public const string SimpleMlTextPattern = @"^[\p{L}\p{M}\p{Nd} \.'\-\r\n]+\z";

    /// <summary>
    /// Gets the pattern for validating complex single line text.
    /// </summary>
    public const string ComplexSlTextPattern = @"^[\p{L}\p{M}\p{Nd}\t _!\?+\-@,\.:'\(\)\/—\""«»;&–`´’‘\+\*%=§\[\]±]+\z";

    /// <summary>
    /// Gets the pattern for validating complex multiline text.
    /// </summary>
    public const string ComplexMlTextPattern = @"^[\p{L}\p{M}\p{Nd}\r\n\t _!\?+\-@,\.:'\(\)\/—\""«»;&–`´’‘\+\*%=§\[\]±]+\z";

    /// <summary>
    /// Gets the pattern for validating untrimmed text.
    /// </summary>
    public const string UntrimmedPattern = @"(^\s)|(\s\z)";

    private const int RegexTimeoutMilliseconds = 500;

    /// <summary>
    /// Gets the timeout for regex operations.
    /// </summary>
    public static TimeSpan ValidationTimeout { get; } = TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds);

    /// <summary>
    /// Gets the regex for validating email addresses.
    /// </summary>
    // copied from https://github.com/microsoft/referencesource/blob/5697c29004a34d80acdaf5742d7e699022c64ecd/System.ComponentModel.DataAnnotations/DataAnnotations/EmailAddressAttribute.cs#L54.
    public static Regex EmailRegex { get; } = GenerateEmailRegex();

    /// <summary>
    /// Gets the regex for validating phone numbers.
    /// </summary>
    public static Regex PhoneRegex { get; } = GeneratePhoneRegex();

    /// <summary>
    /// Gets the regex for validating numeric values.
    /// </summary>
    public static Regex NumericRegex { get; } = GenerateNumericRegex();

    /// <summary>
    /// Gets the regex for validating text.
    /// </summary>
    public static Regex AlphaRegex { get; } = GenerateAlphaRegex();

    /// <summary>
    /// Gets the regex for validating text with whitespace.
    /// </summary>
    public static Regex AlphaWhiteRegex { get; } = GenerateAlphaWhiteRegex();

    /// <summary>
    /// Gets the regex for validating alphanumeric text with whitespace.
    /// </summary>
    public static Regex AlphaNumWhiteRegex { get; } = GenerateAlphaNumWhiteRegex();

    /// <summary>
    /// Gets the regex for validating simple single line text.
    /// </summary>
    public static Regex SimpleSlTextRegex { get; } = GenerateSimpleSlTextRegex();

    /// <summary>
    /// Gets the regex for validating simple multiline text.
    /// </summary>
    public static Regex SimpleMlTextRegex { get; } = GenerateSimpleMlTextRegex();

    /// <summary>
    /// Gets the regex for validating complex single line text.
    /// </summary>
    public static Regex ComplexSlTextRegex { get; } = GenerateComplexSlTextRegex();

    /// <summary>
    /// Gets the regex for validating complex multiline text.
    /// </summary>
    public static Regex ComplexMlTextRegex { get; } = GenerateComplexMlTextRegex();

    /// <summary>
    /// Gets the regex for validating untrimmed text.
    /// </summary>
    public static Regex UntrimmedRegex { get; } = GenerateUntrimmedRegex();

    [GeneratedRegex(EmailPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateEmailRegex();

    [GeneratedRegex(PhonePattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GeneratePhoneRegex();

    [GeneratedRegex(NumericPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateNumericRegex();

    [GeneratedRegex(AlphaPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateAlphaRegex();

    [GeneratedRegex(AlphaWhitePattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateAlphaWhiteRegex();

    [GeneratedRegex(AlphaNumWhitePattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateAlphaNumWhiteRegex();

    [GeneratedRegex(SimpleSlTextPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateSimpleSlTextRegex();

    [GeneratedRegex(SimpleMlTextPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateSimpleMlTextRegex();

    [GeneratedRegex(ComplexSlTextPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateComplexSlTextRegex();

    [GeneratedRegex(ComplexMlTextPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateComplexMlTextRegex();

    [GeneratedRegex(UntrimmedPattern, RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex GenerateUntrimmedRegex();
}
