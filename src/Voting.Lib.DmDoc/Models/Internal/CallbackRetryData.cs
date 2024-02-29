// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models.Internal;

/// <summary>
/// The callback retry policy for asynchronous jobs.
/// </summary>
internal class CallbackRetryData
{
    /// <summary>
    /// Gets or sets the retry_type - 0 for no retry, 1 for exponential back off, 2 for fixed retry interval (Default: 0).
    /// </summary>
    public int RetryType { get; set; }

    /// <summary>
    /// Gets or sets the max_retries - the maximum number of retries which should be made (Default: 10).
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    /// Gets or sets the retry_interval - the interval between two retry actions in Milliseconds (Default: 5000).
    /// </summary>
    public int RetryInterval { get; set; }
}
