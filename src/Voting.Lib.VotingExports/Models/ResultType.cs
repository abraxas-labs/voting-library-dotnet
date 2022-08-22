// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.VotingExports.Models;

/// <summary>
/// The type of the result.
/// </summary>
public enum ResultType
{
    /// <summary>
    /// A single political business on a single counting circle.
    /// </summary>
    CountingCircleResult,

    /// <summary>
    /// A political business across all counting circles.
    /// </summary>
    PoliticalBusinessResult,

    /// <summary>
    /// Multiple political businesses across all counting circles.
    /// </summary>
    MultiplePoliticalBusinessesResult,

    /// <summary>
    /// Multiple political businesses across a single counting circle.
    /// </summary>
    MultiplePoliticalBusinessesCountingCircleResult,

    /// <summary>
    /// A single contest.
    /// </summary>
    Contest,

    /// <summary>
    /// Multiple political businesses within a political business union.
    /// </summary>
    PoliticalBusinessUnionResult,
}
