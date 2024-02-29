// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Testing.Mocks;

/// <summary>
/// A mock logger provider.
/// </summary>
public class MockLoggerProvider : ILoggerProvider
{
    private readonly object _loggerInstancesLock = new();

    /// <summary>
    /// Gets the logger instances by their category names.
    /// </summary>
    public Dictionary<string, List<MockLogger>> LoggerInstances { get; } = new();

    /// <summary>
    /// Tries to get a single logger instance with a given category name.
    /// </summary>
    /// <param name="categoryName">The category name to look for.</param>
    /// <returns>The logger instance or <c>null</c> if none is found.</returns>
    public MockLogger? GetSingleLogger(string categoryName)
        => LoggerInstances.GetValueOrDefault(categoryName)?.SingleOrDefault();

    /// <inheritdoc cref="ILoggerProvider.CreateLogger"/>
    public ILogger CreateLogger(string categoryName)
    {
        lock (_loggerInstancesLock)
        {
            var logger = new MockLogger(categoryName, l =>
            {
                lock (_loggerInstancesLock)
                {
                    LoggerInstances[l.Category].Remove(l);
                }
            });

            if (LoggerInstances.TryGetValue(logger.Category, out var categoryLoggers))
            {
                categoryLoggers.Add(logger);
            }
            else
            {
                LoggerInstances[logger.Category] = new() { logger };
            }

            return logger;
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
