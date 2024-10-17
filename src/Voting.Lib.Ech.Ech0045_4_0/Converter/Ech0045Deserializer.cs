// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Ech0045_4_0;
using Ech0058_5_0;
using Voting.Lib.Common;
using Voting.Lib.Ech.Ech0045_4_0.Schemas;
using Voting.Lib.Ech.Extensions;

namespace Voting.Lib.Ech.Ech0045_4_0.Converter;

/// <summary>
/// Since these xml files can get huge (according to business up to around 800MB),
/// we stream the voters out of the xml request stream directly into the database.
/// </summary>
public class Ech0045Deserializer
{
    private static readonly XmlReaderSettings XmlReaderSettings = new() { Async = true };

    public XmlReader BuildReader(Stream stream)
    {
        var schemaSet = Ech0045Schemas.LoadEch0045Schemas();
        return XmlUtil.CreateReaderWithSchemaValidation(stream, schemaSet, XmlReaderSettings);
    }

    /// <summary>
    /// Reads all voters from the xml stream.
    /// </summary>
    /// <param name="stream">Incoming data stream.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of voters.</returns>
    public async IAsyncEnumerable<(int Index, VotingPersonType Voter)> ReadVoters(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var schemaSet = Ech0045Schemas.LoadEch0045Schemas();
        using var reader = XmlUtil.CreateReaderWithSchemaValidation(stream, schemaSet, XmlReaderSettings);

        await foreach (var voter in ReadVoters(reader, cancellationToken))
        {
            yield return voter;
        }
    }

    /// <summary>
    /// Reads all voters from the XML reader.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of voters.</returns>
    public async IAsyncEnumerable<(int Index, VotingPersonType Voter)> ReadVoters(XmlReader reader, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var i = 0;
        await foreach (var voter in reader.EnumerateElementsAsync<VotingPersonType>(Ech0045SerializerInfo.Voter, cancellationToken))
        {
            yield return (i, voter);
            i++;
        }
    }

    /// <summary>
    /// Reads the delivery header from the XML reader.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The delivery header.</returns>
    public async Task<HeaderType> ReadDeliveryHeader(XmlReader reader, CancellationToken cancellationToken)
    {
        var name = Ech0045SerializerInfo.DeliveryHeader;

        var overrides = new XmlAttributes
        {
            XmlRoot = new XmlRootAttribute
            {
                ElementName = name.Name,
                Namespace = name.Namespace,
            },
            XmlType = new XmlTypeAttribute
            {
                TypeName = "headerType",
                Namespace = "http://www.ech.ch/xmlns/eCH-0058/5",
            },
        };

        var xmlAttrOverrides = new XmlAttributeOverrides();
        xmlAttrOverrides.Add(typeof(HeaderType), overrides);
        var serializer = new XmlSerializer(typeof(HeaderType), xmlAttrOverrides);

        await reader.MoveToElementAsync(Ech0045SerializerInfo.DeliveryHeader);
        return await reader.DeserializeElement<HeaderType>(serializer, cancellationToken);
    }
}
