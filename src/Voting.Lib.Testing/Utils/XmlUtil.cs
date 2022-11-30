// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// Utilities related to testing XML.
/// </summary>
public static class XmlUtil
{
    /// <summary>
    /// Formats XML to a human readable version.
    /// </summary>
    /// <param name="xml">The XML to format.</param>
    /// <returns>The formatted XML.</returns>
    public static string FormatTestXml(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
        catch (Exception)
        {
            return xml;
        }
    }

    /// <summary>
    /// Validates an XML against a set of schemas and throwns an exception if the XML is invalid.
    /// </summary>
    /// <param name="xml">The XML to validate.</param>
    /// <param name="schemas">The schemas to validate against.</param>
    /// <exception cref="XmlSchemaException">In case the XML is invalid.</exception>
    public static void ValidateSchema(string xml, XmlSchemaSet schemas)
    {
        using var sr = new StringReader(xml);
        using var xmlReader = Common.XmlUtil.CreateReaderWithSchemaValidation(sr, schemas);
        while (xmlReader.Read())
        {
            // Just read to the end of the file
        }
    }
}
