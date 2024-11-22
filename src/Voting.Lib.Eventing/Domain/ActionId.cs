// (c) Copyright by Abraxas Informatik AG
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
    /// <param name="aggregateVersions">The included aggregate versions.</param>
    public ActionId(string action, params IEventSourcingAggregateVersion[] aggregateVersions)
    {
        Action = action;
        AggregateVersions = aggregateVersions;
    }

    /// <summary>
    /// Gets the action, i.e. 'SubmissionFinished'.
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// Gets the aggregates.
    /// </summary>
    public IReadOnlyCollection<IEventSourcingAggregateVersion> AggregateVersions { get; }

    /// <summary>
    /// Computes the SHA256 hash of this action ID.
    /// The action ID includes the concatenated aggregates signature prepended with the action name,
    /// i.e. 'SubmissionFinished-aggregate1-{GUID}@1-aggregate2-{GUID}@1'.
    /// </summary>
    /// <returns>Returns the computed hash.</returns>
    public string ComputeHash()
    {
        var aggregateVersions = AggregateVersions.Select(x => $"{x.StreamName}@{x.Version}");
        var input = string.Join(ActionIdSeparator, aggregateVersions.Prepend(Action));
        return HashUtil.GetSHA256Hash(input);
    }
}
