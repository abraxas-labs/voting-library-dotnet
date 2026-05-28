// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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

/// <summary>
/// Tests that a typed config derived from <see cref="ICronJobConfig"/> is correctly
/// propagated to the job via <see cref="IJobRunnerConfigAccessor{TConfig}"/>.
/// </summary>
public class CronJobConfigAccessorTest
{
    [Fact]
    public async Task TypedConfigShouldBeInjectedIntoJob()
    {
        const string expectedTag = "my-cron-tag";
        var config = new MockCronJobConfig
        {
            CronSchedule = "* * * * * *", // every second
            Tag = expectedTag,
        };

        var (scheduler, store, timeProvider, services) = BuildScheduler(config);
        await using var sp = services;
        await scheduler.StartAsync(CancellationToken.None);

        // Advance past the first tick so the job runs once
        await AdvanceTime(timeProvider, TimeSpan.FromSeconds(1));

        store.CapturedTag.Should().Be(expectedTag);
        await scheduler.StopAsync(CancellationToken.None);
    }

    private static (IHostedService Scheduler, ConfigCaptureStore Store, FakeTimeProvider TimeProvider, ServiceProvider Services) BuildScheduler(MockCronJobConfig config)
    {
        var sc = new ServiceCollection();
        sc.AddLogging();
        sc.AddSingleton<ConfigCaptureStore>();
        sc.AddCronJob<ConfigAwareCronJob, MockCronJobConfig>(config);
        sc.AddMockedClock();
        var services = sc.BuildServiceProvider();
        return (
            services.GetRequiredService<IHostedService>(),
            services.GetRequiredService<ConfigCaptureStore>(),
            services.GetRequiredService<FakeTimeProvider>(),
            services);
    }

    private static async Task AdvanceTime(FakeTimeProvider timeProvider, TimeSpan delta)
    {
        timeProvider.Advance(delta);
        await Task.Delay(100);
    }
}
