// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Text;
using EventStore.Client;
using Voting.Lib.Eventing.Test.Events;

namespace Voting.Lib.Eventing.Test;

internal static class EventRecordTestUtil
{
    internal static EventRecord BuildRecord(bool addMetadata = false, bool unknownEventType = false)
    {
        return new(
            "stream-id",
            Uuid.FromGuid(Guid.Parse("e2dd66f4-a932-44e5-bae4-28eb0bef8ef5")),
            new(10UL),
            new(556UL, 556UL),
            new Dictionary<string, string>
            {
                ["type"] = unknownEventType ? "unknown_event_type" : TestEvent.Descriptor.FullName,
                ["created"] = "1652789613",
                ["content-type"] = "application/json",
            },
            Encoding.UTF8.GetBytes("{\"test_value\": 10}").AsMemory(),
            addMetadata ? Encoding.UTF8.GetBytes("{\"test_meta_value\": 20}").AsMemory() : ReadOnlyMemory<byte>.Empty);
    }
}
