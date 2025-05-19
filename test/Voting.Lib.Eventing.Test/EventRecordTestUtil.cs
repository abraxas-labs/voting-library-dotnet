// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using EventStore.Client;
using Voting.Lib.Eventing.Test.Events;

namespace Voting.Lib.Eventing.Test;

internal static class EventRecordTestUtil
{
    internal static EventRecord BuildRecord(string type, ReadOnlyMemory<byte> data, ReadOnlyMemory<byte>? metadata = null)
    {
        return new(
            "stream-id",
            Uuid.FromGuid(Guid.Parse("e2dd66f4-a932-44e5-bae4-28eb0bef8ef5")),
            new(10UL),
            new(556UL, 556UL),
            new Dictionary<string, string>
            {
                ["type"] = type,
                ["created"] = "1652789613",
                ["content-type"] = "application/json",
            },
            data,
            metadata ?? ReadOnlyMemory<byte>.Empty);
    }

    internal static EventRecord BuildRecord(bool addMetadata = false, bool unknownEventType = false)
    {
        return BuildRecord(
            unknownEventType ? "unknown_event_type" : TestEvent.Descriptor.FullName,
            "{\"test_value\": 10}"u8.ToArray().AsMemory(),
            addMetadata ? "{\"test_meta_value\": 20}"u8.ToArray().AsMemory() : null);
    }
}
