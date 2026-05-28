// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class KeyedAsyncLockTest
{
    [Fact]
    public async Task AcquireAsyncForSameKeyShouldSerializeCallers()
    {
        using var locker = new KeyedAsyncLock<string>();

        var l1 = await locker.AcquireAsync("a");
        var l2Task = locker.AcquireAsync("a");

        l2Task.IsCompleted.Should().BeFalse();

        l1.Dispose();
        using var l2 = await l2Task;
        l2.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsyncForDifferentKeysShouldNotBlock()
    {
        using var locker = new KeyedAsyncLock<string>();

        using var l1 = await locker.AcquireAsync("a");
        using var l2 = await locker.AcquireAsync("b");

        l1.Should().NotBeNull();
        l2.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsyncWithTimeoutShouldThrowWhenLockHeld()
    {
        using var locker = new KeyedAsyncLock<string>();
        using var held = await locker.AcquireAsync("a");

        await locker.Awaiting(l => l.AcquireAsync("a", TimeSpan.Zero))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AcquireAsyncWithTimeoutShouldSucceedWhenLockIsAvailable()
    {
        using var locker = new KeyedAsyncLock<string>();

        using var l1 = await locker.AcquireAsync("a", TimeSpan.Zero);
        l1.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsyncShouldHonorCancellation()
    {
        using var locker = new KeyedAsyncLock<string>();
        using var held = await locker.AcquireAsync("a");

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await locker.Awaiting(l => l.AcquireAsync("a", cts.Token)).Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task TimedOutAcquireShouldNotReleaseHeldLock()
    {
        using var locker = new KeyedAsyncLock<string>();

        var l1 = await locker.AcquireAsync("a");
        await locker.Awaiting(l => l.AcquireAsync("a", TimeSpan.Zero)).Should().ThrowAsync<InvalidOperationException>();

        var l2Task = locker.AcquireAsync("a");
        l2Task.IsCompleted.Should().BeFalse();

        l1.Dispose();
        using var l2 = await l2Task;
        l2.Should().NotBeNull();
    }

    [Fact]
    public async Task CancelledAcquireShouldNotReleaseHeldLock()
    {
        using var locker = new KeyedAsyncLock<string>();

        var l1 = await locker.AcquireAsync("a");
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await locker.Awaiting(l => l.AcquireAsync("a", cts.Token)).Should().ThrowAsync<OperationCanceledException>();

        var l2Task = locker.AcquireAsync("a");
        l2Task.IsCompleted.Should().BeFalse();

        l1.Dispose();
        using var l2 = await l2Task;
        l2.Should().NotBeNull();
    }

    [Fact]
    public async Task ReleasingShouldUnblockNextWaiter()
    {
        using var locker = new KeyedAsyncLock<string>();
        var l1 = await locker.AcquireAsync("a");

        var l2Task = locker.AcquireAsync("a");
        l2Task.IsCompleted.Should().BeFalse();

        l1.Dispose();
        using var l2 = await l2Task;
        l2.Should().NotBeNull();
    }

    [Fact]
    public async Task AcquireAsyncForDifferentKeysShouldCompleteWhileAnotherKeyIsHeld()
    {
        using var locker = new KeyedAsyncLock<string>();

        using var l1 = await locker.AcquireAsync("a");
        var l2Task = locker.AcquireAsync("b");

        l2Task.IsCompletedSuccessfully.Should().BeTrue();
        using var l2 = await l2Task;
        l2.Should().NotBeNull();
    }
}
