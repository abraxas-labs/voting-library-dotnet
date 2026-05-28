// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Scheduler.Test.Mocks;

/// <summary>
/// A job that reads its typed interval config from <see cref="IJobRunnerConfigAccessor{MockIntervalJobConfig}"/>
/// and stores the <see cref="MockIntervalJobConfig.Tag"/> in <see cref="ConfigCaptureStore"/>.
/// </summary>
public class ConfigAwareIntervalJob : IScheduledJob
{
    private readonly IJobRunnerConfigAccessor<MockIntervalJobConfig> _configAccessor;
    private readonly ConfigCaptureStore _store;

    public ConfigAwareIntervalJob(IJobRunnerConfigAccessor<MockIntervalJobConfig> configAccessor, ConfigCaptureStore store)
    {
        _configAccessor = configAccessor;
        _store = store;
    }

    public Task Run(CancellationToken ct)
    {
        _store.CapturedTag = _configAccessor.Config.Tag;
        return Task.CompletedTask;
    }
}
