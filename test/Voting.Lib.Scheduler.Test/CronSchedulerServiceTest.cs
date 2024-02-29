// (c) Copyright 2024 by Abraxas Informatik AG
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
        var (scheduler, store, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(TimeSpan.FromSeconds(1));
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldDropParallelExecutions()
    {
        var cronSchedule = "* * * * * *"; // every second
        var (scheduler, store, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(0);
        await Task.Delay(TimeSpan.FromSeconds(1));
        store.CountOfExecutions.Should().Be(1);
        await Task.Delay(store.JobExecutionTime);
        store.CountOfExecutions.Should().Be(2);
        await scheduler.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCancelIfStopped()
    {
        var cronSchedule = "* * * * * *"; // every second
        var (scheduler, store, disposable) = BuildScheduler(cronSchedule);
        using var toDispose = disposable;
        store.JobExecutionTime = TimeSpan.FromSeconds(2);
        await scheduler.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(1));
        await scheduler.StopAsync(CancellationToken.None);
        await Task.Delay(store.JobExecutionTime + TimeSpan.FromSeconds(.1));
        store.CountOfExecutions.Should().Be(1);
        store.CountOfCancellations.Should().Be(1);
    }

    private (IHostedService SchedulerService, JobStore Store, IDisposable Disposable) BuildScheduler(string schedule)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<JobStore>();
        sc.AddCronJob<MockJob>(schedule);
        var services = sc.BuildServiceProvider();
        var schedulerService = services.GetRequiredService<IHostedService>();
        var store = services.GetRequiredService<JobStore>();
        return (schedulerService, store, services);
    }
}
