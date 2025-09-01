// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Database.Models;

/// <summary>
/// Class which contains paged data.
/// </summary>
/// <typeparam name="T">Type of the paged data.</typeparam>
public class Page<T> : PageInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Page{T}"/> class.
    /// </summary>
    public Page()
    {
        Items = new List<T>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Page{T}"/> class.
    /// </summary>
    /// <param name="items">The items in this page.</param>
    /// <param name="totalItemsCount">The total count of items that exist, if no paging were used.</param>
    /// <param name="currentPage">The 1-based index of this page.</param>
    /// <param name="pageSize">The page size.</param>
    public Page(List<T> items, int totalItemsCount, int currentPage, int pageSize)
        : base(totalItemsCount, items.Count, currentPage, pageSize)
    {
        Items = items;
    }

    /// <summary>
    /// Gets the items of this page.
    /// </summary>
    public List<T> Items { get; }
}
