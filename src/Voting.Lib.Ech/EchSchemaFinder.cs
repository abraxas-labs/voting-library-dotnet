// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Voting.Lib.Ech;

/// <summary>
/// Ech schema finder.
/// </summary>
public static class EchSchemaFinder
{
    /// <summary>
    /// Gets a schema which matches the xml root namespace.
    /// </summary>
    /// <param name="xmlStream">The xml stream.</param>
    /// <param name="validSchemas">A list of valid schemas.</param>
    /// <returns>A matching schema or null.</returns>
    public static string? GetSchema(Stream xmlStream, IReadOnlyCollection<string> validSchemas)
    {
        if (validSchemas.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Valid schema may not contain empty schemas");
        }

        var rootNamespace = GetRootNamespace(xmlStream);
        if (string.IsNullOrWhiteSpace(rootNamespace))
        {
            return null;
        }

        return validSchemas.FirstOrDefault(rootNamespace.Contains);
    }

    private static string GetRootNamespace(Stream xmlStream)
    {
        if (!xmlStream.CanSeek)
        {
            throw new InvalidOperationException("Cannot read the XML if the stream is not seekable");
        }

        var originalPosition = xmlStream.Position;
        try
        {
            xmlStream.Seek(0, SeekOrigin.Begin);
            using var reader = XmlReader.Create(xmlStream);
            reader.MoveToContent();
            return reader.NamespaceURI;
        }
        finally
        {
            xmlStream.Seek(originalPosition, SeekOrigin.Begin);
        }
    }
}
