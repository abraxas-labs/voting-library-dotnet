// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// Factory to create new aggregates.
/// Since some aggregates may use services from the DI, you shouldn't create them yourself.
/// </summary>
public interface IAggregateFactory
{
    /// <summary>
    /// Creates a new, empty aggregate.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type to create.</typeparam>
    /// <returns>The created aggregate.</returns>
    TAggregate New<TAggregate>()
        where TAggregate : BaseEventSourcingAggregate;
}
