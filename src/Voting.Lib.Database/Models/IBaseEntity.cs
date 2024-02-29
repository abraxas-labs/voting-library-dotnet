// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Database.Models;

/// <summary>
/// An entity with a Key.
/// </summary>
/// <typeparam name="TKey">Type of the key.</typeparam>
public interface IBaseEntity<TKey>
{
    /// <summary>
    /// Gets or sets the Key. Usually the primary key.
    /// </summary>
    public TKey Id { get; set; }
}
