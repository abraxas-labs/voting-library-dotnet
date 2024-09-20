// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Voting.Lib.Common;

namespace System.Collections.Generic;

/// <summary>
/// Extensions for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Calculates the sequence hash code of the collection.
    /// </summary>
    /// <typeparam name="T">The item type of the collection.</typeparam>
    /// <param name="items">The items.</param>
    /// <returns>HashCode of the sequence.</returns>
    public static int GetSequenceHashCode<T>(this IEnumerable<T> items)
    {
        HashCode hash = default;
        foreach (var item in items)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Filters null elements and returns an enumerable without any null elements.
    /// Workaround for c# nullable.
    /// </summary>
    /// <param name="enumerable">Source enumerable with possible null entries.</param>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <returns>The filtered enumerable.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        where T : class
    {
#nullable disable
        return enumerable.Where(x => x != null);
#nullable enable
    }

    /// <summary>
    /// Filters null elements and returns an enumerable without any null elements.
    /// Workaround for c# nullable.
    /// </summary>
    /// <param name="enumerable">Source enumerable with possible null entries.</param>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <returns>The filtered enumerable.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        where T : struct
    {
#nullable disable
        return enumerable.Where(x => x.HasValue).Select(x => x.Value);
#nullable enable
    }

    /// <summary>
    /// Builds a diff of two enumerables.
    /// An item is considered as added if it's id does not exist in the existing collection.
    /// An item is considered as modified if it's id exists in the existing collection, but equals returns false.
    /// An item is considered as removed if it's id does exist in the existing collection but not in the modified one.
    /// </summary>
    /// <param name="existing">The existing enumerable.</param>
    /// <param name="modified">The modified enumerable.</param>
    /// <param name="identitySelector">The id selector.</param>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    /// <returns>The diff.</returns>
    public static EnumerableDiff<T> BuildDiff<T, TId>(
        this IEnumerable<T> existing,
        IEnumerable<T> modified,
        Func<T, TId> identitySelector)
        where T : notnull
        where TId : notnull
    {
        var added = new List<T>();
        var updated = new List<T>();

        var existingById = existing.ToDictionary(identitySelector);
        foreach (var modifiedItem in modified)
        {
            var modifiedItemIdentity = identitySelector(modifiedItem);
            if (!existingById.Remove(modifiedItemIdentity, out var existingItem))
            {
                added.Add(modifiedItem);
                continue;
            }

            if (!modifiedItem.Equals(existingItem))
            {
                updated.Add(modifiedItem);
            }
        }

        return new EnumerableDiff<T>(added, updated, existingById.Values);
    }

    /// <summary>
    /// Returns whether two enumerables of decimals are approximately equals.
    /// </summary>
    /// <param name="a">The first enumerable.</param>
    /// <param name="b">The second enumerable.</param>
    /// <param name="precision">Floating point precision of the enumerable elements.</param>
    /// <returns>Approximate equality of the two enumerables.</returns>
    public static bool SequenceApproxEqual(this IEnumerable<decimal> a, IEnumerable<decimal> b, int precision = 10)
    {
        var aArray = a.ToArray();
        var bArray = b.ToArray();

        if (aArray.Length != bArray.Length)
        {
            return false;
        }

        for (var i = 0; i < aArray.Length; i++)
        {
            if (!aArray[i].ApproxEquals(bArray[i], precision))
            {
                return false;
            }
        }

        return true;
    }
}
