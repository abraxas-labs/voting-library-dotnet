// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Voting.Lib.Database.Testing.NQueryDetector;

/// <summary>
/// Counts the number of query executions for active spans.
/// Currently only reader executions are handled.
/// </summary>
public class NQueryDetectorInterceptor : DbCommandInterceptor
{
    private readonly ConcurrentDictionary<Guid, NQueryDetectorSpan> _activeSpans = new();

    /// <inheritdoc />
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        LogCommand(command.CommandText);
        return base.ReaderExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogCommand(command.CommandText);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
    {
        LogCommand(command.CommandText);
        return base.ScalarExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        LogCommand(command.CommandText);
        return base.NonQueryExecuted(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<object?> ScalarExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        object? result,
        CancellationToken cancellationToken = default)
    {
        LogCommand(command.CommandText);
        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogCommand(command.CommandText);
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    internal NQueryDetectorSpan CreateSpan(int maxCount = 1)
    {
        var span = new NQueryDetectorSpan(maxCount, x => _activeSpans.Remove(x.Id, out _));
        _activeSpans.TryAdd(span.Id, span);
        return span;
    }

    /// <summary>
    /// Logs a command text.
    /// </summary>
    /// <param name="text">The text to log.</param>
    protected void LogCommand(string text)
    {
        if (ShouldIgnoreQuery(text))
        {
            return;
        }

        foreach (var span in _activeSpans.Values)
        {
            span.Increment(text);
        }
    }

    /// <summary>
    /// Determines if a query should be ignored.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <returns>Returns whether the query should be ignored.</returns>
    protected virtual bool ShouldIgnoreQuery(string text)
        => text.StartsWith(NQueryDetectorExtensions.IgnoreTag, StringComparison.InvariantCulture);
}
