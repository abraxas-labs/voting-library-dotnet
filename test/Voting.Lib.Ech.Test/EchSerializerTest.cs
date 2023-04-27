// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using eCH_0007_6_0;
using eCH_0045_4_0;
using eCH_0058_5_0;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Ech.Extensions;
using Voting.Lib.Testing.Mocks;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Voting.Lib.Ech.Test;

public class EchSerializerTest
{
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fff";
    private readonly EchSerializer _serializer = new EchSerializer(NullLogger<EchSerializer>.Instance);

    [Fact]
    public async Task WriteXmlShouldWork()
    {
        using var ms = new MemoryStream();
        _serializer.WriteXml(ms, CreateDelivery(2, true));
        await ms.FlushAsync();

        var xml = Encoding.UTF8.GetString(ms.ToArray());
        xml.MatchXmlSnapshot();
    }

    [Fact]
    public async Task WriteXmlWithElementsShouldWork()
    {
        var delivery = CreateDelivery(2, false);

        var importer = new XmlReflectionImporter();
        var voter = importer.GetElementName(typeof(VotingPersonType), typeof(VoterListType), nameof(VoterListType.Voter));

        using var ms = new MemoryStream();
        await _serializer.WriteXmlWithElements(ms, voter, delivery, GenerateVoters(2));
        await ms.FlushAsync();

        var xml = Encoding.UTF8.GetString(ms.ToArray());
        xml.MatchXmlSnapshot();
    }

    private async IAsyncEnumerable<VotingPersonType> GenerateVoters(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await Task.Delay(10);
            yield return new();
        }
    }

    private VoterDelivery CreateDelivery(uint countOfVoters, bool addVoters)
    {
        var authority = AuthorityType.Create(CantonalRegisterType.Create("SG", "Ständiges Register", CantonAbbreviation.SG));
        var votersToAdd = addVoters ? (int)countOfVoters : 1; // always add 1 prototype
        var voters = Enumerable.Range(0, votersToAdd).Select(_ => new VotingPersonType()).ToList();
        var voterList = VoterListType.Create(authority, null, countOfVoters, voters);
        var deliveryHeader = new Header
        {
            Action = "1",
            MessageId = "123",
            SendingApplication = new SendingApplication
            {
                Manufacturer = "Abraxas",
                Product = "Voting.Lib.Tests",
                ProductVersion = "0.0.1-dev",
            },
            TestDeliveryFlag = true,
            SenderId = "ABX",
            MessageDate = MockedClock.UtcNowDate.ToString(DateFormat, CultureInfo.InvariantCulture),
            MessageType = "1234",
        };
        return VoterDelivery.Create(deliveryHeader, voterList);
    }
}
