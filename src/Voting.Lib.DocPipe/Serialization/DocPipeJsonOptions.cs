// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text.Json;
using System.Text.Json.Serialization;
using Voting.Lib.Common.Json;

namespace Voting.Lib.DocPipe.Serialization;

internal static class DocPipeJsonOptions
{
    internal static readonly JsonSerializerOptions Instance = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonSnakeCaseNamingPolicy.Instance,
    };
}
