// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Voting.Lib.ApiClientBase.DotNet.Converters;

/// <summary>
/// Converts a date time to and from JSON.
/// </summary>
public class JsonDateTimeConverter : JsonConverter
{
    private readonly ILogger<JsonDateTimeConverter>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDateTimeConverter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public JsonDateTimeConverter(ILogger<JsonDateTimeConverter>? logger = null)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader?.Value == null)
        {
            return null;
        }

        var token = (string?)JToken.Load(reader);
        if (DateTime.TryParse(token, out var dateTime))
        {
            return dateTime;
        }
        else
        {
            _logger?.LogError("The DateTime {token} cannot be converted to type DateTimeOffset.", token);
            return DateTime.MinValue;
        }
    }

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime?) || objectType == typeof(DateTime);
    }
}
