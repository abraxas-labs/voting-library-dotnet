// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;

namespace Voting.Lib.Database.Models;

/// <summary>
/// Base Entity to be used by the provided repositories.
/// </summary>
/// <typeparam name="TKey">Type of the Key.</typeparam>
public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    where TKey : struct
{
    /// <inheritdoc />
    [Key]
    public TKey Id { get; set; }
}

/// <summary>
/// Base entity with a key type <see cref="System.Guid"/>.
/// </summary>
public abstract class BaseEntity : BaseEntity<Guid>
{
}
