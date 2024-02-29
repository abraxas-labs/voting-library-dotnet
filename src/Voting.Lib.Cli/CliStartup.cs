// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Cli;

/// <summary>
/// Base cli application startup implementation.
/// </summary>
public class CliStartup
{
    private const string DefaultAppSettingsName = "appsettings.json";
    private const string EnvironmentAppSettingsFormat = "appsettings.{0}.json";
    private const string NetCoreEnvironmentVariableName = "NETCORE_ENVIRONMENT";

    /// <summary>
    /// Registers all services of the app.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The resolved configuration.</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        AddLogging(services, configuration);
        AddCommands(services, configuration);
    }

    /// <summary>
    /// Builds the configuration.
    /// By default json files, environment variables and command line args are added.
    /// </summary>
    /// <typeparam name="TStartup">The type from the assembly to search for a user secret.</typeparam>
    /// <param name="args">The cli arguments.</param>
    /// <returns>The built configuration.</returns>
    public virtual IConfiguration BuildConfig<TStartup>(string[] args)
        where TStartup : CliStartup
    {
        var configurationBuilder = new ConfigurationBuilder();
        var environment = Environment.GetEnvironmentVariable(NetCoreEnvironmentVariableName);

        configurationBuilder.AddJsonFile(
            Path.Combine(AppContext.BaseDirectory, DefaultAppSettingsName),
            optional: true,
            reloadOnChange: false);
        configurationBuilder.AddJsonFile(
            DefaultAppSettingsName,
            optional: true,
            reloadOnChange: false);
        configurationBuilder.AddJsonFile(
            string.Format(EnvironmentAppSettingsFormat, environment),
            optional: true,
            reloadOnChange: false);

        configurationBuilder.AddUserSecrets<TStartup>();
        configurationBuilder.AddEnvironmentVariables();
        configurationBuilder.AddCommandLine(args);
        return configurationBuilder.Build();
    }

    /// <summary>
    /// Configures the parser.
    /// </summary>
    /// <param name="settings">The parser settings to customize.</param>
    /// <param name="configuration">The configuration.</param>
    public virtual void ConfigureParser(ParserSettings settings, IConfiguration configuration)
    {
        settings.HelpWriter = Console.Error;
    }

    /// <summary>
    /// Registers the cli commands in the service collection.
    /// By default this scanns the assembly of the current type or the entry assembly
    /// (if the current type is not a subclass of <see cref="CliStartup"/>
    /// for <see cref="ICommand{TOptions}"/> implementations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    protected virtual void AddCommands(IServiceCollection services, IConfiguration configuration)
    {
        if (GetType() != typeof(CliStartup))
        {
            AddCommandsFromAssembly(services, GetType().Assembly);
            return;
        }

        if (Assembly.GetEntryAssembly() is { } entryAssembly)
        {
            AddCommandsFromAssembly(services, entryAssembly);
        }
    }

    /// <summary>
    /// Scans an assembly for <see cref="ICommand{TOptions}"/> implementations and adds them to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly which defines the types to be scanned.</param>
    protected virtual void AddCommandsFromAssembly(IServiceCollection services, Assembly assembly)
    {
        foreach (var definedType in assembly.DefinedTypes)
        {
            if (!TryGetCommandRegistration(definedType, out var registration))
            {
                continue;
            }

            services.AddSingleton(registration);
            services.AddTransient(registration.InterfaceType, registration.CommandType);
        }
    }

    /// <summary>
    /// Adds logging to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration.</param>
    protected virtual void AddLogging(IServiceCollection services, IConfiguration config)
    {
        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.SetMinimumLevel(LogLevel.Information);
            logBuilder.AddConfiguration(config.GetSection("Logging"));
            logBuilder.AddConsole();
        });
    }

    private bool TryGetCommandRegistration(Type t, [NotNullWhen(true)] out CommandRegistration? registration)
    {
        foreach (var intf in t.GetInterfaces())
        {
            if (!intf.IsGenericType || intf.GetGenericTypeDefinition() != typeof(ICommand<>))
            {
                continue;
            }

            registration = new CommandRegistration(t, intf.GetGenericArguments()[0], intf);
            return true;
        }

        registration = default;
        return false;
    }
}
