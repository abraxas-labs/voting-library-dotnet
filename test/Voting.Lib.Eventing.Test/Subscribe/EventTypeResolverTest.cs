// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Subscribe;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Subscribe;

public class EventTypeResolverTest
{
    [Fact]
    public void GetDescriptorNamesShouldWork()
    {
        new EventTypeResolver(ProtobufTypeRegistry.CreateByScanningAssemblies(new[] { typeof(TestEvent).Assembly }), NullLogger<EventTypeResolver>.Instance)
            .GetDescriptorNames(new[] { typeof(TestEvent), typeof(TestEventMetadata) })
            .Should()
            .BeEquivalentTo(TestEvent.Descriptor.FullName, TestEventMetadata.Descriptor.FullName);
    }
}
