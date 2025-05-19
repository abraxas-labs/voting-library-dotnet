// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
}
