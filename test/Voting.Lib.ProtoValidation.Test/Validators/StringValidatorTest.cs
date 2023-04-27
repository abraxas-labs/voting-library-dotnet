// (c) Copyright 2022 by Abraxas Informatik AG
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
    [InlineData("-1")]
    [InlineData("5a")]
    [InlineData("1 0")]
    [InlineData("a3")]
    [InlineData("0_")]
    public void InvalidNumericShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Numeric = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not numeric.");
    }

    [Theory]
    [InlineData("HelloWörld")]
    public void ValidAlphaShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { Alpha = true }), input);
    }

    [Theory]
    [InlineData("Space not allowed")]
    [InlineData("Hello\nWorld")]
    [InlineData("12x")]
    [InlineData("Hello_World")]
    public void InvalidAlphaShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { Alpha = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphabetic.");
    }

    [Theory]
    [InlineData("Space allowed")]
    public void ValidAlphaWhiteShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { AlphaWhite = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld")]
    [InlineData("12x")]
    [InlineData("Hello_World")]
    public void InvalidAlphaWhiteShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { AlphaWhite = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphabetic with Whitespace.");
    }

    [Theory]
    [InlineData("Text 12x")]
    public void ValidAlphaNumWhiteShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { AlphaNumWhite = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld")]
    [InlineData("Hello_World")]
    public void InvalidAlphaNumWhiteShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { AlphaNumWhite = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not alphanumeric with Whitespace.");
    }

    [Theory]
    [InlineData("Hello '301' - World.")]
    public void ValidSimpleSlShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { SimpleSlText = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld")]
    [InlineData("Hello\rWorld")]
    [InlineData("Hello  \bWorld")]
    [InlineData("\nHello World")]
    [InlineData("Hello World\n")]
    [InlineData(" Hello World")]
    [InlineData("Hello World ")]
    [InlineData("Hello%World")]
    [InlineData("Hello\\World")]
    [InlineData("Hello$World")]
    [InlineData("Hello<World")]
    [InlineData("Hello>World")]
    [InlineData("Hello_World")]
    [InlineData("Hello^World")]
    [InlineData("Hello!World")]
    [InlineData("Hello?World")]
    [InlineData("Hello+World")]
    [InlineData("Hello,World")]
    [InlineData("Hello:World")]
    [InlineData("Hello(World")]
    [InlineData("Hello)World")]
    public void InvalidSimpleSlShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { SimpleSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Simple Singleline Text.");
    }

    [Theory]
    [InlineData("Hello\nWorld\nHow is the - weather 'today'.")]
    [InlineData("Hello\rWorld")]
    public void ValidSimpleMlShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { SimpleMlText = true }), input);
    }

    [Theory]
    [InlineData("Hello  \bWorld")]
    [InlineData("\nHello World")]
    [InlineData("Hello World\n")]
    [InlineData(" Hello World")]
    [InlineData("Hello World ")]
    [InlineData("Hello%World")]
    [InlineData("Hello\\World")]
    [InlineData("Hello$World")]
    [InlineData("Hello<World")]
    [InlineData("Hello>World")]
    [InlineData("Hello_World")]
    [InlineData("Hello^World")]
    [InlineData("Hello!World")]
    [InlineData("Hello?World")]
    [InlineData("Hello+World")]
    [InlineData("Hello,World")]
    [InlineData("Hello:World")]
    [InlineData("Hello(World")]
    [InlineData("Hello)World")]
    public void InvalidSimpleMlShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { SimpleMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Simple Multiline Text.");
    }

    [Theory]
    [InlineData("Hello  \bWorld.")]
    [InlineData("\nHello World.")]
    [InlineData("Hello World.\n")]
    [InlineData(" Hello World.")]
    [InlineData("Hello World. ")]
    [InlineData("Hello%World")]
    [InlineData("Hello\\World")]
    [InlineData("Hello$World")]
    [InlineData("Hello<World")]
    [InlineData("Hello>World")]
    [InlineData("Hello^World")]
    public void InvalidComplexMlTextShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { ComplexMlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Complex Multiline Text.");
    }

    [Theory]
    [InlineData("Hello\nWorld.\nHow is the 120 _!?+-@,.:'()/— weather today")]
    [InlineData("Hello\rWorld.")]
    [InlineData("«Hello&\"World;»")]
    public void ValidComplexMlTextShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { ComplexMlText = true }), input);
    }

    [Theory]
    [InlineData("Hello\nWorld.")]
    [InlineData("Hello  \bWorld.")]
    [InlineData("Hello\rWorld.")]
    [InlineData("\nHello World.")]
    [InlineData("Hello World.\n")]
    [InlineData(" Hello World.")]
    [InlineData("Hello World. ")]
    [InlineData("Hello%World")]
    [InlineData("Hello\\World")]
    [InlineData("Hello$World")]
    [InlineData("Hello<World")]
    [InlineData("Hello>World")]
    [InlineData("Hello^World")]
    public void InvalidComplexSlTextShouldFail(string input)
    {
        var failure = Validate(BuildRules(new() { ComplexSlText = true }), input).Single();
        failure.ErrorMessage.Should().Be($"'{FieldName}' is not a Complex Singleline Text.");
    }

    [Theory]
    [InlineData("Hello _!?+-@,.:'()/— 120 World.")]
    [InlineData("«Hello&\"World;»")]
    public void ValidComplexSlTextShouldWork(string input)
    {
        ShouldHaveNoFailures(BuildRules(new() { ComplexSlText = true }), input);
    }

    private Rules BuildRules(StringRules rules) => new() { String = rules };
}
