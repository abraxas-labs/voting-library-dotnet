// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
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
            MessageType = "1710967",
        };

        var messageIdProviderMock = new Mock<IEchMessageIdProvider>();
        messageIdProviderMock
            .Setup(x => x.NewId())
            .Returns("constant-message-id");

        var headerProvider = new DeliveryHeaderProvider(config, new MockedClock(), messageIdProviderMock.Object);
        headerProvider.BuildHeader().ShouldMatchSnapshot();
    }

    [Fact]
    public void BuildHeaderWithTestDeliveryFlagShouldWork()
    {
        var config = new EchConfig
        {
            Product = "Voting.Lib.Tests",
            SenderId = "Voting.Lib.Tests.Sender",
            TestDeliveryFlag = false,
            ProductVersion = "0.1.2",
            MessageType = "1710967",
        };

        var messageIdProviderMock = new Mock<IEchMessageIdProvider>();
        messageIdProviderMock
            .Setup(x => x.NewId())
            .Returns("constant-message-id");

        var headerProvider = new DeliveryHeaderProvider(config, new MockedClock(), messageIdProviderMock.Object);
        headerProvider.BuildHeader(true).ShouldMatchSnapshot();
    }

    [Fact]
    public void LongProductVersionShouldHaveMaxLength()
    {
        var config = new EchConfig
        {
            Product = "Voting.Lib.Tests",
            SenderId = "Voting.Lib.Tests.Sender",
            ProductVersion = "0.1.2-a-very-long-string",
            MessageType = "1710967",
        };

        var headerProvider = new DeliveryHeaderProvider(config, new MockedClock(), new DefaultEchMessageIdProvider());
        var header = headerProvider.BuildHeader();
        header.SendingApplication.ProductVersion.Should().Be("0.1.2-a-ve");
    }
}
