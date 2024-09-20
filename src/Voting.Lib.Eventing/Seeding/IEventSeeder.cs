// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Voting.Lib.Eventing.Seeding;

/// <summary>
/// Interface for seeding events.
/// </summary>
public interface IEventSeeder
{
    /// <summary>
    /// Seeds events. Always pass the same collection of events in the same order to this method.
    /// </summary>
    /// <param name="stream">The stream to publish the events to.</param>
    /// <param name="events">The events to seed. Always pass the same (or more) events to this method each time you call it.</param>
    /// <remarks>
    /// Passing more events than last time to this method is allowed. Anything else is discouraged.
    /// Ensure consistent ordering of the events.
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Seed(string stream, IEnumerable<IMessage> events);
}
