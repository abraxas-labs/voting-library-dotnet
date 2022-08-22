// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Threading.Tasks;
using EventStore.Client;
using FluentAssertions;
using Voting.Lib.Eventing.Subscribe;
using Xunit;

namespace Voting.Lib.Eventing.Test.Subscribe;

public class TransientEventProcessorScopeTest
{
    [Fact]
    public async Task ShouldStartAtNull()
    {
        var scope = new TransientEventProcessorScope();
        var position = await scope.GetSnapshotPosition();
        position.Should().BeNull();

        // should store position in memory after
        await scope.Begin(new Position(10UL, 10UL), new StreamPosition(1UL));
        await scope.Complete(new Position(10UL, 10UL), new StreamPosition(1UL));
    }

    [Fact]
    public async Task ShouldStorePositionInMemory()
    {
        var scope = new TransientEventProcessorScope();
        await scope.Begin(new Position(10UL, 10UL), new StreamPosition(1UL));
        await scope.Complete(new Position(10UL, 10UL), new StreamPosition(1UL));

        var snapshot = await scope.GetSnapshotPosition();
        snapshot.Should().NotBeNull();

        var (position, streamPosition) = snapshot!.Value;
        position.CommitPosition.Should().Be(10UL);
        position.PreparePosition.Should().Be(10UL);
        streamPosition.Should().Be(1UL);
    }

    [Fact]
    public async Task ShouldDropPositionIfNotCompleted()
    {
        var scope = new TransientEventProcessorScope();
        await scope.Begin(new Position(10UL, 10UL), new StreamPosition(1UL));

        var snapshot = await scope.GetSnapshotPosition();
        snapshot.Should().BeNull();
    }

    [Fact]
    public async Task ShouldKeepPreviousPositionIfNotCompleted()
    {
        var scope = new TransientEventProcessorScope();
        await scope.Begin(new Position(10UL, 10UL), new StreamPosition(1UL));
        await scope.Complete(new Position(10UL, 10UL), new StreamPosition(1UL));
        await scope.Begin(new Position(20UL, 20UL), new StreamPosition(2UL));

        var snapshot = await scope.GetSnapshotPosition();
        snapshot.Should().NotBeNull();

        var (position, streamPosition) = snapshot!.Value;
        position.CommitPosition.Should().Be(10UL);
        position.PreparePosition.Should().Be(10UL);
        streamPosition.Should().Be(1UL);
    }
}
