// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Scheduler;

/// <summary>
/// Default implementation of <see cref="IJobRunnerConfigAccessor{TConfig}"/>.
/// Registered as a scoped service so each job execution scope gets its own instance.
/// </summary>
/// <typeparam name="TConfig">The type of the job configuration.</typeparam>
public class JobRunnerConfigAccessor<TConfig> : IJobRunnerConfigAccessor<TConfig>
    where TConfig : class
{
    private TConfig? _config;

    /// <inheritdoc />
    public TConfig Config => _config
        ?? throw new InvalidOperationException($"Job config of type {typeof(TConfig).Name} has not been initialized.");

    internal void Initialize(TConfig config) => _config = config;
}
