// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// A context describing the current event processing.
/// </summary>
/// <param name="IsCatchUp">Whether this event is processed due to catch-up processing.</param>
/// <param name="Event">The event data.</param>
public record EventProcessorContext(bool IsCatchUp, EventWithMetadata Event);
