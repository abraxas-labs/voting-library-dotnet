// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Abraxas.Voting.Validation.V1;
using FluentAssertions;
using Voting.Lib.ProtoValidation.Validators;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test.Validators;

public class StringValidatorTest : ProtoValidatorBaseTest
{
    public override IProtoFieldValidator Validator => new StringValidator();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EmptyWithAllowEmptyShouldWork(object? input)
    {
        ShouldHaveNoFailures(BuildRules(new() { AllowEmpty = true }), input);
    }

    [Fact]
    public void EmptyWithoutAllowEmptyShouldThrow()
    {
        var failure = Validate(BuildRules(new() { AllowEmpty = false }), string.Empty).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is empty.");
    }

    [Fact]
    public void EmptyWithAllowEmptyAndAnyOtherStringRulesShouldWork()
    {
        ShouldHaveNoFailures(
            BuildRules(new()
            {
                AllowEmpty = true,
                Email = true,
                Guid = true,
                Phone = true,
                MinLength = 1,
                Regex = "H",
                ComplexMlText = true,
            }),
            string.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abxperson")]
    [InlineData("max.muster@test..ch")]
    [InlineData("maxmuster.com")]
    [InlineData("$A12345@example.com")]
    public void InvalidEmailShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Email = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a valid E-Mail Address.");
    }

    [Theory]
    [InlineData("max.muster@test.ch")]
    [InlineData("max+muster@test.ch")]
    [InlineData("__max+muster@example.com")]
    [InlineData("max/muster=mann@domain.de")]
    [InlineData("\"Max\\Muster\"@test.co.uk")]
    public void ValidEmailShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Email = true }), input);
    }

    [Theory]
    [InlineData("")]
    [InlineData("f4467e9-3c14e-442e-8047-2fcf068c87f5")]
    [InlineData("h4467e93-c14e-442e-8047-2fcf068c87f5")]
    public void InvalidGuidShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Guid = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a GUID.");
    }

    [Theory]
    [InlineData("f4467e93-c14e-442e-8047-2fcf068c87f5")]
    [InlineData("ab3dcedf-7908-4a65-a172-277ff3d59480")]
    public void ValidGuidShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Guid = true }), input);
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(2, "Z")]
    [InlineData(7, "Hello\n")]
    [InlineData(12, "Hello World")]
    public void InvalidMinLengthShouldFail(int minLength, string input)
    {
        var failure = Validate(BuildRules(new() { MinLength = minLength }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' has Length {input.Length}, but the MinLength is {minLength}");
    }

    [Theory]
    [InlineData(1, "Z")]
    [InlineData(2, "Z.")]
    [InlineData(7, "Hello\n.")]
    [InlineData(11, "Hello World")]
    public void ValidMinLengthShouldWork(int minLength, string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { MinLength = minLength }), input);
    }

    [Theory]
    [InlineData(1, "Ab")]
    [InlineData(2, "Ab ")]
    [InlineData(10, "Hello World")]
    public void InvalidMaxLengthShouldFail(int maxLength, string input)
    {
        var failure = Validate(BuildRules(new() { MaxLength = maxLength }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' has Length {input.Length}, but the MaxLength is {maxLength}");
    }

    [Theory]
    [InlineData(1, "A")]
    [InlineData(2, "")]
    [InlineData(11, "Hello World")]
    public void ValidMaxLengthShouldWork(int maxLength, string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { MaxLength = maxLength }), input);
    }

    [Theory]
    [InlineData("martin")]
    [InlineData("Mart1n")]
    [InlineData("Mart-n")]
    [InlineData("AMerika")]
    public void InvalidRegexShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Regex = "^M[a-z]+$" }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' does not match the Regex Pattern.");
    }

    [Theory]
    [InlineData("Martin")]
    [InlineData("Martha")]
    public void ValidRegexShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Regex = "^M[a-z]+$" }), input);
    }

    [Theory]
    [InlineData("071 6a0 01 05")]
    [InlineData("++41 72 000 64 71")]
    [InlineData("+41720006.71")]
    [InlineData("08-0 800 08 08")]
    public void InvalidPhoneShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Phone = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a valid Phone Number.");
    }

    [Theory]
    [InlineData("071 620 01 01")]
    [InlineData("0717991408")]
    [InlineData("+41 72 000 64 71")]
    [InlineData("+41720006471")]
    [InlineData("0800 800 08 08")]
    [InlineData("071 620 01 0")]
    [InlineData("071799140812")]
    public void ValidPhoneShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Phone = true }), input);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("5")]
    [InlineData("302")]
    [InlineData("01")]
    public void ValidNumericShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Numeric = true }), input);
    }

    [Theory]
    [InlineData("-1", "2D")]
    [InlineData("5a", "61")]
    [InlineData("1 0", "20")]
    [InlineData("a3", "61")]
    [InlineData("0__", "5F, 5F")]
    public void InvalidNumericShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { Numeric = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not numeric. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("HelloWörld")]
    public void ValidAlphaShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Alpha = true }), input);
    }

    [Theory]
    [InlineData("Space not allowed", "20, 20")]
    [InlineData("Hello\nWorld", "0A")]
    [InlineData("12x", "31, 32")]
    [InlineData("Hello__World", "5F, 5F")]
    public void InvalidAlphaShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { Alpha = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphabetic. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("Space allowed")]
    public void ValidAlphaWhiteShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { AlphaWhite = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld", "0A")]
    [InlineData("12x", "31, 32")]
    [InlineData("Hello__World", "5F, 5F")]
    public void InvalidAlphaWhiteShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { AlphaWhite = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphabetic with Whitespace. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("Text 12x")]
    public void ValidAlphaNumWhiteShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { AlphaNumWhite = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld", "0A")]
    [InlineData("Hello__World", "5F, 5F")]
    public void InvalidAlphaNumWhiteShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { AlphaNumWhite = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphanumeric with Whitespace. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("Hello '301' - World.")]
    public void ValidSimpleSlShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { SimpleSlText = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld", "0A")]
    [InlineData("Hello\rWorld", "0D")]
    [InlineData("Hello  \bWorld", "08")]
    [InlineData("Hello%World", "25")]
    [InlineData("Hello\\World", "5C")]
    [InlineData("Hello$World", "24")]
    [InlineData("Hello<World", "3C")]
    [InlineData("Hello>World", "3E")]
    [InlineData("Hello_World", "5F")]
    [InlineData("Hello^World", "5E")]
    [InlineData("Hello!World", "21")]
    [InlineData("Hello?World", "3F")]
    [InlineData("Hello+World", "2B")]
    [InlineData("Hello,World", "2C")]
    [InlineData("Hello:World", "3A")]
    [InlineData("Hello(World", "28")]
    [InlineData("Hello()World", "28, 29")]
    public void InvalidSimpleSlShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { SimpleSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Simple Singleline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData(" Hello World")]
    [InlineData("Hello World ")]
    public void UntrimmedSimpleSlShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { SimpleSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
    }

    [Theory]
    [InlineData("\nHello World", "0A")]
    [InlineData("Hello World!\n", "21, 0A")]
    public void UntrimmedAndInvalidSimpleSlShouldFail(string input, string expectedInvalidChars)
    {
        var failures = Validate(BuildRules(new() { SimpleSlText = true }), input).ToList();
        failures.Should().HaveCount(2);
        failures[0].ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
        failures[1].ErrorMessage.Should().Be($"'{FieldName}' is not a Simple Singleline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("Hello\nWorld\nHow is the - weather 'today'.")]
    [InlineData("Hello\rWorld")]
    public void ValidSimpleMlShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { SimpleMlText = true }), input);
    }

    [Theory]
    [InlineData("Hello  \bWorld", "08")]
    [InlineData("Hello%World", "25")]
    [InlineData("Hello\\World", "5C")]
    [InlineData("Hello$World", "24")]
    [InlineData("Hello<World", "3C")]
    [InlineData("Hello>World", "3E")]
    [InlineData("Hello_World", "5F")]
    [InlineData("Hello^World", "5E")]
    [InlineData("Hello!World", "21")]
    [InlineData("Hello?World", "3F")]
    [InlineData("Hello+World", "2B")]
    [InlineData("Hello,World", "2C")]
    [InlineData("Hello:World", "3A")]
    [InlineData("Hello(World", "28")]
    [InlineData("Hello()World", "28, 29")]
    public void InvalidSimpleMlShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { SimpleMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Simple Multiline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("\nHello World")]
    [InlineData("Hello World\n")]
    [InlineData(" Hello World")]
    [InlineData("Hello World ")]
    public void UntrimmedSimpleMlShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { SimpleMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
    }

    [Theory]
    [InlineData("Hello  \bWorld.", "08")]
    [InlineData("Hello\\World", "5C")]
    [InlineData("Hello$World", "24")]
    [InlineData("Hello<World", "3C")]
    [InlineData("Hello>World", "3E")]
    [InlineData("Hello^$World", "5E, 24")]
    public void InvalidComplexMlTextShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { ComplexMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Complex Multiline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("\nHello World.")]
    [InlineData("Hello World.\n")]
    [InlineData(" Hello World.")]
    [InlineData("Hello World. ")]
    public void UntrimmedComplexMlTextShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { ComplexMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
    }

    [Theory]
    [InlineData("Hello\nWorld.\nHow is the 120 _!?+-@,.:'()/—–`´’‘+*%= weather today")]
    [InlineData("Hello\rWorld.")]
    [InlineData("«Hello&\"World;»")]
    public void ValidComplexMlTextShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { ComplexMlText = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld.", "0A")]
    [InlineData("Hello  \bWorld.", "08")]
    [InlineData("Hello\rWorld.", "0D")]
    [InlineData("Hello\\World", "5C")]
    [InlineData("Hello$World", "24")]
    [InlineData("Hello<World", "3C")]
    [InlineData("Hello>World", "3E")]
    [InlineData("Hello^World", "5E")]
    [InlineData("H\ne\bl\rl\\o$W<o>r^ld", "0A, 08, 0D, 5C, 24, 3C, 3E, 5E")]
    public void InvalidComplexSlTextShouldFail(string input, string expectedInvalidChars)
    {
        var failure = Validate(BuildRules(new() { ComplexSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Complex Singleline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData(" Hello World.")]
    [InlineData("Hello World. ")]
    public void UntrimmedComplexSlTextShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { ComplexSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
    }

    [Theory]
    [InlineData("\n\nHello World.", "0A, 0A")]
    [InlineData("Hello World.\n", "0A")]
    public void UntrimmedAndInvalidComplexSlTextShouldFail(string input, string expectedInvalidChars)
    {
        var failures = Validate(BuildRules(new() { ComplexSlText = true }), input).ToList();
        failures.Should().HaveCount(2);
        failures[0].ErrorMessage.Should().Be($"'{FieldName}' is not a trimmed Text, contains leading or trailing whitespace characters.");
        failures[1].ErrorMessage.Should().Be($"'{FieldName}' is not a Complex Singleline Text. Non-matching characters (UTF-8 hex): {expectedInvalidChars}");
    }

    [Theory]
    [InlineData("Hello _!?+-@,.:'()/—–`´’‘+*%= 120 World.")]
    [InlineData("«Hello&\"World;»")]
    public void ValidComplexSlTextShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { ComplexSlText = true }), input);
    }

    private Rules BuildRules(StringRules rules) => new() { String = rules };
}
