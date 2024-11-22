// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;

namespace Voting.Lib.Eventing.Persistence;

/// <inheritdoc/>
public class AggregateRepository : IAggregateRepository
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IAggregateEventReader _aggregateEventReader;
    private readonly IAggregateFactory _aggregateFactory;
    private readonly IAggregateRepositoryHandler _aggregateRepositoryHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRepository"/> class.
    /// </summary>
    /// <param name="eventPublisher">The event publisher.</param>
    /// <param name="aggregateEventReader">The aggregate event reader.</param>
    /// <param name="aggregateFactory">The aggregate factory.</param>
    /// <param name="aggregateRepositoryHandler">The aggregate repository handler.</param>
    public AggregateRepository(
        IEventPublisher eventPublisher,
        IAggregateEventReader aggregateEventReader,
        IAggregateFactory aggregateFactory,
        IAggregateRepositoryHandler aggregateRepositoryHandler)
    {
        _eventPublisher = eventPublisher;
        _aggregateEventReader = aggregateEventReader;
        _aggregateFactory = aggregateFactory;
        _aggregateRepositoryHandler = aggregateRepositoryHandler;
    }

    /// <inheritdoc/>
    public async Task<EventSourcingAggregateVersion?> TryGetVersion<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        var aggregate = _aggregateFactory.New<TAggregate>();
        aggregate.Id = id;

        var version = await _aggregateEventReader.TryGetVersion(aggregate.StreamName, id).ConfigureAwait(false);
        if (version == null)
        {
            return null;
        }

        return new EventSourcingAggregateVersion(aggregate.StreamName, version.Value);
    }

    /// <inheritdoc/>
    public async Task<TAggregate> GetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        return await GetById<TAggregate>(id, null);
    }

    /// <inheritdoc/>
    public async Task<TAggregate> GetSnapshotById<TAggregate>(Guid id, DateTime endTimestampInclusive)
        where TAggregate : BaseEventSourcingAggregate
    {
        return await GetById<TAggregate>(id, endTimestampInclusive);
    }

    /// <inheritdoc/>
    public async Task<TAggregate?> TryGetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        try
        {
            return await GetById<TAggregate>(id).ConfigureAwait(false);
        }
        catch (AggregateNotFoundException)
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<TAggregate> GetOrCreateById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        return await TryGetById<TAggregate>(id).ConfigureAwait(false)
            ?? _aggregateFactory.New<TAggregate>();
    }

    /// <inheritdoc/>
    public async Task Save<TAggregate>(TAggregate aggregate, bool disableIdempotencyGuarantee = false)
        where TAggregate : BaseEventSourcingAggregate
    {
        await _aggregateRepositoryHandler.BeforeSaved(aggregate).ConfigureAwait(false);
        var events = aggregate.GetUncommittedEvents().ToList();
        if (events.Count == 0)
        {
            return;
        }

        var eventsWithMetadata = events.Select(ev => new EventWithMetadata(ev.Data, ev.Metadata, ev.Id));

        if (disableIdempotencyGuarantee)
        {
            await _eventPublisher.PublishWithoutIdempotencyGuarantee(aggregate.StreamName, eventsWithMetadata).ConfigureAwait(false);
        }
        else
        {
            await _eventPublisher.Publish(aggregate.StreamName, eventsWithMetadata, aggregate.OriginalVersion).ConfigureAwait(false);
        }

        aggregate.ClearUncommittedEvents();
        aggregate.OriginalVersion = aggregate.Version!.Value;
        await _aggregateRepositoryHandler.AfterSaved(aggregate, events).ConfigureAwait(false);
    }

    private async Task<TAggregate> GetById<TAggregate>(Guid id, DateTime? endTimestampInclusive)
        where TAggregate : BaseEventSourcingAggregate
    {
        var aggregate = _aggregateFactory.New<TAggregate>();
        aggregate.Id = id;

        await foreach (var @event in _aggregateEventReader.ReadEvents(aggregate.StreamName, id, endTimestampInclusive).ConfigureAwait(false))
        {
            aggregate.ApplyEvent(@event);
        }

        aggregate.OriginalVersion = aggregate.Version ?? StreamRevision.FromStreamPosition(StreamPosition.Start);
        return aggregate;
    }
}
