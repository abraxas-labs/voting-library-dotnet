// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Serialization.Json;

/// <inheritdoc />
public class DmDocJsonDataSerializer : IDmDocDataSerializer
{
    /// <inheritdoc />
    public string MimeType => "application/json";

    /// <inheritdoc />
    public string Serialize<T>(T data)
        => DmDocJsonSerializer.Serialize(data);
}
