// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Scheduler.Test.Mocks;

/// <summary>
/// A job that reads its typed cron config from <see cref="IJobRunnerConfigAccessor{MockCronJobConfig}"/>
/// and stores the <see cref="MockCronJobConfig.Tag"/> in <see cref="ConfigCaptureStore"/>.
/// </summary>
public class ConfigAwareCronJob : IScheduledJob
{
    private readonly IJobRunnerConfigAccessor<MockCronJobConfig> _configAccessor;
    private readonly ConfigCaptureStore _store;

    public ConfigAwareCronJob(IJobRunnerConfigAccessor<MockCronJobConfig> configAccessor, ConfigCaptureStore store)
    {
        _configAccessor = configAccessor;
        _store = store;
    }

    public Task Run(CancellationToken ct)
    {
        _store.CapturedTag = _configAccessor.Config?.Tag;
        return Task.CompletedTask;
    }
}
