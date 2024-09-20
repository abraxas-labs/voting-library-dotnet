// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text.Json;

namespace Voting.Lib.DmDoc.Serialization.Json;

/// <summary>
/// The DmDoc JSON serializer.
/// </summary>
public static class DmDocJsonSerializer
{
    /// <summary>
    /// Serializes DmDoc data to JSON.
    /// </summary>
    /// <param name="data">The data to serialize.</param>
    /// <typeparam name="T">The type of the data to serialize.</typeparam>
    /// <returns>The data serialized as JSON.</returns>
    public static string Serialize<T>(T data)
        => JsonSerializer.Serialize(data, DmDocJsonOptions.Instance);

    /// <summary>
    /// Deserializes JSON to DmDoc data.
    /// </summary>
    /// <param name="json">The JSON to deserialize.</param>
    /// <typeparam name="T">The type of the data to deserialize.</typeparam>
    /// <returns>The deserialized data.</returns>
    public static T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, DmDocJsonOptions.Instance)
            ?? throw new InvalidOperationException($"{json} deserialized to null");
}
