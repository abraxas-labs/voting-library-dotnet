// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Serialization.Xml;

/// <inheritdoc />
public class DmDocXmlDataSerializer : IDmDocDataSerializer
{
    /// <inheritdoc />
    public string MimeType => "application/xml";

    /// <inheritdoc />
    public string Serialize<T>(T data)
        => DmDocXmlSerializer.Serialize(data);
}
