// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Voting.Lib.Scheduler.Test.Mocks;
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
        var (scheduler, store, disposable) = BuildScheduler(interval);
        using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldExecuteOnStart()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, disposable) = BuildScheduler(interval, true);
        using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldDropParallelExecutions()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, disposable) = BuildScheduler(interval);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        await Task.Delay(store.JobExecutionTime);
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCancelIfStopped()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store, disposable) = BuildScheduler(interval);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        await scheduler.StopAsync(CancellationToken.None);
        await Task.Delay(store.JobExecutionTime + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(1);
    }

    private (IHostedService SchedulerService, JobStore Store, IDisposable Disposable) BuildScheduler(TimeSpan interval, bool runOnStart = false)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<JobStore>();
        sc.AddScheduledJob<MockJob>(interval, runOnStart);
        var services = sc.BuildServiceProvider();
        var schedulerService = services.GetRequiredService<IHostedService>();
        var store = services.GetRequiredService<JobStore>();
        return (schedulerService, store, services);
    }
}
