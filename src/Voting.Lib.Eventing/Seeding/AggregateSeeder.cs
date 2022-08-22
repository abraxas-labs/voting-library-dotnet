// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Seeding;

internal class AggregateSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventSeeder _eventSeeder;
    private readonly ILogger<AggregateSeeder> _logger;

    public AggregateSeeder(
        IServiceProvider serviceProvider,
        IEventSeeder eventSeeder,
        ILogger<AggregateSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _eventSeeder = eventSeeder;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // generating aggregates/events ofter requires a DI scope (for example the IAggregateFactory)
        await using var scope = _serviceProvider.CreateAsyncScope();
        var seedSources = scope.ServiceProvider.GetRequiredService<IEnumerable<IAggregateSeedSource>>();
        _logger.LogInformation("Start seeding {Count} aggregates...", seedSources.Count());
        await SeedAllAggregates(seedSources).ConfigureAwait(false);
        _logger.LogInformation("Finished seeding aggregates");
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private async Task SeedAllAggregates(IEnumerable<IAggregateSeedSource> seedSources)
    {
        foreach (var seedSource in seedSources)
        {
            var aggregate = await seedSource.GetAggregate().ConfigureAwait(false);
            await Seed(aggregate).ConfigureAwait(false);
        }
    }

    private async Task Seed(BaseEventSourcingAggregate aggregate)
    {
        _logger.LogInformation("Seeding {Name} aggregate...", aggregate.AggregateName);

        var events = aggregate.GetUncommittedEvents();
        await _eventSeeder.Seed(aggregate.StreamName, events.Select(x => x.Data)).ConfigureAwait(false);

        _logger.LogInformation("Finished seeding {Name} aggregate", aggregate.AggregateName);
    }
}
