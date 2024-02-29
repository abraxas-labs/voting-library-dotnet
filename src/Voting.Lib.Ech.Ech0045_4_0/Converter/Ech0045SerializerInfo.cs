// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Xml;
using System.Xml.Serialization;
using Ech0045_4_0;
using Voting.Lib.Ech.Extensions;

namespace Voting.Lib.Ech.Ech0045_4_0.Converter;

internal static class Ech0045SerializerInfo
{
    internal static readonly XmlQualifiedName Voter;

    static Ech0045SerializerInfo()
    {
        // sadly the TypeMapping of the XmlMapping is internal to the dotnet framework
        // therefore we use our own name resolution here
        // works for the generated eCH classes, however it may not work with all edge cases correctly
        var importer = new XmlReflectionImporter();
        Voter = importer.GetElementName(typeof(VotingPersonType), typeof(VoterListType), nameof(VoterListType.Voter));
    }
}
