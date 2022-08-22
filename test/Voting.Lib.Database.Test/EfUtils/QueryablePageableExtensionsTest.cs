// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Models;
using Xunit;

namespace Voting.Lib.Database.Test.EfUtils;

public class QueryablePageableExtensionsTest : BaseDbContextTest
{
    [Fact]
    public async Task ShouldWorkForRandomPage()
    {
        var page = await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(2, 3);
        page.PageSize.Should().Be(3);
        page.ItemsCount.Should().Be(3);
        page.HasPreviousPage.Should().BeTrue();
        page.HasNextPage.Should().BeTrue();
        page.CurrentPage.Should().Be(2);
        page.TotalPages.Should().Be(7);
        page.TotalItemsCount.Should().Be(20);
        page.Items.Select(x => x.Value).Should().BeEquivalentTo(Enumerable.Range(3, 3));
    }

    [Fact]
    public async Task ShouldWorkForFirstPage()
    {
        var page = await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(1, 3);
        page.PageSize.Should().Be(3);
        page.ItemsCount.Should().Be(3);
        page.HasPreviousPage.Should().BeFalse();
        page.HasNextPage.Should().BeTrue();
        page.CurrentPage.Should().Be(1);
        page.TotalPages.Should().Be(7);
        page.TotalItemsCount.Should().Be(20);
        page.Items.Select(x => x.Value).Should().BeEquivalentTo(Enumerable.Range(0, 3));
    }

    [Fact]
    public async Task ShouldWorkForLastPage()
    {
        var page = await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(7, 3);
        page.PageSize.Should().Be(3);
        page.ItemsCount.Should().Be(2);
        page.HasPreviousPage.Should().BeTrue();
        page.HasNextPage.Should().BeFalse();
        page.CurrentPage.Should().Be(7);
        page.TotalPages.Should().Be(7);
        page.TotalItemsCount.Should().Be(20);
        page.Items.Select(x => x.Value).Should().BeEquivalentTo(new[] { 18, 19 });
    }

    [Fact]
    public async Task ShouldWorkWithPageable()
    {
        var page = await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(new Pageable(1, 3));
        page.PageSize.Should().Be(3);
        page.ItemsCount.Should().Be(3);
        page.HasPreviousPage.Should().BeFalse();
        page.HasNextPage.Should().BeTrue();
        page.CurrentPage.Should().Be(1);
        page.TotalPages.Should().Be(7);
        page.TotalItemsCount.Should().Be(20);
        page.Items.Select(x => x.Value).Should().BeEquivalentTo(Enumerable.Range(0, 3));
    }

    [Fact]
    public async Task ShouldThrowForPageZero()
    {
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(0, 100));
        ex.ParamName.Should().Be("currentPage");
    }

    [Fact]
    public async Task ShouldThrowForPageSizeZero()
    {
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(1, 0));
        ex.ParamName.Should().Be("pageSize");
    }

    [Fact]
    public async Task ShouldReturnEmptyForLastPlusOnePage()
    {
        var page = await Context.TestEntities
            .OrderBy(x => x.Value)
            .ToPageAsync(2, 100);
        page.ItemsCount.Should().Be(0);
        page.Items.Should().BeEmpty();
        page.TotalPages.Should().Be(1);
        page.HasPreviousPage.Should().BeTrue();
        page.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public Task ShouldThrowOnSkippedOverflow()
    {
        return Assert.ThrowsAsync<OverflowException>(async () => await Context.TestEntities.ToPageAsync(3, int.MaxValue));
    }
}
