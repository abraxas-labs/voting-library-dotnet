// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Database.Models;

/// <summary>
/// Class which contains pagination information.
/// </summary>
public class Pageable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pageable"/> class.
    /// </summary>
    public Pageable()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pageable"/> class.
    /// </summary>
    /// <param name="page">The 1-based index of the requested page.</param>
    /// <param name="pageSize">The page size.</param>
    public Pageable(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>
    /// Gets or sets 1-based index of the requested page.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }
}
