// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using eCH_0045_4_0;
using Voting.Lib.Ech.Ech0045.Models;

namespace Voting.Lib.Ech.Ech0045.Converter;

/// <summary>
/// Serializes data into eCH-0045.
/// </summary>
public class Ech0045Serializer
{
    // Needs to match the order of SwissPersonType.Extension
    private const int SwissAbroadExtensionXmlAttributeOrder = 6;
    private const string ExtensionXmlAttributeName = "extension";

    private readonly DeliveryHeaderProvider _deliveryHeaderProvider;
    private readonly EchSerializer _serializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ech0045Serializer"/> class.
    /// </summary>
    /// <param name="deliveryHeaderProvider">The delivery header provider.</param>
    /// <param name="serializer">The eCH serializer.</param>
    public Ech0045Serializer(DeliveryHeaderProvider deliveryHeaderProvider, EchSerializer serializer)
    {
        _deliveryHeaderProvider = deliveryHeaderProvider;
        _serializer = serializer;
    }

    /// <summary>
    /// Writes an eCH-0045 to the writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="voterList">The voter list without any voters.</param>
    /// <param name="persons">The voters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task WriteXml(PipeWriter writer, VoterListType voterList, IAsyncEnumerable<VotingPersonType> persons, CancellationToken ct)
    {
        var xmlData = VoterDelivery.Create(_deliveryHeaderProvider.BuildHeader(), voterList);
        voterList.Voter.Add(new VotingPersonType()); // add one prototype entry to be replaced
        return _serializer.WriteXmlWithElements(
            writer,
            Ech0045SerializerInfo.Voter,
            xmlData,
            persons,
            BuildXmlAttributeOverrides(),
            ct);
    }

    private XmlAttributeOverrides BuildXmlAttributeOverrides()
    {
        // ensure that eCH swiss abroad extension can be mapped, since in the contract the type is any.
        var xmlAttributeOverrides = new XmlAttributeOverrides();
        var swissPersonXmlAttributes = new XmlAttributes();
        var swissAbroadExtensionXmlAttribute = new XmlElementAttribute(ExtensionXmlAttributeName, typeof(object));
        swissAbroadExtensionXmlAttribute.Type = typeof(SwissAbroadPersonExtension);
        swissAbroadExtensionXmlAttribute.Order = SwissAbroadExtensionXmlAttributeOrder;
        swissPersonXmlAttributes.XmlElements.Add(swissAbroadExtensionXmlAttribute);
        xmlAttributeOverrides.Add(typeof(SwissPersonType), nameof(SwissPersonType.Extension), swissPersonXmlAttributes);
        return xmlAttributeOverrides;
    }
}
