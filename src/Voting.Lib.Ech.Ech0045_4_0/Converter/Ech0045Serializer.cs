// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ech0045_4_0;
using Voting.Lib.Ech.Ech0045_4_0.Models;

namespace Voting.Lib.Ech.Ech0045_4_0.Converter;

/// <summary>
/// Serializes data into eCH-0045.
/// </summary>
public class Ech0045Serializer
{
    // Needs to match the order of SwissPersonType.Extension
    private const int PersonExtensionXmlAttributeOrder = 5;
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
    /// <param name="leaveWriterOpen">Whether to leave the writer open.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task WriteXml(
        PipeWriter writer,
        VoterListType voterList,
        IAsyncEnumerable<VotingPersonType> persons,
        bool leaveWriterOpen,
        CancellationToken ct)
    {
        var xmlData = new VoterDelivery
        {
            DeliveryHeader = _deliveryHeaderProvider.BuildHeader(),
            VoterList = voterList,
        };

        voterList.Voter.Add(new VotingPersonType()); // add one prototype entry to be replaced
        return _serializer.WriteXmlWithElements(
            writer,
            Ech0045SerializerInfo.Voter,
            xmlData,
            persons,
            leaveWriterOpen,
            BuildXmlAttributeOverrides(),
            ct);
    }

    /// <summary>
    /// Writes an eCH-0045 delivery to xml bytes.
    /// </summary>
    /// <param name="delivery">The eCH-0045 delivery.</param>
    /// <returns>The xml bytes.</returns>
    public byte[] ToXmlBytes(VoterDelivery delivery)
    {
        using var memoryStream = new MemoryStream();
        _serializer.WriteXml(memoryStream, delivery, BuildXmlAttributeOverrides());
        return memoryStream.ToArray();
    }

    private XmlAttributeOverrides BuildXmlAttributeOverrides()
    {
        // ensure that eCH swiss abroad extension can be mapped, since in the contract the type is any.
        var xmlAttributeOverrides = new XmlAttributeOverrides();
        var swissPersonXmlAttributes = new XmlAttributes();

        var swissExtensionXmlAttribute = new XmlElementAttribute(ExtensionXmlAttributeName, typeof(object));
        swissExtensionXmlAttribute.Type = typeof(SwissPersonExtension);
        swissExtensionXmlAttribute.Order = PersonExtensionXmlAttributeOrder;
        swissPersonXmlAttributes.XmlElements.Add(swissExtensionXmlAttribute);

        xmlAttributeOverrides.Add(typeof(PersonType), nameof(SwissPersonType.Extension), swissPersonXmlAttributes);
        return xmlAttributeOverrides;
    }
}
