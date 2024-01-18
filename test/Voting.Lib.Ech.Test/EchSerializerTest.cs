// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ech0007_6_0;
using Ech0045_4_0;
using Ech0058_5_0;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Ech.Extensions;
using Voting.Lib.Testing.Mocks;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Voting.Lib.Ech.Test;

public class EchSerializerTest
{
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
        var cantonalRegister = new CantonalRegisterType
        {
            RegisterIdentification = "SG",
            CantonAbbreviation = CantonAbbreviationType.Sg,
            RegisterName = "Ständiges Register",
        };

        var authority = new AuthorityType
        {
            CantonalRegister = cantonalRegister,
        };

        var votersToAdd = addVoters ? (int)countOfVoters : 1; // always add 1 prototype
        var voterList = new VoterListType
        {
            ReportingAuthority = authority,
            Contest = null,
            NumberOfVoters = countOfVoters.ToString(),
        };

        foreach (var voter in Enumerable.Range(0, votersToAdd).Select(_ => new VotingPersonType()))
        {
            voterList.Voter.Add(voter);
        }

        var deliveryHeader = new HeaderType
        {
            Action = ActionType.Item1,
            MessageId = "123",
            SendingApplication = new SendingApplicationType
            {
                Manufacturer = "Abraxas",
                Product = "Voting.Lib.Tests",
                ProductVersion = "0.0.1-dev",
            },
            TestDeliveryFlag = true,
            SenderId = "ABX",
            MessageDate = MockedClock.UtcNowDate,
            MessageType = "1234",
        };
        return new VoterDelivery
        {
            DeliveryHeader = deliveryHeader,
            VoterList = voterList,
        };
    }
}
