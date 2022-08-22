// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Eventing.Exceptions;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// Base class for event-sourcing aggregates which can be deleted.
/// </summary>
public abstract class BaseDeletableAggregate : BaseEventSourcingAggregate
{
    /// <summary>
    /// Gets or sets a value indicating whether this aggregate is in a deleted state.
    /// </summary>
    public bool Deleted { get; protected set; }

    /// <summary>
    /// Ensures this Aggregate is not deleted.
    /// </summary>
    /// <exception cref="AggregateDeletedException">If the aggregate was already deleted.</exception>
    protected void EnsureNotDeleted()
    {
        if (Deleted)
        {
            throw new AggregateDeletedException(Id);
        }
    }
}
