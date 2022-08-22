// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.Cli;

/// <summary>
/// A command represents an action executable via cli.
/// </summary>
/// <typeparam name="TOptions">The options of the command.</typeparam>
public interface ICommand<in TOptions> : ICommand
    where TOptions : new()
{
    /// <summary>
    /// Runs the command.
    /// </summary>
    /// <param name="options">The options of the command.</param>
    /// <param name="ct">The cancellation token, fired when the user cancels the invocation.</param>
    /// <returns>The exit code.</returns>
    Task<int> Run(TOptions options, CancellationToken ct);

    /// <summary>
    /// Runs the command.
    /// Use the typed overload instead.
    /// </summary>
    /// <param name="options">The options of the command.</param>
    /// <param name="ct">The cancellation token, fired when the user cancels the invocation.</param>
    /// <returns>The exit code.</returns>
    Task<int> ICommand.Run(object options, CancellationToken ct) => Run((TOptions)options, ct);
}

/// <summary>
/// A command represents an action executable via cli.
/// The generic overload (<see cref="ICommand{TOptions}"/> has to be used by the implementing application.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Runs the command.
    /// Use the typed overload of <see cref="ICommand{TOptions}"/> instead.
    /// </summary>
    /// <param name="options">The options of the command.</param>
    /// <param name="ct">The cancellation token, fired when the user cancels the invocation.</param>
    /// <returns>The exit code.</returns>
    Task<int> Run(object options, CancellationToken ct);
}
