// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Ech0045_4_0;
using Ech0045_VoterExtension_1_0;
using Voting.Lib.Ech.Ech0045_4_0.Models;
using Voting.Lib.Ech.Extensions;

namespace Voting.Lib.Ech.Ech0045_4_0.Converter;

/// <summary>
/// Builds the <see cref="XmlAttributeOverrides"/> and extra types needed to serialize and deserialize
/// <see cref="PersonType.Extension"/> as the requested <see cref="PersonExtensionKind"/>.
/// </summary>
internal static class Ech0045PersonExtensionOverrides
{
    // Needs to match the order of SwissPersonType.Extension
    private const int PersonExtensionXmlAttributeOrder = 5;
    private const string ExtensionXmlElementName = "extension";

    private static readonly ConcurrentDictionary<XmlQualifiedName, Func<XElement, XmlSerializer>> AutoDetectingSerializerSelectorCache = new();

    private static readonly IReadOnlyDictionary<PersonExtensionKind, Type> XsiTypeExtensionTypes = new Dictionary<PersonExtensionKind, Type>
    {
        [PersonExtensionKind.EVotingVoterExtension_1_0] = typeof(VoterExtensionType),
    };

    internal static (XmlAttributeOverrides Overrides, Type[] ExtraTypes) BuildOverridesAndExtraTypes(PersonExtensionKind kind)
    {
        var element = new XmlElementAttribute(ExtensionXmlElementName, typeof(object))
        {
            Order = PersonExtensionXmlAttributeOrder,
        };

        Type[] extraTypes;

        if (kind == PersonExtensionKind.VotingVoterExtension)
        {
            // no xsi:type is written or expected
            element.Type = typeof(SwissPersonExtension);
            extraTypes = Type.EmptyTypes;
        }
        else
        {
            // Type is intentionally left unset, the concrete type is instead
            // registered as an extra type, which makes the serializer read/write an xsi:type attribute.
            extraTypes = new[] { XsiTypeExtensionTypes[kind] };
        }

        var attributes = new XmlAttributes();
        attributes.XmlElements.Add(element);

        var overrides = new XmlAttributeOverrides();
        overrides.Add(typeof(PersonType), nameof(SwissPersonType.Extension), attributes);
        return (overrides, extraTypes);
    }

    internal static Func<XElement, XmlSerializer> BuildAutoDetectingSerializerSelector(XmlQualifiedName name)
        => AutoDetectingSerializerSelectorCache.GetOrAdd(name, BuildAutoDetectingSerializerSelectorCore);

    private static Func<XElement, XmlSerializer> BuildAutoDetectingSerializerSelectorCore(XmlQualifiedName name)
    {
        var (directOverrides, directExtraTypes) = BuildOverridesAndExtraTypes(PersonExtensionKind.VotingVoterExtension);
        var directSerializer = XmlExtensions.BuildElementSerializer<VotingPersonType>(name, directOverrides, directExtraTypes);

        var serializersByXsiType = new Dictionary<XName, XmlSerializer>();
        foreach (var kind in XsiTypeExtensionTypes.Keys)
        {
            var (overrides, extraTypes) = BuildOverridesAndExtraTypes(kind);
            var serializer = XmlExtensions.BuildElementSerializer<VotingPersonType>(name, overrides, extraTypes);

            foreach (var extraType in extraTypes)
            {
                serializersByXsiType[extraType.GetXsiTypeName()] = serializer;
            }
        }

        return voter =>
        {
            var extension = voter.Elements().FirstOrDefault(e => e.Name.LocalName == ExtensionXmlElementName);
            var xsiType = extension?.GetXsiType();

            // The default VOTING eCH-0045 SwissPersonExtension doesn't have a xsiType.
            if (xsiType is null)
            {
                return directSerializer;
            }

            return serializersByXsiType.TryGetValue(xsiType, out var serializer)
                ? serializer
                : throw new ValidationException($"Person extension has unknown xsi:type '{xsiType}'.");
        };
    }
}
