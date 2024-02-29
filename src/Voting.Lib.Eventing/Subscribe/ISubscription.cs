// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Eventing.Subscribe;

/// <summary>
/// A stream subscription.
/// </summary>
public interface ISubscription
{
    /// <summary>
    /// Gets the name of the event processor scope.
    /// </summary>
    public string ScopeName { get; }

    /// <summary>
    /// Gets the name of the stream this subscription subscribes to.
    /// </summary>
    string StreamName { get; }
}
