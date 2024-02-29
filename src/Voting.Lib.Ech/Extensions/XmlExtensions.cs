// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Voting.Lib.Ech.Extensions;

/// <summary>
/// Extensions to XML classes.
/// </summary>
public static class XmlExtensions
{
    /// <summary>
    /// Builds a <see cref="XmlQualifiedName"/> based on XML annotated types.
    /// </summary>
    /// <param name="importer">The reflection importer.</param>
    /// <param name="type">The type to create the name for.</param>
    /// <param name="containedType">The type which contains the actual type.</param>
    /// <param name="containedMemberName">The name of the member of the type in the container.</param>
    /// <returns>The <see cref="XmlQualifiedName"/>.</returns>
    public static XmlQualifiedName GetElementName(
        this XmlReflectionImporter importer,
        Type type,
        Type containedType,
        string containedMemberName)
    {
        var elAttr = containedType
            .GetProperty(containedMemberName)
            ?.GetCustomAttributes<XmlElementAttribute>()
            .Where(x => x.Type == null || x.Type == type)
            .MinBy(x => x.Type == type);

        var typeAttr = type
            .GetCustomAttributes<XmlTypeAttribute>()
            .FirstOrDefault();

        return elAttr == null
            ? GetElementName(importer, type)
            : new XmlQualifiedName(elAttr.ElementName, elAttr.Namespace ?? typeAttr?.Namespace ?? importer.ImportTypeMapping(type).Namespace);
    }

    /// <summary>
    /// Enumerates through all elements matching the <see cref="XmlQualifiedName"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="name">The name of the element to look up.</param>
    /// <param name="ct">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">The type to which found elements should be deserialized.</typeparam>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> enumerating through all found deserialized elements.</returns>
    public static async IAsyncEnumerable<T> EnumerateElementsAsync<T>(
        this XmlReader reader,
        XmlQualifiedName name,
        [EnumeratorCancellation] CancellationToken ct)
        where T : class
    {
        // since the XML root on the eCH generated classes from the eai team do not match
        // the element names used in the eCH object tree we need to override it here
        var overrides = new XmlAttributes
        {
            XmlRoot = new XmlRootAttribute
            {
                ElementName = name.Name,
                Namespace = name.Namespace,
            },
        };

        var xmlAttrOverrides = new XmlAttributeOverrides();
        xmlAttrOverrides.Add(typeof(T), overrides);

        var serializer = new XmlSerializer(typeof(T), xmlAttrOverrides);

        // deserialize element moves the cursor to the end of the element
        // therefore the current element could already be the next matching element
        // otherwise move to the next element
        while (reader.Matches(name) || await reader.MoveToElementAsync(name))
        {
            yield return await reader.DeserializeElement<T>(serializer, ct);
        }
    }

    internal static async Task<bool> MoveToElementAsync(this XmlReader reader, XmlQualifiedName name)
    {
        while (await reader.ReadToNodeTypeAsync(XmlNodeType.Element))
        {
            if (reader.Matches(name))
            {
                return true;
            }
        }

        return false;
    }

    private static XmlQualifiedName GetElementName(this XmlReflectionImporter importer, Type type)
    {
        var mapping = importer.ImportTypeMapping(type);
        return new XmlQualifiedName(mapping.ElementName, mapping.Namespace);
    }

    private static async Task<bool> ReadToNodeTypeAsync(this XmlReader reader, XmlNodeType nodeType)
    {
        while (await reader.ReadAsync())
        {
            if (reader.NodeType == nodeType)
            {
                return true;
            }
        }

        return false;
    }

    private static bool Matches(this XmlReader reader, XmlQualifiedName name)
        => name.Name.Equals(reader.LocalName, StringComparison.Ordinal)
            && name.Namespace.Equals(reader.NamespaceURI, StringComparison.Ordinal);

    private static async Task<T> DeserializeElement<T>(
        this XmlReader reader,
        XmlSerializer serializer,
        CancellationToken cancellationToken)
        where T : class
    {
        // since there is no async serializer we need to read async first to then deserialize synchronous
        var el = await XNode.ReadFromAsync(reader, cancellationToken);
        using var nodeReader = el.CreateReader();
        return serializer.Deserialize(nodeReader) as T
            ?? throw new ValidationException("Could not read XML");
    }
}
