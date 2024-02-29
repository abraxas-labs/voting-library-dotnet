// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Prometheus;

/// <summary>
/// Configuration for the prometheus meter adapter.
/// </summary>
public class PrometheusMeterAdapterConfig
{
    /// <summary>
    /// Gets or sets the interval, in which the dotnet observable meters are collected.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);
}
