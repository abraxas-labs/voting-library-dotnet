// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Voting.Lib.Database.Postgres.Locking;
using Xunit;

namespace Voting.Lib.Database.Postgres.IntegrationTest;

public class PostgresLockTest : PostgresFixture
{
    [Fact]
    public async Task ForUpdateSkipLockedShouldWork()
    {
        await using var db1 = NewContext();
        db1.Objects.AddRange(
            new TestObject { Value = 1 },
            new TestObject { Value = 2 },
            new TestObject { Value = 3 });
        await db1.SaveChangesAsync();

        await using (var t1 = await db1.Database.BeginTransactionAsync())
        {
            var result1 = await db1.Objects
                .Where(x => x.Value > 1)
                .OrderBy(x => x.Value)
                .ForUpdateSkipLocked()
                .ToListAsync();
            result1.Select(x => x.Value)
                .Should()
                .BeEquivalentTo([2, 3], o => o.WithStrictOrdering());

            await using var db2 = NewContext();
            var result2 = await db2.Objects
                .OrderBy(x => x.Value)
                .ForUpdateSkipLocked()
                .ToListAsync();
            result2.Select(x => x.Value)
                .Should()
                .BeEquivalentTo([1], o => o.WithStrictOrdering());
        }

        await using var db3 = NewContext();
        var result3 = await db3.Objects
            .OrderBy(x => x.Value)
            .ForUpdateSkipLocked()
            .ToListAsync();
        result3.Select(x => x.Value)
            .Should()
            .BeEquivalentTo([1, 2, 3], o => o.WithStrictOrdering());
    }

    [Fact]
    public async Task ForUpdateShouldWork()
    {
        await using var db1 = NewContext();
        db1.Objects.AddRange(
            new TestObject { Value = 1 },
            new TestObject { Value = 2 },
            new TestObject { Value = 3 });
        await db1.SaveChangesAsync();

        await using (var t1 = await db1.Database.BeginTransactionAsync())
        {
            var result1 = await db1.Objects
                .Where(x => x.Value > 1)
                .OrderBy(x => x.Value)
                .ForUpdate()
                .ToListAsync();
            result1.Select(x => x.Value)
                .Should()
                .BeEquivalentTo([2, 3], o => o.WithStrictOrdering());

            // another ForUpdate in the same transaction should block / timeout
            var fakeTimeProvider = new FakeTimeProvider();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1), fakeTimeProvider);
            await using var db2 = NewContext();
            var throwsTask = Assert.ThrowsAsync<TaskCanceledException>(async () => await db2.Objects
                .OrderBy(x => x.Value)
                .ForUpdate()
                .ToListAsync(cts.Token));
            fakeTimeProvider.Advance(TimeSpan.FromSeconds(2));
            await throwsTask;
        }

        // in another transaction should work again
        await using var db3 = NewContext();
        var result3 = await db3.Objects
            .OrderBy(x => x.Value)
            .ForUpdate()
            .ToListAsync();
        result3.Select(x => x.Value)
            .Should()
            .BeEquivalentTo([1, 2, 3], o => o.WithStrictOrdering());
    }
}
