// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.IntegrationTest;

public static class MockEvents
{
    public const string TestStreamName = "Test";

    public static readonly Guid AggregateId1 = Guid.Parse("95b2844e-be0b-4d43-b473-d4af124f010d");
    public static readonly Guid AggregateId2 = Guid.Parse("6dfdd121-184a-4168-8591-b925f9a2d545");

    public static readonly string TestStream1 = BuildStreamName(AggregateId1);
    public static readonly string TestStream2 = BuildStreamName(AggregateId2);
    public static readonly string UnknownEventsStream1 = "Unknown-4e8195c6-5a9d-4276-9962-8db919c9c91a";

    public static Task PublishUnknownEvents(IServiceProvider sp, string stream, int eventCount)
    {
        var client = sp.GetRequiredService<EventStoreClient>();
        var events = Enumerable
            .Range(0, eventCount)
            .Select(i => new EventData(
                Uuid.NewUuid(),
                "unknown_event",
                Encoding.UTF8.GetBytes($"{{ \"value\": {i}}}"),
                Encoding.UTF8.GetBytes($"{{ \"meta_value\": {i}}}")));
        return client.AppendToStreamAsync(stream, StreamState.Any, events);
    }

    /// <summary>
    /// Publishes some test events to a stream.
    /// </summary>
    /// <param name="sp">The service provider to resolve event store services.</param>
    /// <param name="stream">The stream where to publish the mock events to.</param>
    /// <param name="eventCount">The count of events to be published.</param>
    /// <param name="modEvent2">
    /// <c>null</c> or a positive integer indicating which events should be of type <see cref="TestEvent2"/> instead of <see cref="TestEvent"/>
    /// If <c>null</c> all events are of type <see cref="TestEvent"/>. If an integer is provided every n-th event is of type <see cref="TestEvent2"/>.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task PublishEvents(IServiceProvider sp, string stream, int eventCount, int? modEvent2 = null)
    {
        var client = sp.GetRequiredService<EventStoreClient>();
        var serializer = sp.GetRequiredService<IEventSerializer>();
        return client.AppendToStreamAsync(stream, StreamState.Any, BuildEvents(serializer, eventCount, modEvent2));
    }

    private static IEnumerable<EventData> BuildEvents(IEventSerializer serializer, int eventCount, int? modEvent2)
    {
        for (var i = 0; i < eventCount; i++)
        {
            IMessage ev = !modEvent2.HasValue || i % modEvent2.Value != 0
                ? new TestEvent { TestValue = i }
                : new TestEvent2 { TestValue2 = i };

            yield return new EventData(
                Uuid.NewUuid(),
                ev.Descriptor.FullName,
                serializer.Serialize(ev),
                serializer.Serialize(new TestEventMetadata { TestMetaValue = i + 10 }));
        }
    }

    private static string BuildStreamName(Guid id)
        => $"{TestStreamName}-{id}";
}
