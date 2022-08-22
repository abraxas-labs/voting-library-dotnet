// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Voting.Lib.Scheduler.Test;

public class SchedulerServiceTest
{
    [Fact]
    public void NegativeIntervalShouldThrow()
    {
        var sc = new ServiceCollection();
        Assert.Throws<ValidationException>(() =>
            sc.AddScheduledJob<MyDemoJob>(new JobConfig { Interval = TimeSpan.FromHours(-1) }));
    }

    [Fact]
    public void ZeroIntervalShouldThrow()
    {
        var sc = new ServiceCollection();
        Assert.Throws<ValidationException>(() =>
            sc.AddScheduledJob<MyDemoJob>(new JobConfig { Interval = TimeSpan.Zero }));
    }

    [Fact]
    public async Task ShouldExecute()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store) = BuildScheduler(interval);
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
        var (scheduler, store) = BuildScheduler(interval, true);
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
        var (scheduler, store) = BuildScheduler(interval);
        store.JobDelay = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        await Task.Delay(store.JobDelay);
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCancelIfStopped()
    {
        var interval = TimeSpan.FromSeconds(1);
        var (scheduler, store) = BuildScheduler(interval);
        store.JobDelay = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(interval + TimeSpan.FromSeconds(.1));
        await scheduler.StopAsync(CancellationToken.None);
        await Task.Delay(store.JobDelay + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(1);
    }

    private (IHostedService SchedulerService, MyDemoJobStore Store) BuildScheduler(TimeSpan interval, bool runOnStart = false)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<MyDemoJobStore>();
        sc.AddScheduledJob<MyDemoJob>(interval, runOnStart);
        var services = sc.BuildServiceProvider();
        var schedulerService = services.GetRequiredService<IHostedService>();
        var store = services.GetRequiredService<MyDemoJobStore>();
        return (schedulerService, store);
    }

    private class MyDemoJob : IScheduledJob
    {
        private readonly MyDemoJobStore _store;

        public MyDemoJob(MyDemoJobStore store)
        {
            _store = store;
        }

        public async Task Run(CancellationToken ct)
        {
            _store.CountOfExecutions++;

            try
            {
                await Task.Delay(_store.JobDelay, ct);
            }
            catch (TaskCanceledException)
            {
                _store.CountOfCancellations++;
            }
        }
    }

    private class MyDemoJobStore
    {
        public int CountOfExecutions { get; set; }

        public int CountOfCancellations { get; set; }

        public TimeSpan JobDelay { get; set; } = TimeSpan.FromSeconds(.5);
    }
}
