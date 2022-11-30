// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// A repository handler which methods are invoked by the <see cref="IAggregateRepository"/> lifecycle.
/// </summary>
public interface IAggregateRepositoryHandler
{
    /// <summary>
    /// A method fired at the beginning of a <see cref="IAggregateRepository.Save"/> call.
    /// </summary>
    /// <typeparam name="TAggregate">Type of aggregate.</typeparam>
    /// <param name="aggregate">The aggregate on which the Save is called.</param>
    /// <returns>A completed Task.</returns>
    Task BeforeSaved<TAggregate>(TAggregate aggregate)
        where TAggregate : BaseEventSourcingAggregate;

    /// <summary>
    /// A method fired in the <see cref="IAggregateRepository.Save"/> call after the events are successfully published.
    /// </summary>
    /// <typeparam name="TAggregate">Type of aggregate.</typeparam>
    /// <param name="aggregate">The aggregate on which the Save is called.</param>
    /// <param name="publishedEvents">The successfully published events.</param>
    /// <returns>A completed Task.</returns>
    Task AfterSaved<TAggregate>(TAggregate aggregate, IReadOnlyCollection<IDomainEvent> publishedEvents)
        where TAggregate : BaseEventSourcingAggregate;
}
