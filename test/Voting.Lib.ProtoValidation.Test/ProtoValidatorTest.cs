// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Test.Messages;
using Xunit;

namespace Voting.Lib.ProtoValidation.Test;

public class ProtoValidatorTest
{
    private readonly ProtoValidator _validator;

    public ProtoValidatorTest()
    {
        _validator = new ServiceCollection()
            .AddProtoValidators()
            .AddSingleton<ProtoValidator>()
            .BuildServiceProvider()
            .GetRequiredService<ProtoValidator>();
    }

    [Fact]
    public void GreetWithValidNameShouldWork()
    {
        var msg = new Greet { Name = "Abraxas" };
        _validator.Validate(msg);
    }

    [Fact]
    public void GreetWithInvalidNameShouldThrow()
    {
        var msg = new Greet { Name = "aBraxas" };
        ShouldThrowWithErrorMessage(msg, "'Name' does not match the Regex Pattern.");
    }

    [Fact]
    public void ComplexWithValidValuesShouldWork()
    {
        var msg = NewValidComplexObject();
        _validator.Validate(msg);
    }

    [Fact]
    public void ComplexEmptyShouldThrow()
    {
        var msg = new Complex();
        ShouldThrowWithErrorMessage(msg, "'Id' is not a GUID.\n'Contact' is required.\n'State' is Unspecified.");
    }

    [Fact]
    public void ComplexWithNestedInvalidPropertiesShouldThrow()
    {
        var msg = NewValidComplexObject(c =>
        {
            c.Contact.Email = "hello@abraxas..ch";
            c.Contact.Phone = "058 660 00 0p";
            c.Contact.Company = new()
            {
                Name = "Abraxas\nInformatik",
            };
        });

        ShouldThrowWithErrorMessage(msg, "'Email' is not a valid E-Mail Address.\n'Phone' is not a valid Phone Number.\n'Name' is not a Complex Singleline Text.");
    }

    [Fact]
    public void ComplexWithInvalidPropertyAndMultipleValidatorsShouldThrow()
    {
        var msg = NewValidComplexObject(c => c.ParentId = "f4467e9-3c14e-442e-8047-2fcf068c87f5");
        ShouldThrowWithErrorMessage(msg, "'ParentId' is not a GUID.");
    }

    [Fact]
    public void ComplexWithInvalidPropertyAndWrapperShouldThrow()
    {
        var msg = NewValidComplexObject(c =>
        {
            c.Contact.Age = -1;
            c.Contact.Height = -0.1;
        });
        ShouldThrowWithErrorMessage(msg, "'Age' is smaller than the MinValue 0\n'Height' is smaller than the MinValue 0");
    }

    [Fact]
    public void RepeatWithValidValuesShouldWork()
    {
        var msg = NewValidRepeatObject();
        _validator.Validate(msg);
    }

    [Fact]
    public void RepeatWithInvalidPropertiesShouldThrow()
    {
        var msg = NewValidRepeatObject(c =>
        {
            c.ExternIds.Add("invalid-guid");
            c.ExternIds.Add("aa19444c-9c57-4384-9f08-9b706b5e1388");
            c.ExternIds.Add(string.Empty);
        });

        ShouldThrowWithErrorMessage(msg, "'ExternIds[0]' is not a GUID.\n'ExternIds[2]' is not a GUID.");
    }

    [Fact]
    public void RepeatWithInvalidNestedPropertiesShouldThrow()
    {
        var msg = NewValidRepeatObject(c =>
        {
            c.Children.Add(new RepeatChild { Description = "Nan\bme" });
            c.Children.Add(new RepeatChild { Description = "Max Mustermann" });
            c.Children.Add(new RepeatChild { Description = "peter" });
            c.Children.Add(new RepeatChild { Description = "Jochen" });
            c.Children.Add(new RepeatChild { Description = "Urs" });
            c.Children.Add(new RepeatChild { Description = string.Empty });
        });

        ShouldThrowWithErrorMessage(msg, "'Description' is not a Complex Singleline Text.\n'Description' has Length 3, but the MinLength is 4\n'Description' has Length 0, but the MinLength is 4\n'Description' is not a Complex Singleline Text.");
    }

    [Fact]
    public void MapWithValidValuesShouldWork()
    {
        var msg = NewValidMapObject();
        _validator.Validate(msg);
    }

    [Fact]
    public void MapWithInvalidValuesShouldThrow()
    {
        var msg = NewValidMapObject(m =>
        {
            m.Translations.Add("x", "This string is to long");
            m.Mapping.Add(1, new() { Description = string.Empty });
        });

        ShouldThrowWithErrorMessage(msg, "'Translations[x].key' has Length 1, but the MinLength is 2\n'Translations[x].value' has Length 22, but the MaxLength is 6\n'Description' has Length 0, but the MinLength is 4\n'Description' is not a Complex Singleline Text.");
    }

    private Complex NewValidComplexObject(Action<Complex>? action = null)
    {
        var complex = new Complex
        {
            Id = Guid.Empty.ToString(),
            Contact = new Contact
            {
                Email = "abraxas@abraxas.ch",
                Phone = "058 660 00 00",
                Company = new()
                {
                    Name = "Abraxas",
                },
            },
            State = State.Active,
        };

        action?.Invoke(complex);
        return complex;
    }

    private Repeat NewValidRepeatObject(Action<Repeat>? action = null)
    {
        var repeat = new Repeat
        {
            Id = Guid.Empty.ToString(),
        };

        action?.Invoke(repeat);
        return repeat;
    }

    private Map NewValidMapObject(Action<Map>? action = null)
    {
        var map = new Map
        {
            Translations =
            {
                { "de", "Wert" },
                { "en", "Val" },
            },
            Mapping =
            {
                { 0, new() { Description = "Test" } },
            },
        };

        action?.Invoke(map);
        return map;
    }

    private void ShouldThrowWithErrorMessage<TMessage>(TMessage message, string errorMessage)
        where TMessage : IMessage
    {
        FluentActions.Invoking(() => _validator.Validate(message))
            .Should()
            .Throw<ValidationException>()
            .WithMessage(errorMessage);
    }
}
