// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Voting.Lib.Common.Json;

/// <summary>
/// A JSON converter to convert enums by their snake_case name.
/// </summary>
/// <typeparam name="TEnum">The enum to convert.</typeparam>
public class JsonEnumSnakeCaseConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = new();
    private readonly Dictionary<string, TEnum> _stringToEnum = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonEnumSnakeCaseConverter{TEnum}"/> class.
    /// </summary>
    public JsonEnumSnakeCaseConverter()
    {
        foreach (var value in Enum.GetValues<TEnum>())
        {
            var name = Enum.GetName(value)!;
            var snakeCased = SnakeCaseConverter.ConvertToSnakeCase(name);

            _enumToString.Add(value, snakeCased);
            _stringToEnum.Add(snakeCased, value);
        }
    }

    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = reader.TokenType;
        if (type != JsonTokenType.String)
        {
            return default;
        }

        var stringValue = reader.GetString()!;
        return _stringToEnum.TryGetValue(stringValue, out var enumValue)
            ? enumValue
            : throw new InvalidOperationException($"Could not deserialize {stringValue} into an enum of type {typeof(TEnum).Name}");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_enumToString.TryGetValue(value, out var stringValue))
        {
            writer.WriteStringValue(stringValue);
        }
        else
        {
            throw new InvalidOperationException($"Could not serialize {value} of type {typeof(TEnum).Name} to a string");
        }
    }
}
