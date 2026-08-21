// (c) Copyright by Abraxas Informatik AG
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
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Voting.Lib.Ech.Extensions;

/// <summary>
/// Extensions to XML classes.
/// </summary>
public static class XmlExtensions
{
    private static readonly XName XsiTypeAttributeName = XNamespace.Get(XmlSchema.InstanceNamespace) + "type";

    /// <summary>
    /// Builds a <see cref="XmlQualifiedName"/> based on XML annotated types.
    /// </summary>
    /// <param name="importer">The reflection importer.</param>
    /// <param name="type">The type to create the name for.</param>
    /// <param name="containedType">The type which contains the actual type.</param>
    /// <param name="containedMemberName">The name of the member of the type in the container.</param>
    /// <param name="customNamespace">The custom namespace of the XML element.</param>
    /// <returns>The <see cref="XmlQualifiedName"/>.</returns>
    public static XmlQualifiedName GetElementName(
        this XmlReflectionImporter importer,
        Type type,
        Type containedType,
        string containedMemberName,
        string? customNamespace = null)
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
            : new XmlQualifiedName(elAttr.ElementName, customNamespace ?? elAttr.Namespace ?? typeAttr?.Namespace ?? importer.ImportTypeMapping(type).Namespace);
    }

    /// <summary>
    /// Enumerates through all elements matching the <see cref="XmlQualifiedName"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="name">The name of the element to look up.</param>
    /// <param name="ct">The <see cref="CancellationToken"/>.</param>
    /// <param name="xmlAttributeOverrides">Optional additional XML attribute overrides (eg. for extensions).</param>
    /// <param name="extraTypes">Optional additional types to register with the serializer (eg. for extensions).</param>
    /// <typeparam name="T">The type to which found elements should be deserialized.</typeparam>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> enumerating through all found deserialized elements.</returns>
    public static IAsyncEnumerable<T> EnumerateElementsAsync<T>(
        this XmlReader reader,
        XmlQualifiedName name,
        CancellationToken ct,
        XmlAttributeOverrides? xmlAttributeOverrides = null,
        Type[]? extraTypes = null)
        where T : class
    {
        var serializer = BuildElementSerializer<T>(name, xmlAttributeOverrides, extraTypes);
        return reader.EnumerateElementsCore<T>(name, _ => serializer, ct);
    }

    /// <summary>
    /// Enumerates through all elements matching the <see cref="XmlQualifiedName"/>, selecting the <see cref="XmlSerializer"/> to use for each element individually based on its buffered content
    /// (eg. to support multiple possible types of an extension element).
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="name">The name of the element to look up.</param>
    /// <param name="serializerSelector">Selects the <see cref="XmlSerializer"/> to use for each element based on its buffered content.</param>
    /// <param name="ct">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">The type to which found elements should be deserialized.</typeparam>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> enumerating through all found deserialized elements.</returns>
    public static IAsyncEnumerable<T> EnumerateElementsAsync<T>(
        this XmlReader reader,
        XmlQualifiedName name,
        Func<XElement, XmlSerializer> serializerSelector,
        CancellationToken ct)
        where T : class
        => reader.EnumerateElementsCore<T>(name, serializerSelector, ct);

    /// <summary>
    /// Builds an <see cref="XmlSerializer"/> for <typeparamref name="T"/>, overriding its XML root element name
    /// since the XML root on the eCH generated classes from the eai team do not match the element names used in the eCH object tree.
    /// </summary>
    /// <param name="name">The element name and namespace to deserialize/serialize as.</param>
    /// <param name="xmlAttributeOverrides">Optional additional XML attribute overrides (eg. for extensions). The root element name override is added to this instance, which is mutated as a result.</param>
    /// <param name="extraTypes">Optional additional types to register with the serializer (eg. for polymorphic xsi:type extensions).</param>
    /// <typeparam name="T">The type to build the serializer for.</typeparam>
    /// <returns>The built <see cref="XmlSerializer"/>.</returns>
    public static XmlSerializer BuildElementSerializer<T>(
        XmlQualifiedName name,
        XmlAttributeOverrides? xmlAttributeOverrides = null,
        Type[]? extraTypes = null)
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

        var xmlAttrOverrides = xmlAttributeOverrides ?? new XmlAttributeOverrides();
        xmlAttrOverrides.Add(typeof(T), overrides);

        return new XmlSerializer(typeof(T), xmlAttrOverrides, extraTypes ?? Type.EmptyTypes, null, null);
    }

    /// <summary>
    /// Move the underlying stream of the reader to the name of the element to look up.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="name">The name of the element to look up.</param>
    /// <returns>Whether the name of the element to look up to was found or not.</returns>
    public static async Task<bool> MoveToElementAsync(this XmlReader reader, XmlQualifiedName name)
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

    /// <summary>
    /// Deserializes the XML element.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="serializer">The <see cref="XmlSerializer"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">The type to which found elements should be deserialized.</typeparam>
    /// <returns>The deserialized XML element.</returns>
    /// <exception cref="ValidationException">If the XML element could not be deserialized.</exception>
    public static Task<T> DeserializeElement<T>(
        this XmlReader reader,
        XmlSerializer serializer,
        CancellationToken cancellationToken)
        where T : class
        => reader.DeserializeElementCore<T>(_ => serializer, cancellationToken);

    /// <summary>
    /// Deserializes the XML element.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="serializerSelector">Selects the <see cref="XmlSerializer"/> to use based on the buffered element.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">The type to which found elements should be deserialized.</typeparam>
    /// <returns>The deserialized XML element.</returns>
    /// <exception cref="ValidationException">If the XML element could not be deserialized.</exception>
    public static Task<T> DeserializeElement<T>(
        this XmlReader reader,
        Func<XElement, XmlSerializer> serializerSelector,
        CancellationToken cancellationToken)
        where T : class
        => reader.DeserializeElementCore<T>(serializerSelector, cancellationToken);

    /// <summary>
    /// Gets the <c>xsi:type</c> name (as an <see cref="XName"/>) that a type is serialized with,
    /// based on its <see cref="XmlTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The type to get the xsi:type name for.</param>
    /// <returns>The xsi:type name.</returns>
    /// <exception cref="InvalidOperationException">If the type doesn't declare an <see cref="XmlTypeAttribute"/>.</exception>
    public static XName GetXsiTypeName(this Type type)
    {
        var xmlType = type.GetCustomAttribute<XmlTypeAttribute>()
            ?? throw new InvalidOperationException($"{type} must declare an {nameof(XmlTypeAttribute)} to be used as a polymorphic xsi:type.");
        return XNamespace.Get(xmlType.Namespace ?? string.Empty) + xmlType.TypeName;
    }

    /// <summary>
    /// Reads and resolves the <c>xsi:type</c> attribute of an <see cref="XElement"/>, if present.
    /// </summary>
    /// <param name="element">The element to read the xsi:type attribute from.</param>
    /// <returns>The resolved xsi:type name, or <c>null</c> if the element has no xsi:type attribute.</returns>
    /// <exception cref="ValidationException">If the xsi:type attribute value is malformed or its namespace prefix cannot be resolved.</exception>
    public static XName? GetXsiType(this XElement element)
    {
        var xsiType = element.Attribute(XsiTypeAttributeName)?.Value;
        if (string.IsNullOrWhiteSpace(xsiType))
        {
            return null;
        }

        var separatorIndex = xsiType.IndexOf(':');
        if (separatorIndex < 0)
        {
            return element.GetDefaultNamespace() + xsiType;
        }

        var prefix = xsiType[..separatorIndex];
        var localName = xsiType[(separatorIndex + 1)..];

        if (string.IsNullOrWhiteSpace(localName))
        {
            throw new ValidationException($"Invalid xsi:type value '{xsiType}'.");
        }

        var ns = element.GetNamespaceOfPrefix(prefix)
            ?? throw new ValidationException($"Could not resolve xsi:type namespace prefix '{prefix}'.");
        return ns + localName;
    }

    private static async IAsyncEnumerable<T> EnumerateElementsCore<T>(
        this XmlReader reader,
        XmlQualifiedName name,
        Func<XElement, XmlSerializer> serializerSelector,
        [EnumeratorCancellation] CancellationToken ct)
        where T : class
    {
        // deserialize element moves the cursor to the end of the element
        // therefore the current element could already be the next matching element
        // otherwise move to the next element
        while (reader.Matches(name) || await reader.MoveToElementAsync(name))
        {
            yield return await reader.DeserializeElement<T>(serializerSelector, ct);
        }
    }

    private static async Task<T> DeserializeElementCore<T>(
        this XmlReader reader,
        Func<XElement, XmlSerializer> serializerSelector,
        CancellationToken cancellationToken)
        where T : class
    {
        // since there is no async serializer we need to read async first to then deserialize synchronous
        var el = await XNode.ReadFromAsync(reader, cancellationToken) as XElement
            ?? throw new ValidationException("Could not read XML");
        using var nodeReader = el.CreateReader();

        return serializerSelector(el).Deserialize(nodeReader) as T
               ?? throw new ValidationException("Could not read XML");
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
}
