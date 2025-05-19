// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Database.Configuration;

/// <summary>
/// Data monitoring configuration.
/// </summary>
public class DataMonitoringConfig
{
    /// <summary>
    /// Gets or sets the threshold in milliseconds for query execution time. Monitoring will cover all queries which exceed this threshold. Default is '1000'.
    /// </summary>
    public TimeSpan QueryThreshold { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets a maximum length for the command text. Default is '10000'.
    /// </summary>
    public int MaxCommandTextLength { get; set; } = 10000;
}
