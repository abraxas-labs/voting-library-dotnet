// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using FluentAssertions;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Protobuf;

public class ProtobufTypeRegistryTest
{
    private readonly ProtobufTypeRegistry _registry;

    public ProtobufTypeRegistryTest()
    {
        _registry = ProtobufTypeRegistry.CreateByScanningAssemblies(new[] { typeof(TestEvent).Assembly });
    }

    [Fact]
    public void FindByFullNameShouldWork()
    {
        _registry.Find(TestEvent.Descriptor.FullName).Should().Be(TestEvent.Descriptor);
    }

    [Fact]
    public void UnknownFindByFullNameShouldReturnNull()
    {
        _registry.Find("unknown").Should().BeNull();
    }

    [Fact]
    public void FindByClrTypeShouldWork()
    {
        _registry.Find(typeof(TestEvent)).Should().Be(TestEvent.Descriptor);
    }

    [Fact]
    public void UnknownFindByClrTypeShouldReturnNull()
    {
        _registry.Find(typeof(Version)).Should().BeNull();
    }
}
