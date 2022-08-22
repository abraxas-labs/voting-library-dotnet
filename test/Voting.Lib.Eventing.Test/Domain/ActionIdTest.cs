// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using EventStore.Client;
using FluentAssertions;
using Voting.Lib.Eventing.Domain;
using Xunit;

namespace Voting.Lib.Eventing.Test.Domain;

public class ActionIdTest
{
    [Theory]
    [InlineData("action1", "stream1", null, "aca7ee21c7a2dbf36b2781116662a70d05c3830d09f0e080cf73b4420a4b5806")]
    [InlineData("action2", "stream2", 0UL, "116e9dd55907ff70a0d25b48bf8db126d26982d83f99fa3373f8cecd13e06759")]
    [InlineData("action3", "stream3", 1UL, "252d5f82eea7bb1649330df95cc70f5020f741af44bccca8e595edbf4cf456f8")]
    [InlineData("action4", "stream4", (ulong)long.MaxValue, "98d304ea0690c92b1550f8977e37a0a75d8b78542de7db254d04be5ec58d3464")]
    [InlineData("action5", "stream5", ulong.MaxValue, "15f4368ffc5908249fb7e904449ea289d0553526fedbbceb80831914ce3452cd")]
    public void ShouldComputeHash(string action, string streamName, ulong? streamRevision, string hash)
    {
        new ActionId(action, streamName, streamRevision == null ? null : new StreamRevision(streamRevision.Value))
            .ComputeHash()
            .Should()
            .Be(hash);
    }
}
