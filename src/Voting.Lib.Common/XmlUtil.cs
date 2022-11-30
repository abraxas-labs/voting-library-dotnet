// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Voting.Lib.Common;

/// <summary>
/// Utilities related to XML.
/// </summary>
public static class XmlUtil
{
    /// <summary>
    /// Creates an XML reader which validates and XML against a set of schemas and throws an exception if the XML is invalid.
    /// </summary>
    /// <param name="xmlTextReader">The XML to validate.</param>
    /// <param name="schemas">The schemas to validate against.</param>
    /// <param name="settings">The optional XML reader settings.</param>
    /// <returns>The XML reader with schema validation enabled.</returns>
    /// <exception cref="XmlSchemaException">In case the XML is invalid.</exception>
    public static XmlReader CreateReaderWithSchemaValidation(
        TextReader xmlTextReader,
        XmlSchemaSet schemas,
        XmlReaderSettings? settings = null)
    {
        var xmlReaderSettings = CreateXmlSettings(schemas, settings);
        return XmlReader.Create(xmlTextReader, xmlReaderSettings);
    }

    /// <summary>
    /// Creates an XML reader which validates and XML against a set of schemas and throws an exception if the XML is invalid.
    /// </summary>
    /// <param name="xmlStream">The XML to validate.</param>
    /// <param name="schemas">The schemas to validate against.</param>
    /// <param name="settings">The optional XML reader settings.</param>
    /// <returns>The XML reader with schema validation enabled.</returns>
    /// <exception cref="XmlSchemaException">In case the XML is invalid.</exception>
    public static XmlReader CreateReaderWithSchemaValidation(
        Stream xmlStream,
        XmlSchemaSet schemas,
        XmlReaderSettings? settings = null)
    {
        var xmlReaderSettings = CreateXmlSettings(schemas, settings);
        return XmlReader.Create(xmlStream, xmlReaderSettings);
    }

    private static XmlReaderSettings CreateXmlSettings(XmlSchemaSet schemas, XmlReaderSettings? settings)
    {
        settings ??= new XmlReaderSettings();
        settings.ValidationType = ValidationType.Schema;
        settings.Schemas = schemas;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationEventHandler += (_, args) =>
        {
            // Only throw exceptions in error cases. We may get warnings when elements cannot be validated because of missing
            // schemas, which is often the case in eCH extensions.
            if (args.Severity == XmlSeverityType.Error)
            {
                throw args.Exception;
            }
        };

        return settings;
    }
}
