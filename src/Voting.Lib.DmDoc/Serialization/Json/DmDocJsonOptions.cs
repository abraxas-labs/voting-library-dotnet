// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json;
using System.Text.Json.Serialization;
using Voting.Lib.Common.Json;
using Voting.Lib.DmDoc.Models;

namespace Voting.Lib.DmDoc.Serialization.Json;

internal static class DmDocJsonOptions
{
    internal static readonly JsonSerializerOptions Instance = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonSnakeCaseNamingPolicy.Instance,
        Converters =
        {
            new JsonEnumSnakeCaseConverter<CallbackAction>(),
            new JsonStringEnumConverter(),
            JsonBase64ByteConverter.Instance,
        },
    };
}
