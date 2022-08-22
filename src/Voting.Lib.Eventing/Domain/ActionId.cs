// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using EventStore.Client;
using Voting.Lib.Common;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// An action ID represents an action taking place on a stream.
/// </summary>
public class ActionId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionId"/> class.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="aggregateStreamName">The aggregate stream name.</param>
    /// <param name="aggregateVersion">The aggregate version.</param>
    public ActionId(string action, string aggregateStreamName, StreamRevision? aggregateVersion)
    {
        Action = action;
        AggregateStreamName = aggregateStreamName;
        AggregateVersion = aggregateVersion;
    }

    /// <summary>
    /// Gets the action.
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// Gets the aggregate stream name.
    /// </summary>
    public string AggregateStreamName { get; }

    /// <summary>
    /// Gets the aggregate version.
    /// </summary>
    public StreamRevision? AggregateVersion { get; }

    /// <summary>
    /// Computes the hash of this action ID.
    /// </summary>
    /// <returns>Returns the computed hash.</returns>
    public string ComputeHash()
    {
        return HashUtil.GetSHA256Hash($"{AggregateStreamName}-{Action}-@{AggregateVersion}");
    }
}
