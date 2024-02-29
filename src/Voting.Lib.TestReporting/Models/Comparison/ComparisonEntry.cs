// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.TestReporting.Models.Comparison;

/// <inheritdoc />
public class ComparisonEntry<T> : IComparisonEntry
    where T : IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonEntry{T}"/> class.
    /// </summary>
    /// <param name="parentDescription">The comparison's parent description.</param>
    /// <param name="name">The comparison name.</param>
    /// <param name="valueCompareLeft">The compare left value.</param>
    /// <param name="valueCompareRight">The compare right value.</param>
    internal ComparisonEntry(string parentDescription, string name, T valueCompareLeft, T valueCompareRight)
    {
        Equal = ReferenceEquals(valueCompareLeft, valueCompareRight) || valueCompareLeft.CompareTo(valueCompareRight) == 0;
        Name = name;
        ParentDescription = parentDescription;
        Description = $"[{name}]: Left: {valueCompareLeft}, Right: {valueCompareRight}";
        FullDescription = $"{ParentDescription} {Description}";
        DetailedDescription = $"{ParentDescription} {Environment.NewLine}{Environment.NewLine}" +
                              $"{Name} {Environment.NewLine}{Environment.NewLine}" +
                              $"Left: {valueCompareLeft}{Environment.NewLine}" +
                              $"Right: {valueCompareRight}{Environment.NewLine}";
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string ParentDescription { get; }

    /// <inheritdoc />
    public string FullDescription { get; }

    /// <inheritdoc />
    public string DetailedDescription { get; }

    /// <inheritdoc />
    public string Description { get; }

    /// <inheritdoc />
    public bool Equal { get; }
}
