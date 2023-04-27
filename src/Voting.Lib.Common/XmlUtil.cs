// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
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

    /// <summary>
    /// Creates a <see cref="XmlSchemaSet"/> by reading XML schemas from the file system.
    /// </summary>
    /// <param name="schemas">The path of the schemas by their name.</param>
    /// <param name="dir">The directory of the schema, if relative it is expanded based on the current domain base directory.</param>
    /// <returns>The <see cref="XmlSchemaSet"/>.</returns>
    public static XmlSchemaSet LoadSchemas(IReadOnlyDictionary<string, string> schemas, string dir = "Schemas")
    {
        var schemaDirectory = Path.IsPathRooted(dir)
            ? dir
            : Path.Join(AppDomain.CurrentDomain.BaseDirectory, dir);
        var xmlSchemaSet = new XmlSchemaSet();
        foreach (var (schemaName, schemaFileName) in schemas)
        {
            var schemaPath = Path.Join(schemaDirectory, schemaFileName);
            using var xmlReader = XmlReader.Create(schemaPath);
            xmlSchemaSet.Add(schemaName, xmlReader);
        }

        return xmlSchemaSet;
    }

    /// <summary>
    /// Creates a <see cref="XmlSchemaSet"/> by reading XML schemas from the resources of an assembly.
    /// </summary>
    /// <param name="schemas">The path of the schemas by their name.</param>
    /// <typeparam name="T">The type of which the assembly is used to load the resources. The schema names are prefixed by the namespace of this type.</typeparam>
    /// <returns>The <see cref="XmlSchemaSet"/>.</returns>
    public static XmlSchemaSet LoadSchemasFromResources<T>(IReadOnlyDictionary<string, string> schemas)
    {
        var xmlSchemaSet = new XmlSchemaSet();
        foreach (var (schemaName, schemaFileName) in schemas)
        {
            using var stream = typeof(T).Assembly.GetManifestResourceStream(typeof(T), schemaFileName)
                ?? throw new InvalidOperationException($"Could not find xml schema {schemaFileName} in type {typeof(T).FullName}");
            using var xmlReader = XmlReader.Create(stream);
            xmlSchemaSet.Add(schemaName, xmlReader);
        }

        return xmlSchemaSet;
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
