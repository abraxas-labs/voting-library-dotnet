// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using Voting.Lib.Common;

namespace Voting.Lib.Eventing.Domain;

/// <summary>
/// An action ID represents an action taking place on a stream.
/// </summary>
public class ActionId
{
    private const string ActionIdSeparator = "-";

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionId"/> class.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="aggregates">The aggregates.</param>
    public ActionId(string action, params BaseEventSourcingAggregate[] aggregates)
    {
        Action = action;
        Aggregates = aggregates;
    }

    /// <summary>
    /// Gets the action, i.e. 'SubmissionFinished'.
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// Gets the aggregates.
    /// </summary>
    public IReadOnlyCollection<BaseEventSourcingAggregate> Aggregates { get; }

    /// <summary>
    /// Computes the SHA256 hash of this action ID.
    /// The action ID includes the concatenated aggregates signature prepended with the action name,
    /// i.e. 'SubmissionFinished-aggregate1-{GUID}@1-aggregate2-{GUID}@1'.
    /// </summary>
    /// <returns>Returns the computed hash.</returns>
    public string ComputeHash()
    {
        var aggregateVersions = Aggregates.Select(x => $"{x.StreamName}@{x.Version}");
        var input = string.Join(ActionIdSeparator, aggregateVersions.Prepend(Action));
        return HashUtil.GetSHA256Hash(input);
    }
}
