// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Scheduler;

/// <summary>
/// Provides access to the job configuration within a DI scope.
/// The <see cref="JobRunner"/> initializes the configuration before the job is resolved and executed,
/// allowing jobs to inject this accessor and read their per-instance configuration.
/// </summary>
/// <typeparam name="TConfig">The type of the job configuration.</typeparam>
public interface IJobRunnerConfigAccessor<out TConfig>
    where TConfig : class
{
    /// <summary>
    /// Gets the configuration for the current job run.
    /// </summary>
    TConfig Config { get; }
}
