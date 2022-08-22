// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class AsyncLockTest
{
    [Fact]
    public async Task TestLockAsync()
    {
        await using var locker = new AsyncLock();
        var l1 = await locker.AcquireAsync();
        var t1 = ReleaseLockAfterWait(l1);
        t1.Status.Should().BeOneOf(TaskStatus.Running, TaskStatus.WaitingForActivation, TaskStatus.WaitingToRun);

        using var l2 = await locker.AcquireAsync();
        await t1.WaitAsync(TimeSpan.FromMilliseconds(100));
    }

    private async Task ReleaseLockAfterWait(IDisposable locker)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        locker.Dispose();
    }
}
