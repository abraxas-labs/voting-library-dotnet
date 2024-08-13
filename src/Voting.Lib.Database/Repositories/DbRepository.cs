// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Voting.Lib.Database.Models;

namespace Voting.Lib.Database.Repositories;

/// <inheritdoc />
public class DbRepository<TDbContext, TKey, TEntity> : IDbRepository<TDbContext, TKey, TEntity>
    where TDbContext : DbContext
    where TKey : struct
    where TEntity : class, IBaseEntity<TKey>, new()
{
    private readonly StoreObjectIdentifier _tableStoreObjectIdentifier;

    private IEntityType? _entityType;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbRepository{TDbContext,TKey,TEntity}"/> class.
    /// </summary>
    /// <param name="context">The EF Core DbContext.</param>
    public DbRepository(TDbContext context)
    {
        Context = context;
        SqlGenerationHelper = context.GetService<ISqlGenerationHelper>();

        _tableStoreObjectIdentifier = StoreObjectIdentifier.Create(EntityType, StoreObjectType.Table)
                                      ?? throw new Exception("could not create a store object identifier.");
    }

    /// <summary>
    /// Gets the SqlGenerationHelper.
    /// </summary>
    protected ISqlGenerationHelper SqlGenerationHelper { get; }

    /// <summary>
    /// Gets the EF Core DbContext.
    /// </summary>
    protected TDbContext Context { get; }

    /// <summary>
    /// Gets the DbSet of the Entity of the current DbContext.
    /// </summary>
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    /// <summary>
    /// Gets the <see cref="EntityType"/> from the ef core model of the current entity.
    /// </summary>
    protected IEntityType EntityType => _entityType ??= Context.Model.FindEntityType(typeof(TEntity))
        ?? throw new Exception("could not find entity type for " + typeof(TEntity).FullName);

    /// <summary>
    /// Gets the Name of the schema of the current entity, or null if none is set.
    /// </summary>
    protected string? Schema => EntityType.GetSchema();

    /// <summary>
    /// Gets the relational database mapped table name of the current entity.
    /// </summary>
    protected string TableName => EntityType.GetTableName()
        ?? throw new InvalidOperationException("could not get table name from entity type.");

    /// <summary>
    /// Gets the delimited schema and table name of the current entity.
    /// </summary>
    protected string DelimitedSchemaAndTableName => SqlGenerationHelper.DelimitIdentifier(TableName, Schema);

    /// <summary>
    /// A new <see cref="IQueryable{TEntity}">Queryable</see> for the current Set.
    /// Always sets no tracking.
    /// </summary>
    /// <returns>An Instance of <see cref="IQueryable{TEntity}">Queryable</see>.</returns>
    public virtual IQueryable<TEntity> Query()
        => Set.AsNoTracking();

    /// <inheritdoc />
    public virtual Task<TEntity?> GetByKey(TKey key)
        => Query().FirstOrDefaultAsync(x => x.Id.Equals(key));

    /// <inheritdoc />
    public virtual Task<bool> ExistsByKey(TKey key)
        => Set.AnyAsync(x => key.Equals(x.Id));

    /// <inheritdoc />
    public virtual async Task Create(TEntity value)
    {
        Context.Add(value);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task CreateRange(IEnumerable<TEntity> values)
    {
        Context.AddRange(values);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// If a tracked instance with the key of the value exists
    /// it is detached before updating the values.
    /// </summary>
    public virtual async Task Update(TEntity value)
    {
        if (IsTracked(value.Id, out var entity))
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        Set.Update(value);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// If a tracked instance with the key of the value exists
    /// it is detached before updating the values.
    /// </summary>
    public virtual async Task UpdateRange(IEnumerable<TEntity> values)
    {
        foreach (var value in values)
        {
            if (IsTracked(value.Id, out var entity))
            {
                Context.Entry(entity).State = EntityState.Detached;
            }
        }

        Set.UpdateRange(values);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// Sets only direct values, ignores all navigation properties.
    /// If a tracked instance with the key of the value exists
    /// it is detached before updating the values.
    /// </summary>
    public virtual async Task UpdateIgnoreRelations(TEntity value)
    {
        if (IsTracked(value.Id, out var entity))
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        var entry = Context.Attach(new TEntity { Id = value.Id });
        entry.CurrentValues.SetValues(value);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <summary>
    /// Sets only direct values, ignores all navigation properties.
    /// If a tracked instance with the key of the value exists
    /// it is detached before updating the values.
    /// </summary>
    public virtual async Task UpdateRangeIgnoreRelations(IEnumerable<TEntity> values)
    {
        foreach (var value in values)
        {
            if (IsTracked(value.Id, out var entity))
            {
                Context.Entry(entity).State = EntityState.Detached;
            }

            var entry = Context.Attach(new TEntity { Id = value.Id });
            entry.CurrentValues.SetValues(value);
        }

        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task DeleteByKey(TKey key)
    {
        var existed = await DeleteByKeyIfExists(key).ConfigureAwait(false);
        if (!existed)
        {
            // Mimick the behavior of the old implementation, which threw in this case
            throw new DbUpdateConcurrencyException($"Entity with id {key} does not exist");
        }
    }

    /// <inheritdoc />
    public virtual async Task<bool> DeleteByKeyIfExists(TKey key)
    {
        if (IsTracked(key, out var entity))
        {
            // To stay backwards compatible, we still detach entities from the local tracked state
            // since this method did this previously.
            Context.Entry(entity).State = EntityState.Detached;
        }

        var rowsAffected = await Set
            .Where(x => key.Equals(x.Id))
            .ExecuteDeleteAsync()
            .ConfigureAwait(false);
        return rowsAffected > 0;
    }

    /// <inheritdoc />
    public virtual async Task DeleteRangeByKey(IEnumerable<TKey> keys)
    {
        // Use a transaction if none exists to ensure we can roll back the changes if the deletion fails
        var transaction = Context.Database.CurrentTransaction == null
            ? await Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false)
            : null;

        var keyList = keys.ToList();
        var affectedRows = await Set
            .Where(x => keyList.Contains(x.Id))
            .ExecuteDeleteAsync()
            .ConfigureAwait(false);

        if (affectedRows != keyList.Count)
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
            }

            // Mimick the behavior of the old implementation, which threw in this case
            throw new DbUpdateConcurrencyException($"Expected operation to affect {keyList.Count} rows, but it affected {affectedRows}");
        }

        foreach (var key in keyList)
        {
            // To stay backwards compatible, we still detach entities from the local tracked state
            // since this method did this previously.
            if (IsTracked(key, out var entity))
            {
                Context.Entry(entity).State = EntityState.Detached;
            }
        }

        if (transaction != null)
        {
            await transaction.CommitAsync().ConfigureAwait(false);
            await transaction.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Checks whether an entity is tracked locally and sets the out param accordingly.
    /// </summary>
    /// <param name="key">The key, for which entity should be looked by. Evaluated with FirstOrDefault.</param>
    /// <param name="trackedEntity">Sets this value to the local tracked instance if available, null otherwise.</param>
    /// <returns>True if the entity is tracked locally, else false.</returns>
    protected bool IsTracked(TKey key, [NotNullWhen(true)] out TEntity? trackedEntity)
        => IsTracked(x => key.Equals(x.Id), out trackedEntity);

    /// <summary>
    /// Checks whether an entity is tracked locally and sets the out param accordingly.
    /// </summary>
    /// <param name="predicate">The predicate, for which entity should be looked. Evaluated with FirstOrDefault.</param>
    /// <param name="trackedEntity">Sets this value to the local tracked instance if available, null otherwise.</param>
    /// <returns>True if the entity is tracked locally, else false.</returns>
    protected bool IsTracked(Func<TEntity, bool> predicate, [NotNullWhen(true)] out TEntity? trackedEntity)
    {
        trackedEntity = Set.Local.FirstOrDefault(predicate);
        return trackedEntity != null;
    }

    /// <summary>
    /// Returns the delimited column name from the ef core model of the current entity.
    /// </summary>
    /// <param name="memberAccess">The property path.</param>
    /// <typeparam name="TProp">The type of the property.</typeparam>
    /// <returns>The delimited column name of the relational model.</returns>
    protected string GetDelimitedColumnName<TProp>(Expression<Func<TEntity, TProp>> memberAccess)
    {
        var name = Set.EntityType
                       .FindProperty(memberAccess.GetMemberAccess())
                       ?.GetColumnName(_tableStoreObjectIdentifier)
                   ?? throw new InvalidOperationException($"property {memberAccess} not found");
        return SqlGenerationHelper.DelimitIdentifier(name);
    }
}

/// <inheritdoc cref="DbRepository{TDbContext, Guid, TEntity}"/>
public class DbRepository<TDbContext, TEntity> :
    DbRepository<TDbContext, Guid, TEntity>, IDbRepository<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : BaseEntity, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbRepository{TDbContext, TEntity}"/> class.
    /// </summary>
    /// <param name="context">The EF Core DB context.</param>
    public DbRepository(TDbContext context)
        : base(context)
    {
    }
}
