// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.TestReporting.Models.Comparison;

/// <summary>
/// A comparison entry which holds detailed information about the kind of comparison and whether the comparison result is equal or not.
/// </summary>
public interface IComparisonEntry
{
    /// <summary>
    /// Gets the comparison's parent description.
    /// </summary>
    string ParentDescription { get; }

    /// <summary>
    /// Gets the comparison's full description.
    /// </summary>
    string FullDescription { get; }

    /// <summary>
    /// Gets the comparison's detailed description.
    /// </summary>
    string DetailedDescription { get; }

    /// <summary>
    /// Gets the comparison's description.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the comparison name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the compared values are equal or not.
    /// </summary>
    bool Equal { get; }
}
