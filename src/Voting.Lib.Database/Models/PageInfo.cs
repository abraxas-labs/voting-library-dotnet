// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Database.Models;

/// <summary>
/// Class which contains information about a page.
/// </summary>
public class PageInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageInfo"/> class.
    /// </summary>
    public PageInfo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageInfo"/> class.
    /// </summary>
    /// <param name="totalItemsCount">The total count of items that exist, if no paging were used.</param>
    /// <param name="itemsCount">The count of items on the current page.</param>
    /// <param name="currentPage">The 1-based index of this page.</param>
    /// <param name="pageSize">The page size.</param>
    public PageInfo(int totalItemsCount, int itemsCount, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItemsCount = totalItemsCount;
        ItemsCount = itemsCount;

        TotalPages = Math.Max(1, (int)Math.Ceiling(totalItemsCount / (double)pageSize));
    }

    /// <summary>
    /// Gets 1-Based index of the current page.
    /// </summary>
    public int CurrentPage { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total count of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the count of items in this page.
    /// </summary>
    public int ItemsCount { get; }

    /// <summary>
    /// Gets the total count of items that exist.
    /// </summary>
    public int TotalItemsCount { get; }

    /// <summary>
    /// Gets a value indicating whether a previous page exists.
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether a next page exists.
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;
}
