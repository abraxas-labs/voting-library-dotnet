// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace Voting.Lib.Eventing.IntegrationTest;

public class EventStoreSampleDataFixture : EventStoreFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await MockEvents.PublishEvents(ServiceProvider, MockEvents.TestStream1, 100, 5);
        await MockEvents.PublishEvents(ServiceProvider, MockEvents.TestStream2, 30, 3);

        await MockEvents.PublishEvents(ServiceProvider, MockEvents.UnknownEventsStream1, 10, 2);
        await MockEvents.PublishUnknownEvents(ServiceProvider, MockEvents.UnknownEventsStream1, 5);
        await MockEvents.PublishEvents(ServiceProvider, MockEvents.UnknownEventsStream1, 10, 2);
    }
}
