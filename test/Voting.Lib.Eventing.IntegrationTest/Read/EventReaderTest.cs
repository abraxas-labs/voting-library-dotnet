// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using FluentAssertions;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Read;
using Xunit;

namespace Voting.Lib.Eventing.IntegrationTest.Read;

public class EventReaderTest : IClassFixture<EventStoreSampleDataFixture>
{
    private readonly IEventReader _eventReader;
    private readonly IEventSerializer _serializer;

    public EventReaderTest(EventStoreSampleDataFixture fixture)
    {
        _eventReader = fixture.GetService<IEventReader>();
        _serializer = fixture.GetService<IEventSerializer>();
    }

    [Fact]
    public async Task ReadEventsShouldReturnEvents()
    {
        var events = await _eventReader.ReadEvents(MockEvents.TestStream1, TestEventMetadata.Descriptor).ToListAsync();
        events.Should().HaveCount(100);
        ((TestEvent2)events[0].Data).TestValue2.Should().Be(0);
        ((TestEvent)events[1].Data).TestValue.Should().Be(1);
        ((TestEventMetadata)events[1].Metadata!).TestMetaValue.Should().Be(11);
    }

    [Fact]
    public async Task ReadEventsWithUnknownsShouldReturnKnownEvents()
    {
        var events = await _eventReader.ReadEvents(MockEvents.UnknownEventsStream1, TestEventMetadata.Descriptor).ToListAsync();
        events.Should().HaveCount(20);
        ((TestEvent2)events[0].Data).TestValue2.Should().Be(0);
        ((TestEvent)events[1].Data).TestValue.Should().Be(1);
        ((TestEventMetadata)events[1].Metadata!).TestMetaValue.Should().Be(11);
    }

    [Fact]
    public Task ReadEventsWithUnknownsNotIgnoringUnknownsShouldThrow()
    {
        return Assert.ThrowsAsync<UnknownEventException>(async () => await _eventReader.ReadEvents(MockEvents.UnknownEventsStream1, TestEventMetadata.Descriptor, false).ToListAsync());
    }

    [Fact]
    public Task ReadEventsShouldThrowNotFoundForUnknownStream()
    {
        return Assert.ThrowsAsync<StreamNotFoundException>(async () => await _eventReader.ReadEvents("unknown").ToListAsync());
    }

    [Fact]
    public async Task ReadEventsFromAllWithEndCondition()
    {
        var testEvents = await _eventReader.ReadEventsFromAll(Position.Start, x => x.Data is TestEvent { TestValue: >= 20 })
            .Select(x => x.Data)
            .OfType<TestEvent>()
            .ToListAsync();
        testEvents.Should().HaveCount(17);
    }

    [Fact]
    public async Task ReadEventsFromAllShouldIgnoreUnknown()
    {
        var allAreTestEvents = await _eventReader.ReadEventsFromAll(Position.Start, _ => false)
            .Select(x => x.Data is TestEvent or TestEvent2)
            .AllAsync(x => x);
        allAreTestEvents.Should().BeTrue();
    }

    [Fact]
    public Task ReadEventsFromAllShouldThrowOnUnknown()
    {
        return Assert.ThrowsAsync<UnknownEventException>(async () => await _eventReader.ReadEventsFromAll(Position.Start, _ => false, null, false).ToListAsync());
    }

    [Fact]
    public async Task ReadEventsFromAllShouldRespectStartPosition()
    {
        var startEvent = await _eventReader.ReadEventsFromAll(Position.Start, x => x.Data is TestEvent { TestValue: >= 20 }).LastAsync();
        var firstEvent = await _eventReader.ReadEventsFromAll(startEvent.Position, _ => false).FirstAsync();
        firstEvent.Id.Should().Be(startEvent.Id);
    }

    [Fact]
    public async Task ReadEventsFromAllWithEventTypeFilter()
    {
        var events = await _eventReader.ReadEventsFromAll(Position.Start, new[] { typeof(TestEvent) }, _ => false).ToListAsync();
        events.Should().HaveCount(150);
    }

    [Fact]
    public async Task GetLatestEventFromStreamShouldWork()
    {
        var latestEvent = await _eventReader.GetLatestEvent(MockEvents.TestStream1);
        latestEvent.Should().NotBeNull();
        latestEvent!.EventNumber.Should().Be(99);

        var deserialized = _serializer.Deserialize(latestEvent) as TestEvent;
        deserialized.Should().NotBeNull();
        deserialized!.TestValue.Should().Be(99);
    }

    [Fact]
    public async Task GetLatestEventFromAllShouldWork()
    {
        var latestEvent = await _eventReader.GetLatestEvent(WellKnownStreams.All);
        latestEvent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetLatestEventNumberFromStreamShouldWork()
    {
        var position = await _eventReader.GetLatestEventNumber(MockEvents.TestStream1);
        position.HasValue.Should().BeTrue();
        position!.Value.Should().Be(99);
    }

    [Fact]
    public async Task GetLatestEventNumberFromAllShouldWork()
    {
        var position = await _eventReader.GetLatestEventNumber(WellKnownStreams.All);
        position.HasValue.Should().BeTrue();

        // the value of the position is not deterministic due to eventstore internal generated events
    }

    [Fact]
    public async Task GetLatestEventNumberFromUnknownShouldReturnNull()
    {
        var position = await _eventReader.GetLatestEventNumber("unknown");
        position.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestPositionFromStreamShouldWork()
    {
        var position = await _eventReader.GetLatestEventPosition(MockEvents.TestStream1);
        position.HasValue.Should().BeTrue();
        position!.Value.CommitPosition.Should().BeGreaterThan(0);
        position.Value.PreparePosition.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetLatestPositionFromAllShouldWork()
    {
        var position = await _eventReader.GetLatestEventPosition(WellKnownStreams.All);
        position.HasValue.Should().BeTrue();
        position!.Value.CommitPosition.Should().BeGreaterThan(0);
        position.Value.PreparePosition.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetLatestPositionFromUnknownShouldReturnNull()
    {
        var position = await _eventReader.GetLatestEventPosition("unknown");
        position.Should().BeNull();
    }
}
