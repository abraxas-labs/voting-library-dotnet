// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Xunit.Abstractions;

namespace Voting.Lib.Testing;

/// <summary>
/// A logger that redirects output to the <see cref="ITestOutputHelper"/>.
/// </summary>
/// <typeparam name="T">The category type of the logger.</typeparam>
public class XUnitLogger<T> : ILogger<T>
{
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLogger{T}"/> class.
    /// </summary>
    /// <param name="output">The XUnit output helper.</param>
    public XUnitLogger(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <inheritdoc cref="ILogger"/>
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
        => new Disposable(static () => { });

    /// <inheritdoc cref="ILogger"/>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _output.WriteLine($"{logLevel}: {formatter(state, exception)}");
    }
}
