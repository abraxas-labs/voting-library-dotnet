// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Database.Models;

/// <summary>
/// Class which contains paged data.
/// </summary>
/// <typeparam name="T">Type of the paged data.</typeparam>
public class Page<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Page{T}"/> class.
    /// </summary>
    public Page()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Page{T}"/> class.
    /// </summary>
    /// <param name="items">The items in this page.</param>
    /// <param name="count">The total count of items that exist, if no paging were used.</param>
    /// <param name="currentPage">The 1-based index of this page.</param>
    /// <param name="pageSize">The page size.</param>
    public Page(IEnumerable<T> items, int count, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItemsCount = count;

        TotalPages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        Items.AddRange(items);
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
    /// Gets the items of this page.
    /// </summary>
    public List<T> Items { get; } = new List<T>();

    /// <summary>
    /// Gets the count of items in this page.
    /// </summary>
    public int ItemsCount => Items.Count;

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
