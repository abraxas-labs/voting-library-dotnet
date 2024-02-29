// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Voting.Lib.Scheduler;

/// <summary>
/// Service collection extensions for scheduled jobs.
/// </summary>
public static class SchedulerServiceCollectionExtensions
{
    /// <summary>
    /// Adds a scheduled job to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="interval">The interval in which the job should run.</param>
    /// <param name="runOnStart">Gets a value indicating whether this job should run immediately on startup.</param>
    /// <typeparam name="TJob">The type of job to run.</typeparam>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddScheduledJob<TJob>(
        this IServiceCollection services,
        TimeSpan interval,
        bool runOnStart = false)
        where TJob : class, IScheduledJob
    {
        return services.AddScheduledJob<TJob>(new JobConfig { Interval = interval, RunOnStart = runOnStart });
    }

    /// <summary>
    /// Adds a scheduled job to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The job configuration.</param>
    /// <typeparam name="TJob">The type of job to run.</typeparam>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddScheduledJob<TJob>(
        this IServiceCollection services,
        IJobConfig config)
        where TJob : class, IScheduledJob
    {
        if (config.Interval <= TimeSpan.Zero)
        {
            throw new ValidationException($"{nameof(config.Interval)} should be greater than 0");
        }

        services.TryAddScoped<TJob>();
        services.AddSingleton<IHostedService>(sp => ActivatorUtilities.CreateInstance<IntervalSchedulerService<TJob>>(sp, config));
        services.TryAddSingleton<JobRunner>();
        return services;
    }

    /// <summary>
    /// Adds a cron job to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="cronSchedule">The cron schedule in which the job should run.</param>
    /// <param name="cronTimeZone">The time zone in which the cron schedule should be evaluated.</param>
    /// <typeparam name="TJob">The type of job to run.</typeparam>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddCronJob<TJob>(
        this IServiceCollection services,
        string cronSchedule,
        string cronTimeZone = "Europe/Zurich")
        where TJob : class, IScheduledJob
    {
        return services.AddCronJob<TJob>(new CronJobConfig { CronSchedule = cronSchedule, CronTimeZone = cronTimeZone });
    }

    /// <summary>
    /// Adds a cron job to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The job configuration.</param>
    /// <typeparam name="TJob">The type of job to run.</typeparam>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection AddCronJob<TJob>(
        this IServiceCollection services,
        ICronJobConfig config)
        where TJob : class, IScheduledJob
    {
        if (string.IsNullOrEmpty(config.CronSchedule))
        {
            throw new ValidationException($"{nameof(config.CronSchedule)} should not be empty");
        }

        if (string.IsNullOrEmpty(config.CronTimeZone))
        {
            throw new ValidationException($"{nameof(config.CronTimeZone)} should not be empty");
        }

        services.TryAddScoped<TJob>();
        services.AddSingleton<IHostedService>(sp => ActivatorUtilities.CreateInstance<CronSchedulerService<TJob>>(sp, config));
        services.TryAddSingleton<JobRunner>();
        return services;
    }
}
