// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Eventing;

/// <summary>
/// Well known event streams.
/// </summary>
public static class WellKnownStreams
{
    /// <summary>
    /// The EventStore All stream, containing all events.
    /// </summary>
    public const string All = "$all";

    /// <summary>
    /// The VOTING category stream, containing all VOTING events.
    /// </summary>
    public const string CategoryVoting = "$ce-voting";
}
