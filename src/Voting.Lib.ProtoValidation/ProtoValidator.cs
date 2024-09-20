// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abraxas.Voting.Validation.V1;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Voting.Lib.ProtoValidation.Validators;

namespace Voting.Lib.ProtoValidation;

/// <summary>
/// Proto validator.
/// </summary>
public class ProtoValidator
{
    private const string ErrorSeparator = "\n";

    private readonly IEnumerable<IProtoFieldValidator> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtoValidator"/> class.
    /// </summary>
    /// <param name="validators">Proto field validators.</param>
    public ProtoValidator(IEnumerable<IProtoFieldValidator> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Validates the proto message.
    /// </summary>
    /// <typeparam name="TMessage">Type of message.</typeparam>
    /// <param name="message">The message.</param>
    public void Validate<TMessage>(TMessage message)
        where TMessage : IMessage
    {
        var context = new ProtoValidationContext();
        Validate(context, message);

        if (context.Failures.Count > 0)
        {
            var errorMessage = string.Join(ErrorSeparator, context.Failures.Select(f => f.ErrorMessage));
            throw new ValidationException(errorMessage);
        }
    }

    private void Validate<TMessage>(ProtoValidationContext context, TMessage message)
        where TMessage : IMessage
    {
        var descriptor = message.Descriptor;

        foreach (var fieldDescriptor in descriptor.Fields.InFieldNumberOrder())
        {
            ValidateField(context, GetRules(fieldDescriptor), fieldDescriptor.Accessor.GetValue(message), fieldDescriptor.PropertyName);
        }
    }

    private void ValidateField(ProtoValidationContext context, Rules? fieldRules, object? fieldValue, string fieldName)
    {
        if (fieldValue is IList list)
        {
            // validate repeated field.
            ValidateList(context, list, fieldRules, fieldName);
            return;
        }

        if (fieldValue is IDictionary map)
        {
            ValidateMap(context, map, fieldRules, fieldName);
            return;
        }

        if (fieldValue is IMessage complexObject)
        {
            // validate nested fields of the field.
            Validate(context, complexObject);
        }

        if (fieldRules != null)
        {
            // validate the field itself.
            ApplyValidators(context, fieldRules, fieldValue, fieldName);
        }
    }

    private void ValidateList(ProtoValidationContext context, IList list, Rules? fieldRules, string fieldName)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var listElement = list[i];

            if (fieldRules != null)
            {
                // validate the list element with the validation rules on the repeat field.
                ApplyValidators(context, fieldRules, listElement, $"{fieldName}[{i}]");
            }

            if (listElement is IMessage message)
            {
                // validate the nested fields of the list element.
                Validate(context, message);
            }
        }
    }

    private void ValidateMap(ProtoValidationContext context, IDictionary map, Rules? fieldRules, string fieldName)
    {
        foreach (DictionaryEntry mapEntry in map)
        {
            if (fieldRules != null)
            {
                // validate the key and value foreach map entry with the rules set on rules.map.key / rules.map.value
                var keyValueFieldName = $"{fieldName}[{mapEntry.Key}]";
                var keyRules = BuildRules(fieldRules.Map.Key);
                var valueRules = BuildRules(fieldRules.Map.Value);

                if (keyRules != null)
                {
                    ApplyValidators(context, keyRules, mapEntry.Key, $"{keyValueFieldName}.key");
                }

                if (valueRules != null)
                {
                    ApplyValidators(context, valueRules, mapEntry.Value, $"{keyValueFieldName}.value");
                }
            }

            // validate the properties of a map entry value with the validation rules of the object itself if it is a complex proto message.
            // keys may only contain simple data and do not need nested validation (https://developers.google.com/protocol-buffers/docs/proto3#maps)
            if (mapEntry.Value is IMessage mapEntryValueMessage)
            {
                Validate(context, mapEntryValueMessage);
            }
        }
    }

    private void ApplyValidators(ProtoValidationContext context, Rules rules, object? value, string fieldName)
    {
        foreach (var validator in _validators)
        {
            validator.Validate(context, rules, value, fieldName);
        }
    }

    private Rules? GetRules(FieldDescriptor fieldDescriptor)
    {
        return fieldDescriptor.GetOptions()?.GetExtension<Rules>(RulesExtensions.Rules);
    }

    private Rules? BuildRules(MapEntryComponentRules? mapEntryComponentRules)
        => mapEntryComponentRules == null
            ? null
            : new()
            {
                String = mapEntryComponentRules.String,
            };
}
