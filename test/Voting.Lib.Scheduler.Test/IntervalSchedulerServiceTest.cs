// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;
using Voting.Lib.Scheduler.Test.Mocks;
using Voting.Lib.Testing.Mocks;
using Xunit;

namespace Voting.Lib.Scheduler.Test;

public class IntervalSchedulerServiceTest
{
    [Fact]
    public void NegativeIntervalShouldThrow()
    {
        var sc = new ServiceCollection();
        Assert.Throws<ValidationException>(() =>
            sc.AddScheduledJob<MockJob>(new JobConfig { Interval = TimeSpan.FromHours(-1) }));
    }

    [Fact]
    public void ZeroIntervalShouldThrow()
    {
        var sc = new ServiceCollection();
        Assert.Throws<ValidationException>(() =>
            sc.AddScheduledJob<MockJob>(new JobConfig { Interval = TimeSpan.Zero }));
    }

    [Fact]
    public async Task ShouldExecute()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(interval);
        await using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);

        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(0);

        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldExecuteOnStart()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(interval, true);
        await using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);

        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(0);

        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldDropParallelExecutions()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(interval);
        await using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);

        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(0);

        // 1. run is still running, 2. run is not yet started (interval is skipped).
        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(1);

        // 1. run should be completed by now, 2. run should be started.
        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCancelIfStopped()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(interval);
        await using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);

        await scheduler.StartAsync(CancellationToken.None);
        await AdvanceTime(timeProvider, interval);
        await scheduler.StopAsync(CancellationToken.None);

        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(1);
    }

    private (IHostedService SchedulerService, JobStore Store, FakeTimeProvider TimeProvider, ServiceProvider Services) BuildScheduler(TimeSpan interval, bool runOnStart = false)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<JobStore>();
        sc.AddScheduledJob<MockJob>(interval, runOnStart);
        sc.AddMockedTimeProvider();
        var services = sc.BuildServiceProvider();
        var schedulerService = services.GetRequiredService<IHostedService>();
        var store = services.GetRequiredService<JobStore>();
        var timeProvider = services.GetRequiredService<FakeTimeProvider>();
        return (schedulerService, store, timeProvider, services);
    }

    private async Task AdvanceTime(FakeTimeProvider timeProvider, params TimeSpan[] deltas)
    {
        // multiple separate advances may be needed to notify multiple delays in the scheduler.
        foreach (var delta in deltas)
        {
            timeProvider.Advance(delta);

            // ensures the code of the thread being awaited/asserted continues
            // value was evaluated by trial & error
            await Task.Delay(20);
        }
    }
}
