// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Common;

/// <summary>
/// Represents a diff of two enumerables.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class EnumerableDiff<T>
{
    internal EnumerableDiff(IReadOnlyCollection<T> added, IReadOnlyCollection<T> modified, IReadOnlyCollection<T> removed)
    {
        Added = added;
        Modified = modified;
        Removed = removed;
    }

    /// <summary>
    /// Gets the added items.
    /// </summary>
    public IReadOnlyCollection<T> Added { get; }

    /// <summary>
    /// Gets the modified items.
    /// </summary>
    public IReadOnlyCollection<T> Modified { get; }

    /// <summary>
    /// Gets the removed items.
    /// </summary>
    public IReadOnlyCollection<T> Removed { get; }
}
