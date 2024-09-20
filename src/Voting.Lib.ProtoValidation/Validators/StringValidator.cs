// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Abraxas.Voting.Validation.V1;

namespace Voting.Lib.ProtoValidation.Validators;

/// <summary>
/// A string proto validator.
/// </summary>
public class StringValidator : IProtoFieldValidator
{
    // copied from https://github.com/microsoft/referencesource/blob/5697c29004a34d80acdaf5742d7e699022c64ecd/System.ComponentModel.DataAnnotations/DataAnnotations/EmailAddressAttribute.cs#L54.
    private static readonly Regex _emailRegex = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _phoneRegex = new Regex(@"^\+?(?:[1-9]\s*){0,3}(\s)?(?:[0-9]\s?){7,14}\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _numericRegex = new Regex(@"^\d+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _alphaRegex = new Regex(@"^[\p{L}\p{M}]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _alphaWhiteRegex = new Regex(@"^[\p{L}\p{M} ]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _alphaNumWhiteRegex = new Regex(@"^[\p{L}\p{M}\p{Nd} ]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _simpleSlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd} \.'-]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _simpleMlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd} \.'\-\r\n]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _complexSlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd} _!\?+\-@,\.:'\(\)\/—\""«»;&–`´’‘\+\*%=]+\z", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500));

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _complexMlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd}\r\n _!\?+\-@,\.:'\(\)\/—\""«»;&–`´’‘\+\*%=]+\z", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500));

    private static readonly Regex _untrimmedRegex = new Regex(@"(^\s+)|(\s+\z)", RegexOptions.Compiled);

    /// <inheritdoc />
    public void Validate(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        var stringRules = rules.String;
        if (stringRules == null)
        {
            return;
        }

        var stringValue = value as string;

        if (value != null && stringValue == null)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, " is not a string."));
            return;
        }

        if (string.IsNullOrEmpty(stringValue) && stringRules.HasAllowEmpty)
        {
            if (stringRules.AllowEmpty)
            {
                return;
            }

            context.Failures.Add(new ProtoValidationError(fieldName, "is empty."));
        }

        if (stringRules.HasMinLength)
        {
            ValidateMinLength(context, fieldName, stringValue!, stringRules.MinLength);
        }

        if (stringRules.HasMaxLength)
        {
            ValidateMaxLength(context, fieldName, stringValue!, stringRules.MaxLength);
        }

        if (stringRules.Guid)
        {
            ValidateGuid(context, fieldName, stringValue!);
        }

        if (stringRules.HasRegex)
        {
            ValidateRegex(context, fieldName, stringValue!, stringRules.Regex);
        }

        if (stringRules.Numeric)
        {
            ValidateNumeric(context, fieldName, stringValue!);
        }

        if (stringRules.Email)
        {
            ValidateEmail(context, fieldName, stringValue!);
        }

        if (stringRules.Phone)
        {
            ValidatePhone(context, fieldName, stringValue!);
        }

        if (stringRules.Alpha)
        {
            ValidateAlphabetic(context, fieldName, stringValue!);
        }

        if (stringRules.AlphaWhite)
        {
            ValidateAlphabeticWithWhitespace(context, fieldName, stringValue!);
        }

        if (stringRules.AlphaNumWhite)
        {
            ValidateAlphanumericWithWhitespace(context, fieldName, stringValue!);
        }

        if (stringRules.SimpleSlText)
        {
            ValidateSimpleSinglelineText(context, fieldName, stringValue!);
        }

        if (stringRules.SimpleMlText)
        {
            ValidateSimpleMultilineText(context, fieldName, stringValue!);
        }

        if (stringRules.ComplexSlText)
        {
            ValidateComplexSinglelineText(context, fieldName, stringValue!);
        }

        if (stringRules.ComplexMlText)
        {
            ValidateComplexMultilineText(context, fieldName, stringValue!);
        }
    }

    private void ValidateGuid(ProtoValidationContext context, string fieldName, string value)
    {
        if (!Guid.TryParse(value, out _))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a GUID."));
        }
    }

    private void ValidateMinLength(ProtoValidationContext context, string fieldName, string value, int minLength)
    {
        if (value.Length < minLength)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"has Length {value.Length}, but the MinLength is {minLength}"));
        }
    }

    private void ValidateMaxLength(ProtoValidationContext context, string fieldName, string value, int maxLength)
    {
        if (value.Length > maxLength)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, $"has Length {value.Length}, but the MaxLength is {maxLength}"));
        }
    }

    private void ValidateEmail(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _emailRegex, "is not a valid E-Mail Address.", false, false);

    private void ValidatePhone(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _phoneRegex, "is not a valid Phone Number.", false, false);

    private void ValidateRegex(ProtoValidationContext context, string fieldName, string value, string regexString)
        => ValidateString(context, fieldName, value, new Regex(regexString), "does not match the Regex Pattern.", false, false);

    private void ValidateNumeric(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _numericRegex, "is not numeric.", false);

    private void ValidateAlphabetic(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _alphaRegex, "is not alphabetic.", false);

    private void ValidateAlphabeticWithWhitespace(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _alphaWhiteRegex, "is not alphabetic with Whitespace.", false);

    private void ValidateAlphanumericWithWhitespace(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _alphaNumWhiteRegex, "is not alphanumeric with Whitespace.", false);

    private void ValidateSimpleSinglelineText(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _simpleSlTextRegex, "is not a Simple Singleline Text.", true);

    private void ValidateSimpleMultilineText(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _simpleMlTextRegex, "is not a Simple Multiline Text.", true);

    private void ValidateComplexSinglelineText(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _complexSlTextRegex, "is not a Complex Singleline Text.", true);

    private void ValidateComplexMultilineText(ProtoValidationContext context, string fieldName, string value)
        => ValidateString(context, fieldName, value, _complexMlTextRegex, "is not a Complex Multiline Text.", true);

    private void ValidateString(ProtoValidationContext context, string fieldName, string value, Regex regex, string validationMessage, bool requireTrimmed, bool reportInvalidChars = true)
    {
        if (requireTrimmed)
        {
            EnsureNoUntrimmedWhitespace(context, fieldName, value);
        }

        if (regex.Match(value).Success)
        {
            return;
        }

        if (reportInvalidChars)
        {
            var invalidChars = GetNonMatchingCharactersAsUtf8Hex(value, regex);
            if (invalidChars.Count > 0)
            {
                context.Failures.Add(new ProtoValidationError(fieldName, $"{validationMessage} Non-matching characters (UTF-8 hex): {string.Join(", ", invalidChars)}"));
                return;
            }
        }

        context.Failures.Add(new ProtoValidationError(fieldName, validationMessage));
    }

    private void EnsureNoUntrimmedWhitespace(ProtoValidationContext context, string fieldName, string value)
    {
        if (_untrimmedRegex.Match(value).Success)
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a trimmed Text, contains leading or trailing whitespace characters."));
        }
    }

    private List<string> GetNonMatchingCharactersAsUtf8Hex(string value, Regex regex)
    {
        var nonMatchingCharsHex = new List<string>();
        var utf8Encoding = System.Text.Encoding.UTF8;

        foreach (char c in value)
        {
            if (!regex.IsMatch(c.ToString()))
            {
                var utf8Bytes = utf8Encoding.GetBytes(new[] { c });
                var hexString = BitConverter.ToString(utf8Bytes).Replace("-", " ");
                nonMatchingCharsHex.Add(hexString);
            }
        }

        return nonMatchingCharsHex;
    }
}
