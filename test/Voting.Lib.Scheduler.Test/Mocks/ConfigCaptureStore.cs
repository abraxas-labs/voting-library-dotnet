// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Scheduler.Test.Mocks;

/// <summary>
/// Captures the typed config that was injected via <see cref="IJobRunnerConfigAccessor{TConfig}"/>
/// during a job run, so tests can assert on it.
/// </summary>
public class ConfigCaptureStore
{
    public string? CapturedTag { get; set; }
}
