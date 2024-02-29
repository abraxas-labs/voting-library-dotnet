// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Database.Configuration;

namespace Voting.Lib.Database.Interceptors;

/// <summary>
/// Intercepts and monitors database queries which exceed a specified execution time threshold.
/// </summary>
public class DatabaseQueryMonitoringInterceptor : DbCommandInterceptor
{
    private const string MeterName = VotingMeters.NamePrefix + "Database";
    private const string DatabaseQueryExecutionTimeHistogramName = VotingMeters.InstrumentNamePrefix + "database_query_execution_time_histogram";
    private readonly ILogger<DatabaseQueryMonitoringInterceptor> _logger;
    private readonly DataMonitoringConfig _config;
    private readonly Histogram<double> _histogram;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseQueryMonitoringInterceptor"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="config">The data monitoring configuration.</param>
    public DatabaseQueryMonitoringInterceptor(ILogger<DatabaseQueryMonitoringInterceptor> logger, DataMonitoringConfig config)
    {
        _logger = logger;
        _config = config;

        Meter meter = new(MeterName, VotingMeters.Version);
        _histogram = meter.CreateHistogram<double>(
            DatabaseQueryExecutionTimeHistogramName,
            "s",
            "Milliseconds spent processing a specific query");
    }

    /// <inheritdoc />
    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        MonitorQuery(command, eventData);

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        MonitorQuery(command, eventData);

        return base.ReaderExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        MonitorQuery(command, eventData);

        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        MonitorQuery(command, eventData);

        return base.NonQueryExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<object?> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object? result, CancellationToken cancellationToken = default)
    {
        MonitorQuery(command, eventData);

        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
    {
        MonitorQuery(command, eventData);

        return base.ScalarExecuted(command, eventData, result);
    }

    private void MonitorQuery(DbCommand command, CommandExecutedEventData eventData)
    {
        if (!(eventData.Duration.TotalMilliseconds > _config.QueryThreshold.TotalMilliseconds))
        {
            return;
        }

        LogQuery(command, eventData);
        RegisterQueryMetrics(eventData);
    }

    private void LogQuery(DbCommand command, CommandExecutedEventData eventData)
    {
        _logger.LogWarning("Long running database query has been detected. Total milliseconds:{DurationInMs}ms\r\n\r\n{CommandText}", eventData.Duration.TotalMilliseconds, command.CommandText);
    }

    private void RegisterQueryMetrics(CommandExecutedEventData eventData)
    {
        _histogram.Record(eventData.Duration.TotalSeconds);
    }
}
