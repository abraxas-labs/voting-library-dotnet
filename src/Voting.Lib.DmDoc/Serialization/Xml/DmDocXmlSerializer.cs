// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Voting.Lib.DmDoc.Serialization.Xml;

/// <summary>
/// The DmDoc XML serializer.
/// </summary>
public static class DmDocXmlSerializer
{
    private static readonly Encoding Encoding = new UTF8Encoding(false);
    private static readonly XmlWriterSettings XmlSettings = new()
    {
        Indent = false,
        NewLineOnAttributes = false,
        Encoding = Encoding,
        OmitXmlDeclaration = true,
    };

    /// <summary>
    /// Serialize the data to XML.
    /// </summary>
    /// <param name="data">The data to serialize.</param>
    /// <typeparam name="T">The type of the data to serialize.</typeparam>
    /// <returns>The data, serialized as XML.</returns>
    // This method is exposed as public, so that it is easily possible to generate the DmDoc XML
    // without actually calling DmDoc for testing purposes (used to send the XML to the DmDoc team)
    public static string Serialize<T>(T data)
    {
        // absolutely no namespace declarations are allowed by dmDoc
        // this removes them completely
        // see https://stackoverflow.com/a/935749/3302887
        var emptyNamespaces = new XmlSerializerNamespaces();
        emptyNamespaces.Add(string.Empty, string.Empty);

        var serializer = new XmlSerializer(typeof(T));
        using var memoryStream = new MemoryStream();
        using var stringWriter = new StreamWriter(memoryStream, Encoding);
        using var xmlWriter = XmlWriter.Create(stringWriter, XmlSettings);
        serializer.Serialize(xmlWriter, data, emptyNamespaces);
        return Encoding.GetString(memoryStream.ToArray());
    }
}
