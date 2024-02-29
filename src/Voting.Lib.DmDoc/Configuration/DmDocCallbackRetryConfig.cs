// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Configuration;

/// <summary>
/// Configuration for callback retries.
/// </summary>
public class DmDocCallbackRetryConfig
{
    /// <summary>
    /// Gets or sets the retry type.
    /// </summary>
    public DmDocCallbackRetryType RetryType { get; set; } = DmDocCallbackRetryType.ExponentialBackoff;

    /// <summary>
    /// Gets or sets the number of retries which should be made.
    /// </summary>
    public int MaxRetries { get; set; } = 10;

    /// <summary>
    /// Gets or sets the retry interval in milliseconds.
    /// </summary>
    public int RetryInterval { get; set; } = 5000;
}
