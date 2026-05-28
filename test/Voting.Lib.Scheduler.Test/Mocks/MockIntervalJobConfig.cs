// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Scheduler.Test.Mocks;

/// <summary>
/// A custom interval job config with an extra property, used to verify
/// that typed config is propagated through <see cref="IJobRunnerConfigAccessor{TConfig}"/>.
/// </summary>
public class MockIntervalJobConfig : JobConfig
{
    public string Tag { get; set; } = string.Empty;
}
