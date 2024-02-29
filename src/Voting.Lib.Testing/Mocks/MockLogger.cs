// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;

namespace Voting.Lib.Testing.Mocks;

/// <summary>
/// A mocked logger implementation.
/// </summary>
/// <typeparam name="T">The category type name.</typeparam>
public class MockLogger<T> : MockLogger, ILogger<T>
{
}

/// <summary>
/// A mocked logger implementation.
/// </summary>
public class MockLogger : ILogger, IDisposable
{
    private readonly Action<MockLogger> _onDispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockLogger"/> class.
    /// </summary>
    /// <param name="category">The name of the category of the logger.</param>
    public MockLogger(string category = "")
        : this(category, _ => { })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockLogger"/> class.
    /// </summary>
    /// <param name="category">The name of the category of the logger.</param>
    /// <param name="onDispose">An action which is invoked when this instance is disposed.</param>
    public MockLogger(string category, Action<MockLogger> onDispose)
    {
        _onDispose = onDispose;
        Category = category;
    }

    /// <summary>
    /// Gets a list of active scopes.
    /// </summary>
    public List<object?> ActiveScopes { get; } = new();

    /// <summary>
    /// Gets a list of log messages.
    /// </summary>
    public List<LogMessage> Messages { get; } = new();

    /// <summary>
    /// Gets the category name of this logger.
    /// </summary>
    public string Category { get; }

    /// <inheritdoc cref="ILogger.BeginScope{TState}"/>
    public IDisposable BeginScope<TState>(TState state)
    {
        ActiveScopes.Add(state);
        return new Disposable(() => ActiveScopes.Remove(state));
    }

    /// <inheritdoc cref="ILogger.IsEnabled"/>
    public bool IsEnabled(LogLevel logLevel)
        => true;

    /// <inheritdoc cref="ILogger.Log{TState}"/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Messages.Add(new(logLevel, eventId, state, exception, formatter(state, exception)));
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _onDispose(this);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Represents a recorded log message.
    /// </summary>
    /// <param name="LogLevel">The level of the log message.</param>
    /// <param name="EventId">The id of the event.</param>
    /// <param name="State">The state object of the log message.</param>
    /// <param name="Exception">The exception of the log message.</param>
    /// <param name="Formatted">The formatted string message of the log message.</param>
    public record LogMessage(LogLevel LogLevel, EventId EventId, object? State, Exception? Exception, string Formatted);
}
