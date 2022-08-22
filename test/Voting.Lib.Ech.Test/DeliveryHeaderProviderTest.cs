// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Moq;
using Snapper;
using Voting.Lib.Ech.Configuration;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Ech.Test;

public class DeliveryHeaderProviderTest
{
    [Fact]
    public void BuildHeaderShouldWork()
    {
        var config = new EchConfig
        {
            Product = "Voting.Lib.Tests",
            SenderId = "Voting.Lib.Tests.Sender",
            TestDeliveryFlag = true,
            ProductVersion = "0.1.2",
        };

        var messageIdProviderMock = new Mock<IEchMessageIdProvider>();
        messageIdProviderMock
            .Setup(x => x.NewId())
            .Returns("constant-message-id");

        var headerProvider = new DeliveryHeaderProvider(config, new MockedClock(), messageIdProviderMock.Object);
        headerProvider.BuildHeader().ShouldMatchSnapshot();
    }
}
