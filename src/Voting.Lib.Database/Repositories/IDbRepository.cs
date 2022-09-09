// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Repositories;

/// <summary>
/// Repository for operations on a EF Core DbContext.
/// </summary>
/// <typeparam name="TDbContext">Type of the DbContext.</typeparam>
/// <typeparam name="TKey">Type of the Key of the Entity.</typeparam>
/// <typeparam name="TEntity">Type of the Entity.</typeparam>
public interface IDbRepository<TDbContext, TKey, TEntity>
    where TDbContext : DbContext
    where TKey : struct
    where TEntity : class, IBaseEntity<TKey>, new()
{
    /// <summary>
    /// A new <see cref="IQueryable{TEntity}">Queryable</see> for the current Entity.
    /// </summary>
    /// <returns>An Instance of <see cref="IQueryable{TEntity}">Queryable</see>.</returns>
    IQueryable<TEntity> Query();

    /// <summary>
    /// Returns the entity found by the provided key or null if not found.
    /// </summary>
    /// <param name="key">The key to look for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<TEntity?> GetByKey(TKey key);

    /// <summary>
    /// Returns whether an entity for a key exists or not.
    /// </summary>
    /// <param name="key">The key to look for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<bool> ExistsByKey(TKey key);

    /// <summary>
    /// Creates a new Entity.
    /// </summary>
    /// <param name="value">The entity data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Create(TEntity value);

    /// <summary>
    /// Creates new Entities.
    /// </summary>
    /// <param name="values">The entities to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateRange(IEnumerable<TEntity> values);

    /// <summary>
    /// Updates the provided entity in the database.
    /// </summary>
    /// <param name="value">The updated entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Update(TEntity value);

    /// <summary>
    /// Updates the provided entities in the database.
    /// </summary>
    /// <param name="values">The updated entities.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateRange(IEnumerable<TEntity> values);

    /// <summary>
    /// Updates the provided entity in the database ignoring relations.
    /// </summary>
    /// <param name="value">The updated entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateIgnoreRelations(TEntity value);

    /// <summary>
    /// Updates the provided entities in the database ignoring relations.
    /// </summary>
    /// <param name="values">The updated entities.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateRangeIgnoreRelations(IEnumerable<TEntity> values);

    /// <summary>
    /// Removes an entity by the provided key.
    /// </summary>
    /// <param name="key">The key of the entity to be removed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteByKey(TKey key);

    /// <summary>
    /// Removes an entity by the provided key if it exists in the database.
    /// </summary>
    /// <param name="key">The key of the entity to be removed.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// Resolves to true if an entity was deleted, false otherwise.
    /// </returns>
    Task<bool> DeleteByKeyIfExists(TKey key);

    /// <summary>
    /// Removes entities by the provided keys.
    /// </summary>
    /// <param name="keys">The keys of the entities to be removed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteRangeByKey(IEnumerable<TKey> keys);
}

/// <inheritdoc />
public interface IDbRepository<TDbContext, TEntity> : IDbRepository<TDbContext, Guid, TEntity>
    where TDbContext : DbContext
    where TEntity : BaseEntity, new()
{
}
