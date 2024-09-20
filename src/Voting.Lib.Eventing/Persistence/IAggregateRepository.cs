// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Exceptions;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Interface for loading and saving aggregates.
/// </summary>
public interface IAggregateRepository
{
    /// <summary>
    /// Load an aggregate by its id.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <param name="id">The aggregate id to load.</param>
    /// <returns>The loaded aggregate.</returns>
    /// <exception cref="AggregateNotFoundException">If the aggregate was not found.</exception>
    Task<TAggregate> GetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate;

    /// <summary>
    /// Try to load an aggregate by its id.
    /// </summary>
    /// <param name="id">The id of the aggregate.</param>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <returns>The loaded aggregate or null if it wasn't found.</returns>
    Task<TAggregate?> TryGetById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate;

    /// <summary>
    /// Loads an aggregate by its id or returns a new aggregate if none was found.
    /// </summary>
    /// <param name="id">The id of the aggregate.</param>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <returns>The loaded or created aggregate.</returns>
    Task<TAggregate> GetOrCreateById<TAggregate>(Guid id)
        where TAggregate : BaseEventSourcingAggregate;

    /// <summary>
    /// Load an aggregate partially.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to load.</typeparam>
    /// <param name="id">The aggregate id to load.</param>
    /// <param name="endTimestampInclusive">Reads from beginning to this timestamp (created timestamp).</param>
    /// <returns>The loaded aggregate.</returns>
    /// <exception cref="AggregateNotFoundException">If the aggregate was not found.</exception>
    // TODO: change datetime to position with https://jira.abraxas-tools.ch/jira/browse/VOTING-1856.
    Task<TAggregate> GetSnapshotById<TAggregate>(Guid id, DateTime endTimestampInclusive)
        where TAggregate : BaseEventSourcingAggregate;

    /// <summary>
    /// Saves an aggregate.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to save.</typeparam>
    /// <param name="aggregate">The aggregate to save.</param>
    /// <param name="disableIdempotencyGuarantee">Whether to disable the idempotency guarantee,
    /// see <see cref="IEventPublisher.PublishWithoutIdempotencyGuarantee(string,Voting.Lib.Eventing.Persistence.EventWithMetadata)"/>.
    /// Note that when setting this parameter to true, the aggregate version may be inaccurate.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Save<TAggregate>(TAggregate aggregate, bool disableIdempotencyGuarantee = false)
        where TAggregate : BaseEventSourcingAggregate;
}
