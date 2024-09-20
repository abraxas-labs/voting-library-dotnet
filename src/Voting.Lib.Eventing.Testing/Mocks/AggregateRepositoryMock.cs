// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Testing.Mocks;

/// <summary>
/// This is a mock implementation of <see cref="IAggregateRepository"/>.
/// This needs to be registered as scoped since it injects scoped services.
/// Otherwise it would leak the scope.
/// </summary>
public class AggregateRepositoryMock : IAggregateRepository
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IAggregateFactory _aggregateFactory;
    private readonly AggregateRepositoryMockStore _store;
    private readonly IAggregateRepositoryHandler _aggregateRepositoryHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRepositoryMock"/> class.
    /// </summary>
    /// <param name="eventPublisher">The mock event publisher.</param>
    /// <param name="aggregateFactory">The mock aggregate factory.</param>
    /// <param name="store">The mock aggregate store.</param>
    /// <param name="aggregateRepositoryHandler">The aggregate repository handler.</param>
    public AggregateRepositoryMock(
        IEventPublisher eventPublisher,
        IAggregateFactory aggregateFactory,
        AggregateRepositoryMockStore store,
        IAggregateRepositoryHandler aggregateRepositoryHandler)
    {
        _eventPublisher = eventPublisher;
        _aggregateFactory = aggregateFactory;
        _store = store;
        _aggregateRepositoryHandler = aggregateRepositoryHandler;
    }

    /// <summary>
    /// Reconstructs the aggregate by applying all saved events.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <param name="id">The aggregate id.</param>
    /// <returns>The loaded aggregate.</returns>
    /// <exception cref="AggregateNotFoundException">If the aggregate was not found.</exception>
    public async Task<TAggregate> GetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        return await TryGetById<TAggregate>(id).ConfigureAwait(false)
            ?? throw new AggregateNotFoundException(id);
    }

    /// <summary>
    /// Reconstructs the aggregate by applying all saved events.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <param name="id">The aggregate id.</param>
    /// <returns>The loaded aggregate or null if it wasn't found.</returns>
    public Task<TAggregate?> TryGetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        var aggregate = _aggregateFactory.New<TAggregate>();

        if (!_store.TryGetEvents(aggregate.AggregateName, id, out var events))
        {
            return Task.FromResult<TAggregate?>(null);
        }

        foreach (var ev in events)
        {
            aggregate.ApplyEvent(ev);
        }

        return Task.FromResult<TAggregate?>(aggregate);
    }

    /// <inheritdoc />
    public async Task<TAggregate> GetOrCreateById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate
    {
        return await TryGetById<TAggregate>(id).ConfigureAwait(false)
            ?? _aggregateFactory.New<TAggregate>();
    }

    /// <summary>
    /// Clears the published events list.
    /// </summary>
    public void Clear() => _store.Clear();

    /// <summary>
    /// Saves all uncommitted events of an aggregate in memory.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to save.</typeparam>
    /// <param name="aggregate">The aggregate to save.</param>
    /// <param name="disableIdempotencyGuarantee">Has no effect in the mock.</param>
    /// <returns>A task representing the asynchonous operation.</returns>
    public async Task Save<TAggregate>(TAggregate aggregate, bool disableIdempotencyGuarantee = false)
        where TAggregate : BaseEventSourcingAggregate
    {
        await _aggregateRepositoryHandler.BeforeSaved(aggregate).ConfigureAwait(false);

        var events = aggregate.GetUncommittedEvents().ToList();
        foreach (var ev in events)
        {
            _store.AddEvent(aggregate.AggregateName, aggregate.Id, ev);
            var eventWithMetadata = new EventWithMetadata(ev.Data, ev.Metadata, ev.Id);
            await _eventPublisher.Publish(aggregate.Id.ToString(), eventWithMetadata, null).ConfigureAwait(false);
        }

        aggregate.ClearUncommittedEvents();
        await _aggregateRepositoryHandler.AfterSaved(aggregate, events).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<TAggregate> GetSnapshotById<TAggregate>(Guid id, DateTime endTimestampInclusive)
        where TAggregate : BaseEventSourcingAggregate
    {
        var aggregate = _aggregateFactory.New<TAggregate>();

        if (!_store.TryGetEvents(aggregate.AggregateName, id, out var events))
        {
            throw new AggregateNotFoundException(id);
        }

        foreach (var ev in events)
        {
            if (ev.Created > endTimestampInclusive)
            {
                break;
            }

            aggregate.ApplyEvent(ev);
        }

        return Task.FromResult(aggregate);
    }
}
