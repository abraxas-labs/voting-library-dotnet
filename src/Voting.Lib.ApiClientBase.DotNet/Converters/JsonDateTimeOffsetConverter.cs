// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Voting.Lib.ApiClientBase.DotNet.Converters;

/// <summary>
/// Converts a date time offset to and from JSON.
/// </summary>
public class JsonDateTimeOffsetConverter : JsonConverter
{
    private readonly ILogger<JsonDateTimeOffsetConverter>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDateTimeOffsetConverter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public JsonDateTimeOffsetConverter(ILogger<JsonDateTimeOffsetConverter>? logger = null)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer?.WriteValue(value?.ToString());
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = (string?)JToken.Load(reader);

        if (DateTimeOffset.TryParse(token, out var offsetDate))
        {
            return (DateTimeOffset?)offsetDate;
        }

        if (!DateTime.TryParse(token, out var dateTime))
        {
            _logger?.LogError("The DateTimeOffset {token} cannot be converted to type DateTime.", token);
        }

        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return (DateTimeOffset)dateTime;
    }

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTimeOffset?) || objectType == typeof(DateTimeOffset);
    }
}
