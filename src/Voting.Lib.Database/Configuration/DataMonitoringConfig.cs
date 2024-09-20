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
}
