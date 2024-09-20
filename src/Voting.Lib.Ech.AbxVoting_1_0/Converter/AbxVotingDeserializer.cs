// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using ABX_Voting_1_0;
using Voting.Lib.Common;
using Voting.Lib.Ech.AbxVoting_1_0.Schemas;
using Voting.Lib.Ech.Extensions;

namespace Voting.Lib.Ech.AbxVoting_1_0.Converter;

public class AbxVotingDeserializer
{
    private static readonly XmlReaderSettings XmlReaderSettings = new() { Async = true };

    public async IAsyncEnumerable<PersonInfoType> ReadVoters(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var schemaSet = AbxVotingSchemas.LoadAbxVotingSchemas();
        using var reader = XmlUtil.CreateReaderWithSchemaValidation(stream, schemaSet, XmlReaderSettings);

        await foreach (var voter in reader.EnumerateElementsAsync<PersonInfoType>(AbxVotingSerializerInfo.Person, cancellationToken))
        {
            yield return voter;
        }
    }
}
