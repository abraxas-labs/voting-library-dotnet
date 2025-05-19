// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text;
using FluentAssertions;
using Google.Protobuf;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Protobuf;

public class ProtobufJsonSerializerTest
{
    private readonly ProtobufJsonSerializer _serializer;

    public ProtobufJsonSerializerTest()
    {
        _serializer = new(
            JsonFormatter.Default,
            JsonParser.Default,
            ProtobufTypeRegistry.CreateByScanningAssemblies([typeof(TestEvent).Assembly]),
            new TestMetadataProvider());
    }

    [Fact]
    public void SerializeAndDeserializeShouldWork()
    {
        var testEvent = new TestEvent { TestValue = 10 };
        var memory = _serializer.Serialize(testEvent);
        Encoding.UTF8.GetString(memory.ToArray()).Should().Be("{ \"testValue\": 10 }");

        var deserialized = _serializer.Deserialize(EventRecordTestUtil.BuildRecord(TestEvent.Descriptor.FullName, memory)) as TestEvent;
        deserialized.Should().NotBeNull();
        deserialized!.TestValue.Should().Be(testEvent.TestValue);
    }

    [Fact]
    public void DeserializeEventRecordShouldWork()
    {
        var protoMessage = (TestEvent)_serializer.Deserialize(EventRecordTestUtil.BuildRecord());
        protoMessage.TestValue.Should().Be(10);
    }

    [Fact]
    public void TryDeserializeWithMetadataShouldWork()
    {
        var ok = _serializer.TryDeserialize(EventRecordTestUtil.BuildRecord(true), out var result);
        ok.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(Guid.Parse("e2dd66f4-a932-44e5-bae4-28eb0bef8ef5"));
        ((TestEvent)result.Data).TestValue.Should().Be(10);
        ((TestEventMetadata)result.Metadata!).TestMetaValue.Should().Be(20);
    }

    [Fact]
    public void TryDeserializeWithoutEventMetadataShouldWork()
    {
        var ok = _serializer.TryDeserialize(EventRecordTestUtil.BuildRecord(), out var result);
        ok.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(Guid.Parse("e2dd66f4-a932-44e5-bae4-28eb0bef8ef5"));
        ((TestEvent)result.Data).TestValue.Should().Be(10);
        result.Metadata.Should().BeNull();
    }

    [Fact]
    public void TryDeserializeWithoutMetadataDescriptorShouldWork()
    {
        var serializer = new ProtobufJsonSerializer(
            JsonFormatter.Default,
            JsonParser.Default,
            ProtobufTypeRegistry.CreateByScanningAssemblies([typeof(TestEvent).Assembly]));
        var ok = serializer.TryDeserialize(EventRecordTestUtil.BuildRecord(true), out var result);
        ok.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Id.Should().Be(Guid.Parse("e2dd66f4-a932-44e5-bae4-28eb0bef8ef5"));
        ((TestEvent)result.Data).TestValue.Should().Be(10);
        result.Metadata.Should().BeNull();
    }

    [Fact]
    public void TryDeserializeShouldReturnFalseIfEventTypeUnknown()
    {
        var ok = _serializer.TryDeserialize(EventRecordTestUtil.BuildRecord(unknownEventType: true), out var result);
        ok.Should().BeFalse();
        result.Should().BeNull();
    }
}
