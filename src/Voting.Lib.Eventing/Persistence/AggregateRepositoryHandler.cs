// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Persistence;

/// <inheritdoc />
public class AggregateRepositoryHandler : IAggregateRepositoryHandler
{
    /// <inheritdoc />
    public Task BeforeSaved<TAggregate>(TAggregate aggregate)
        where TAggregate : BaseEventSourcingAggregate
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task AfterSaved<TAggregate>(TAggregate aggregate, IReadOnlyCollection<IDomainEvent> publishedEvents)
        where TAggregate : BaseEventSourcingAggregate
        => Task.CompletedTask;
}
