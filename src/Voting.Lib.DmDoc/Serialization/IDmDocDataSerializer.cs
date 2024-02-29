// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Serialization;

/// <summary>
/// DmDoc data serializer interface.
/// </summary>
public interface IDmDocDataSerializer
{
    /// <summary>
    /// Gets the mime type of this serializer.
    /// </summary>
    string MimeType { get; }

    /// <summary>
    /// Serialize the data as string.
    /// </summary>
    /// <param name="data">The data to serialize.</param>
    /// <typeparam name="T">The type of the data to serialize.</typeparam>
    /// <returns>The serialized data.</returns>
    string Serialize<T>(T data);
}
