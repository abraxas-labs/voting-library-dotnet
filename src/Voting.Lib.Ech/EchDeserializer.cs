// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Voting.Lib.Common;

namespace Voting.Lib.Ech;

/// <summary>
/// Provides methods to deserialize eCH content.
/// </summary>
public class EchDeserializer
{
    /// <summary>
    /// Deserialize an XML to the provided type.
    /// </summary>
    /// <param name="xml">The serialized XML data.</param>
    /// <param name="schemaSet">The schema set to validate against.</param>
    /// <typeparam name="T">The type of the eCH object to deserialize.</typeparam>
    /// <returns>The eCH object.</returns>
    /// <exception cref="ValidationException">If deserialization returned null.</exception>
    /// <exception cref="Exception">If deserialization failed.</exception>
    public T DeserializeXml<T>(string xml, XmlSchemaSet schemaSet)
    {
        using var sr = new StringReader(xml);
        using var reader = XmlUtil.CreateReaderWithSchemaValidation(sr, schemaSet);
        return Deserialize<T>(reader);
    }

    /// <summary>
    /// Deserialize an XML to the provided type.
    /// </summary>
    /// <param name="stream">The serialized XML data as stream.</param>
    /// <param name="schemaSet">The schema set to validate against.</param>
    /// <typeparam name="T">The type of the eCH object to deserialize.</typeparam>
    /// <returns>The eCH object.</returns>
    /// <exception cref="ValidationException">If deserialization returned null.</exception>
    /// <exception cref="Exception">If deserialization failed.</exception>
    public T DeserializeXml<T>(Stream stream, XmlSchemaSet schemaSet)
    {
        using var reader = XmlUtil.CreateReaderWithSchemaValidation(stream, schemaSet);
        return Deserialize<T>(reader);
    }

    private T Deserialize<T>(XmlReader reader)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T?)serializer.Deserialize(reader)
                ?? throw new ValidationException("Deserialization returned null");
        }
        catch (InvalidOperationException ex) when (ex.InnerException != null)
        {
            // The XmlSerializer wraps all exceptions into an InvalidOperationException.
            // Unwrap it to surface the "correct" exception type.
            throw ex.InnerException;
        }
    }
}
