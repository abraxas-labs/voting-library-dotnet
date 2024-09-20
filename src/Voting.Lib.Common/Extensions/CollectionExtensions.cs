// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System.Collections.Generic;

/// <summary>
/// Extensions to <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns a read only adapter of an <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <typeparam name="T">The element type.</typeparam>
    /// <returns>The read only collection view over the source.</returns>
    public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> source)
        => source as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(source);

    private sealed class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _source;

        public ReadOnlyCollectionAdapter(ICollection<T> source)
        {
            _source = source;
        }

        public int Count => _source.Count;

        public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
