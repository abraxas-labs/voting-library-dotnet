// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Voting.Lib.Eventing.Domain;

namespace Voting.Lib.Eventing.Seeding;

/// <summary>
/// Interface indicating an aggregate seed source.
/// </summary>
public interface IAggregateSeedSource
{
    /// <summary>
    /// Retrieves an aggregate for seeding all of its uncommitted events.
    /// </summary>
    /// <returns>The aggregate to seed.</returns>
    Task<BaseEventSourcingAggregate> GetAggregate();
}
