// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Linq;
using FluentAssertions;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Protobuf;

public class ProtobufMessageUtilsTest
{
    [Fact]
    public void MessageDescriptorsOfAssembliesShouldDiscoverProto()
    {
        var descriptors = ProtobufMessageUtils.MessageDescriptorsOfAssemblies(new[] { typeof(TestEvent).Assembly }).ToList();
        descriptors.Should().HaveCount(3);
        descriptors.OrderBy(x => x.Name).Should().BeEquivalentTo(new[] { TestEvent.Descriptor, TestEvent2.Descriptor, TestEventMetadata.Descriptor });
    }
}
