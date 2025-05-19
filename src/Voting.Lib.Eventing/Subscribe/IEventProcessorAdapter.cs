// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// Event processor adapter interface.
/// </summary>
public interface IEventProcessorAdapter
{
    /// <summary>
    /// Process an event.
    /// </summary>
    /// <param name="eventWithMetadata">The event data.</param>
    /// <param name="isCatchUp">A value indicating whether the processor is in catch up mode.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Process(EventWithMetadata eventWithMetadata, bool isCatchUp);
}
