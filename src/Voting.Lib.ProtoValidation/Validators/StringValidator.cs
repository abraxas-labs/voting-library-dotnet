// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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
    private static readonly Regex _complexSlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd} _!\?+\-@,\.:'\(\)\/—\""«»;&–`´\+\*]+\z", RegexOptions.Compiled);

    // copied from concept VO Ausmittlung - Daten Validierung
    private static readonly Regex _complexMlTextRegex = new Regex(@"^[\p{L}\p{M}\p{Nd}\r\n _!\?+\-@,\.:'\(\)\/—\""«»;&–`´\+\*]+\z", RegexOptions.Compiled);

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

    private void ValidateRegex(ProtoValidationContext context, string fieldName, string value, string regexString)
    {
        if (!MatchRegex(value, new Regex(regexString)))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "does not match the Regex Pattern."));
        }
    }

    private void ValidateEmail(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _emailRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a valid E-Mail Address."));
        }
    }

    private void ValidatePhone(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _phoneRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a valid Phone Number."));
        }
    }

    private void ValidateNumeric(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _numericRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not numeric."));
        }
    }

    private void ValidateAlphabetic(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _alphaRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not alphabetic."));
        }
    }

    private void ValidateAlphabeticWithWhitespace(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _alphaWhiteRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not alphabetic with Whitespace."));
        }
    }

    private void ValidateAlphanumericWithWhitespace(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _alphaNumWhiteRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not alphanumeric with Whitespace."));
        }
    }

    private void ValidateSimpleSinglelineText(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _simpleSlTextRegex) || MatchRegex(value, _untrimmedRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a Simple Singleline Text."));
        }
    }

    private void ValidateSimpleMultilineText(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _simpleMlTextRegex) || MatchRegex(value, _untrimmedRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a Simple Multiline Text."));
        }
    }

    private void ValidateComplexSinglelineText(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _complexSlTextRegex) || MatchRegex(value, _untrimmedRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a Complex Singleline Text."));
        }
    }

    private void ValidateComplexMultilineText(ProtoValidationContext context, string fieldName, string value)
    {
        if (!MatchRegex(value, _complexMlTextRegex) || MatchRegex(value, _untrimmedRegex))
        {
            context.Failures.Add(new ProtoValidationError(fieldName, "is not a Complex Multiline Text."));
        }
    }

    private bool MatchRegex(string value, Regex pattern)
    {
        return pattern.Match(value).Success;
    }
}
