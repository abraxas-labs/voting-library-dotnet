// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Moq;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Read;
using Voting.Lib.Eventing.Seeding;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Seeding;

public class EventSeederTest
{
    [Fact]
    public async Task SeedShouldWorkWithoutExistingEvents()
    {
        var publisherMock = new Mock<IEventPublisher>();
        publisherMock
            .Setup(x => x.Publish("my-stream", It.IsAny<IEnumerable<EventWithMetadata>>(), null))
            .Returns(Task.CompletedTask);

        var eventReaderMock = new Mock<IEventReader>();
        eventReaderMock
            .Setup(x => x.GetLatestEventNumber("my-stream", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<StreamPosition?>(null));

        var seeder = new EventSeeder(publisherMock.Object, eventReaderMock.Object);
        var eventDatas = Enumerable.Range(0, 10).Select(i => new TestEvent { TestValue = i }).ToArray();
        await seeder.Seed("my-stream", eventDatas);

        eventReaderMock.Verify(x => x.GetLatestEventNumber("my-stream", It.IsAny<CancellationToken>()), Times.Once());
        publisherMock.Verify(
            x => x.Publish(
                "my-stream",
                It.Is<IEnumerable<EventWithMetadata>>(e => e.Count() == 10),
                null),
            Times.Once());
    }

    [Fact]
    public async Task SeedShouldWorkWithEventPosition()
    {
        var position = new StreamPosition(4);
        var publisherMock = new Mock<IEventPublisher>();
        publisherMock
            .Setup(x => x.Publish("my-stream", It.IsAny<IEnumerable<EventWithMetadata>>(), StreamRevision.FromStreamPosition(position)))
            .Returns(Task.CompletedTask);

        var eventReaderMock = new Mock<IEventReader>();
        eventReaderMock
            .Setup(x => x.GetLatestEventNumber("my-stream", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<StreamPosition?>(4));

        var seeder = new EventSeeder(publisherMock.Object, eventReaderMock.Object);
        var eventDatas = Enumerable.Range(0, 10).Select(i => new TestEvent { TestValue = i }).ToArray();
        await seeder.Seed("my-stream", eventDatas);

        eventReaderMock.Verify(x => x.GetLatestEventNumber("my-stream", It.IsAny<CancellationToken>()), Times.Once());
        publisherMock.Verify(
            x => x.Publish(
                "my-stream",
                It.Is<IEnumerable<EventWithMetadata>>(e => e.Count() == 5 && e.Select((ev, i) => ((TestEvent)ev.Data).TestValue == i + 5).All(i => i)),
                StreamRevision.FromStreamPosition(position)),
            Times.Once());
    }
}
