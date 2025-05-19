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

public class CronSchedulerServiceTest
{
    [Fact]
    public void EmptyScheduleShouldThrow()
    {
        var sc = new ServiceCollection();
        Assert.Throws<ValidationException>(() =>
            sc.AddCronJob<MockJob>(new CronJobConfig { CronSchedule = string.Empty }));
    }

    [Fact]
    public void StandardCronFormatShouldWork()
    {
        BuildScheduler("* * * * *"); // every minute
    }

    [Fact]
    public void CronFormatWithSecondsShouldWork()
    {
        BuildScheduler("* * * * * *"); // every second
    }

    [Fact]
    public async Task ShouldExecute()
    {
        var cronSchedule = "* * * * * *"; // every second
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);
        store.StartedAt.Should().HaveCount(0);
        store.CancelledAt.Should().HaveCount(0);

        await AdvanceTime(timeProvider, interval, store.JobExecutionTime);
        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(0);

        await AdvanceTime(timeProvider, interval, store.JobExecutionTime);
        store.StartedAt.Should().HaveCount(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldDropParallelExecutions()
    {
        var cronSchedule = "* * * * * *"; // every second
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);

        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(0);

        // the interval should trigger the 2. run of the job,
        // but since the 1. run does not have completed yet, the current run is skipped.
        await AdvanceTime(timeProvider, interval);
        store.StartedAt.Should().HaveCount(1);

        // the 2. run should start now, as the 1. run is finished.
        await AdvanceTime(timeProvider, interval, store.JobExecutionTime);
        store.StartedAt.Should().HaveCount(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCancelIfStopped()
    {
        var cronSchedule = "* * * * * *"; // every second
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, timeProvider, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);

        // 1. run should start after <interval>
        await AdvanceTime(timeProvider, interval);

        // stop scheduler while 1. run is still running
        // this should cancel the running job
        await scheduler.StopAsync(CancellationToken.None);
        store.StartedAt.Should().HaveCount(1);
        store.CancelledAt.Should().HaveCount(1);
    }

    private (IHostedService SchedulerService, JobStore Store, FakeTimeProvider TimeProvider, IDisposable Disposable) BuildScheduler(string schedule)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<JobStore>();
        sc.AddCronJob<MockJob>(schedule);
        sc.AddMockedClock();
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
            await Task.Delay(100);
        }
    }
}
