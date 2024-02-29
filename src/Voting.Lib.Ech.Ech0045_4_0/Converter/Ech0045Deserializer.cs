// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using Ech0045_4_0;
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

        var i = 0;
        await foreach (var voter in reader.EnumerateElementsAsync<VotingPersonType>(Ech0045SerializerInfo.Voter, cancellationToken))
        {
            yield return (i, voter);
            i++;
        }
    }
}
