// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Voting.Lib.Common.Test;

public class EnumerableExtensionsTest
{
    [Theory]
    [InlineData("equal-items-and-same-order", new int[] { 10, 11, 12 }, new int[] { 10, 11, 12 }, true)]
    [InlineData("equal-items-and-different-order", new int[] { 10, 11, 12 }, new int[] { 11, 10, 12 }, false)]
    [InlineData("empty", new int[] { }, new int[] { }, true)]
    public void TestGetSequenceHashCode(string testName, IEnumerable<int> left, IEnumerable<int> right, bool isEqual)
    {
        var leftSequenceHashCode = left.GetSequenceHashCode();
        var rightSequenceHashCode = right.GetSequenceHashCode();

        leftSequenceHashCode.Equals(rightSequenceHashCode)
            .Should()
            .Be(isEqual, testName);
    }

    [Fact]
    public void TestNotNullClass()
    {
        new[] { new Data { Value = 1 }, null, null, new Data { Value = 2 } }
            .WhereNotNull()
            .Should()
            .BeEquivalentTo(new[] { new Data { Value = 1 }, new Data { Value = 2 } });
    }

    [Fact]
    public void TestNotNullStruct()
    {
        new int?[] { 1, null, null, 2 }
            .WhereNotNull()
            .Should()
            .BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public void TestDiff()
    {
        var original = new[]
        {
                new Data { Value = 1 },
                new Data { Value = 2 },
                new Data { Value = 3 },
        };

        var modified = new[]
        {
                new Data { Value = 1 },
                new Data { Value = 2, Value2 = 10 },
                new Data { Value = 4 },
                new Data { Value = 5 },
        };

        var diff = original.BuildDiff(modified, x => x.Value);
        diff.Added.Select(x => x.Value).Should().BeEquivalentTo(new[] { 4, 5 });
        diff.Removed.Select(x => x.Value).Should().BeEquivalentTo(new[] { 3 });
        diff.Modified.Should().BeEquivalentTo(new[] { new Data { Value = 2, Value2 = 10 } });
    }

    public class Data
    {
        public int Value { get; set; }

        public int Value2 { get; set; }

        public override bool Equals(object? obj)
            => obj is Data d && d.Value == Value && d.Value2 == Value2;

        public override int GetHashCode() => HashCode.Combine(Value, Value2);
    }
}
