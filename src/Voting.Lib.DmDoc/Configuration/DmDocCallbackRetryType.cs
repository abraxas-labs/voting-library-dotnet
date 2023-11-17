// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Configuration;

/// <summary>
/// DmDoc callback retry types.
/// </summary>
public enum DmDocCallbackRetryType
{
    /// <summary>
    /// No retry.
    /// </summary>
    None,

    /// <summary>
    /// Retry with exponential backoff.
    /// </summary>
    ExponentialBackoff,

    /// <summary>
    /// Retry with a fixed interval.
    /// </summary>
    FixedInterval,
}
