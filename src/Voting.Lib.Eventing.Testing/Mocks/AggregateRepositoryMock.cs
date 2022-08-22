// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRepositoryMock"/> class.
    /// </summary>
    /// <param name="eventPublisher">The mock event publisher.</param>
    /// <param name="aggregateFactory">The mock aggregate factory.</param>
    /// <param name="store">The mock aggregate store.</param>
    public AggregateRepositoryMock(
        IEventPublisher eventPublisher,
        IAggregateFactory aggregateFactory,
        AggregateRepositoryMockStore store)
    {
        _eventPublisher = eventPublisher;
        _aggregateFactory = aggregateFactory;
        _store = store;
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
        if (!_store.TryGetEvents(id, out var events))
        {
            return Task.FromResult<TAggregate?>(null);
        }

        var aggregate = _aggregateFactory.New<TAggregate>();
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
    /// <returns>A task representing the asynchonous operation.</returns>
    public async Task Save<TAggregate>(TAggregate aggregate)
        where TAggregate : BaseEventSourcingAggregate
    {
        foreach (var ev in aggregate.GetUncommittedEvents())
        {
            _store.AddEvent(aggregate.Id, ev);
            await _eventPublisher.Publish(aggregate.Id.ToString(), new EventWithMetadata(ev.Data, ev.Metadata, ev.Id), null).ConfigureAwait(false);
        }

        aggregate.ClearUncommittedEvents();
    }

    /// <inheritdoc />
    public Task<TAggregate> GetSnapshotById<TAggregate>(Guid id, DateTime endTimestampInclusive)
        where TAggregate : BaseEventSourcingAggregate
    {
        if (!_store.TryGetEvents(id, out var events))
        {
            throw new AggregateNotFoundException(id);
        }

        var aggregate = _aggregateFactory.New<TAggregate>();
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
