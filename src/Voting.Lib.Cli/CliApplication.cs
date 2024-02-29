// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Common;

namespace Voting.Lib.Cli;

/// <summary>
/// Application helper class for cli's.
/// </summary>
public static class CliApplication
{
    /// <summary>
    /// Runs the cli application with a default startup implementation.
    /// </summary>
    /// <param name="args">The cli arguments.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the exit code.</returns>
    public static Task<int> Run(string[] args)
        => Run<CliStartup>(args);

    /// <summary>
    /// Runs the cli application using the provided startup implementation.
    /// </summary>
    /// <param name="args">The cli arguments.</param>
    /// <typeparam name="TStartup">Type of the startup to use.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the exit code.</returns>
    public static async Task<int> Run<TStartup>(string[] args)
        where TStartup : CliStartup, new()
    {
        EnvironmentVariablesFixer.FixDotEnvironmentVariables();
        var startup = new TStartup();
        var config = startup.BuildConfig<TStartup>(args);

        var services = new ServiceCollection();
        startup.ConfigureServices(services, config);

        await using var serviceProvider = services.BuildServiceProvider();
        var optionTypes = serviceProvider
            .GetServices<CommandRegistration>()
            .Select(x => x.OptionsType)
            .ToArray();

        if (optionTypes.Length == 0)
        {
            throw new InvalidOperationException("no ICommand<TOptions> registrations found");
        }

        return await new Parser(opts => startup.ConfigureParser(opts, config))
            .ParseArguments(args, optionTypes)
            .MapResult(
                opts => RunAction(serviceProvider, opts),
                errs => errs.FirstOrDefault()?.Tag == ErrorType.VersionRequestedError
                    ? Task.FromResult(ExitCodes.Ok)
                    : Task.FromResult(ExitCodes.ParserError))
            .ConfigureAwait(false);
    }

    private static async Task<int> RunAction(IServiceProvider sp, object commandOptions)
    {
        using var cts = new CancellationTokenSource();

        void CancelKeyPressHandler(object? o, ConsoleCancelEventArgs consoleCancelEventArgs) => cts.Cancel();

        Console.CancelKeyPress += CancelKeyPressHandler;

        var commandHandlerType = typeof(ICommand<>).MakeGenericType(commandOptions.GetType());
        var commandHandler = (ICommand)sp.GetRequiredService(commandHandlerType);
        var result = await commandHandler.Run(commandOptions, cts.Token).ConfigureAwait(false);

        Console.CancelKeyPress -= CancelKeyPressHandler;

        return result;
    }
}
