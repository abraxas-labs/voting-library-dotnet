// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

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
    // This method is exposed to public, so that it is easily possible to generate the DmDoc JSON
    // without actually calling DmDoc for testing purposes (used to send the JSON to the DmDoc team)
    public static string Serialize<T>(T data)
        => JsonSerializer.Serialize(
            data,
            DmDocJsonOptions.Instance);
}
