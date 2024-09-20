// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Test;
using Voting.Lib.Database.Testing.NQueryDetector;
using Xunit;

namespace Voting.Lib.Database.Testing.Test.NQueryDetector;

public class NQueryDetectorTest : BaseDbContextTest<NQueryDetectorTestContext>
{
    [Fact]
    public async Task ShouldFailTooManyAsyncQueries()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var query = Context.TestEntities.Where(x => x.Value > 10);
        await query.ToListAsync();

        var ex = await Assert.ThrowsAsync<QueryRunTooManyTimesException>(async () => await query.ToListAsync());
        ex.ActualCount.Should().Be(2);
        ex.MaxCount.Should().Be(1);
        ex.Query.Replace("\r", string.Empty).Should().Be(
            "SELECT \"t\".\"Id\", \"t\".\"Name\", \"t\".\"Value\"\nFROM \"TestEntities\" AS \"t\"\nWHERE \"t\".\"Value\" > 10");
    }

    [Fact]
    public void ShouldFailTooManyQueries()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var query = Context.TestEntities.Where(x => x.Value > 10);
        _ = query.ToList();

        var ex = Assert.Throws<QueryRunTooManyTimesException>(() => query.ToList());
        ex.ActualCount.Should().Be(2);
        ex.MaxCount.Should().Be(1);
    }

    [Fact]
    public async Task ShouldWorkFailWithTooManyUpdateQueries()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var entities = await Context.TestEntities
            .AsTracking()
            .Where(x => x.Value > 10)
            .ToListAsync();

        entities[0].Value += 1000;
        await Context.SaveChangesAsync();

        entities[1].Value += 1000;
        var ex = await Assert.ThrowsAsync<DbUpdateException>(async () => await Context.SaveChangesAsync());
        ex.InnerException.Should().BeOfType<QueryRunTooManyTimesException>();
    }

    [Fact]
    public async Task ShouldWorkFailWithTooManyDeleteQueries()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var entities = await Context.TestEntities
            .AsTracking()
            .Where(x => x.Value > 10)
            .ToListAsync();

        Context.Remove(entities[0]);
        await Context.SaveChangesAsync();

        Context.Remove(entities[1]);
        var ex = await Assert.ThrowsAsync<DbUpdateException>(async () => await Context.SaveChangesAsync());
        ex.InnerException.Should().BeOfType<QueryRunTooManyTimesException>();
    }

    [Fact]
    public async Task ShouldWorkWithSql()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var sql = "SELECT CURRENT_TIMESTAMP";

        await Context.Database.ExecuteSqlRawAsync(sql);

        await Assert.ThrowsAsync<QueryRunTooManyTimesException>(async () => await Context.Database.ExecuteSqlRawAsync(sql));
    }

    [Fact]
    public void ShouldBeOk()
    {
        using var span = Context.CreateNQueryDetectorSpan(3);
        var query = Context.TestEntities.Where(x => x.Value > 10).Take(1);
        _ = query.ToList();
        _ = query.ToList();

        var counters = span.QueryCounters;
        counters.Should().HaveCount(1);
        counters.Should().ContainValue(2);
    }

    [Fact]
    public async Task ShouldBeOkAsync()
    {
        using var span = Context.CreateNQueryDetectorSpan(3);
        var query = Context.TestEntities.Where(x => x.Value > 10).Take(1);
        await query.ToListAsync();
        await query.ToListAsync();

        var counters = span.QueryCounters;
        counters.Should().HaveCount(1);
        counters.Should().ContainValue(2);
    }

    [Fact]
    public async Task ShouldBeOkWithAnotherContext()
    {
        using var span = Context.CreateNQueryDetectorSpan(2);
        await Context.TestEntities.ToListAsync();

        await using var ctx2 = new NQueryDetectorTestContext(false);

        using var span2 = ctx2.CreateNQueryDetectorSpan();
        await ctx2.Database.EnsureCreatedAsync();

        // second query on second context should throw
        await ctx2.TestEntities.ToListAsync();
        await Assert.ThrowsAsync<QueryRunTooManyTimesException>(async () => await ctx2.TestEntities.ToListAsync());

        // third query on primary context should throw
        await Context.TestEntities.ToListAsync();
        await Assert.ThrowsAsync<QueryRunTooManyTimesException>(async () => await Context.TestEntities.ToListAsync());
    }

    [Fact]
    public async Task ShouldThrowWithAnotherContextAndGlobal()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        await Context.TestEntities.ToListAsync();

        await using var ctx2 = new NQueryDetectorTestContext();

        using var span2 = ctx2.CreateNQueryDetectorSpan();
        await ctx2.Database.EnsureCreatedAsync();
        await Assert.ThrowsAsync<QueryRunTooManyTimesException>(async () => await ctx2.TestEntities.ToListAsync());
    }

    [Fact]
    public async Task ShouldWorkWithIgnore()
    {
        using var span = Context.CreateNQueryDetectorSpan();
        var query = Context.TestEntities.Where(x => x.Value > 10).Take(1);
        await query.ToListAsync();
        await query.IgnoreNQueryDetector().ToListAsync();
        span.QueryCounters.Should().HaveCount(1);
    }
}
