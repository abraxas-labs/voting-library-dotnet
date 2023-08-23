// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class CollectionExtensionsTest
{
    [Fact]
    public void ShouldCastIfPossible()
    {
        ICollection<int> arr = new[] { 1, 2, 3 };
        var readOnlyCollection = arr.AsReadOnly();
        ReferenceEquals(arr, readOnlyCollection).Should().BeTrue();
    }

    [Fact]
    public void ShouldUseAdapterIfCannotCast()
    {
        ICollection<int> collection = new NonReadOnlyCollection();
        var readOnlyCollection = collection.AsReadOnly();
        ReferenceEquals(collection, readOnlyCollection).Should().BeFalse();
        collection.SequenceEqual(readOnlyCollection).Should().BeTrue();
    }

    private class NonReadOnlyCollection : ICollection<int>
    {
        private readonly ICollection<int> _delegate = new[] { 1, 2, 3 };

        public int Count => _delegate.Count;

        public bool IsReadOnly => true;

        public IEnumerator<int> GetEnumerator() => _delegate.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(int item)
            => throw new System.NotImplementedException();

        public void Clear()
            => throw new System.NotImplementedException();

        public bool Contains(int item)
            => throw new System.NotImplementedException();

        public void CopyTo(int[] array, int arrayIndex)
            => throw new System.NotImplementedException();

        public bool Remove(int item)
            => throw new System.NotImplementedException();
    }
}
