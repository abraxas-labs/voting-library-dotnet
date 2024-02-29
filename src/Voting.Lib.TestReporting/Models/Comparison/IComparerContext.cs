// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.TestReporting.Models.Comparison;

/// <summary>
/// The comparer context model holding context data for the comparers.
/// </summary>
public interface IComparerContext
{
    /// <summary>
    /// Gets the root comparison container which is getting enriched by the comparers.
    /// </summary>
    public ComparisonContainer Comparisons { get; }
}
