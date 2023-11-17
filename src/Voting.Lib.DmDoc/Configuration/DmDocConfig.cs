// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Voting.Lib.Scheduler;

namespace Voting.Lib.DmDoc.Configuration;

/// <summary>
/// DmDoc configuration.
/// </summary>
public class DmDocConfig
{
    /// <summary>
    /// Gets or sets the token used to access DmDoc.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the DmDoc username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base address of the dm doc api (incl. version and trailing slash).
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the timeout used for dm doc calls.
    /// Use <c>null</c> for no timeout.
    /// </summary>
    public TimeSpan? Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the serialization format for the dm doc data containers.
    /// </summary>
    public DmDocDataSerializationFormat DataSerializationFormat { get; set; } = DmDocDataSerializationFormat.Json;

    /// <summary>
    /// Gets or sets the maximum attempts when polling the draft state.
    /// </summary>
    public int MaxDraftStatePollingAttempts { get; set; } = 10;

    /// <summary>
    /// Gets or sets the delay between draft state polling attempts.
    /// </summary>
    public TimeSpan DraftStatePollingDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the callback retry configuration.
    /// </summary>
    public DmDocCallbackRetryConfig CallbackRetryConfig { get; set; } = new();

    /// <summary>
    /// Gets or sets the timeout in seconds for the callback. If no timeout is specified, the default timeout from DmDoc or no timeout will be used.
    /// </summary>
    public int? CallbackTimeout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the async job in DmDoc will be marked as failed if the callback fails.
    /// Default is true.
    /// </summary>
    public bool FailAsyncJobOnCallbackFailure { get; set; } = true;

    /// <summary>
    /// Gets or sets the scheduler job config for the DmDoc draft cleanup.
    /// </summary>
    public JobConfig DraftCleanupScheduler { get; set; } = new() { Interval = TimeSpan.FromMinutes(5) };
}
