// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Extensions for <see cref="DbSet{TEntity}"/>.
/// </summary>
public static class DbSetExtensions
{
    /// <summary>
    /// Returns the delimited schema and table name of an entity.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The delimited schema and table name.</returns>
    public static string GetDelimitedSchemaAndTableName<TEntity>(this DbSet<TEntity> set)
        where TEntity : class
    {
        var sqlGen = set.GetService<ISqlGenerationHelper>();
        var tableName = set.EntityType.GetTableName()
            ?? throw new InvalidOperationException("could not get table name from passed db set entity type.");
        return sqlGen.DelimitIdentifier(tableName, set.EntityType.GetSchema());
    }

    /// <summary>
    /// Returns the delimited column name of a property of an entity.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="propSelector">The member, of which the column name should be returned.</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TProp">The type of the property.</typeparam>
    /// <returns>The delimited column name.</returns>
    public static string GetDelimitedColumnName<TEntity, TProp>(this DbSet<TEntity> set, Expression<Func<TEntity, TProp>> propSelector)
        where TEntity : class
    {
        var sqlGen = set.GetService<ISqlGenerationHelper>();
        var storeObjId = StoreObjectIdentifier.Create(set.EntityType, StoreObjectType.Table)
                         ?? throw new Exception("could not create a store object identifier.");
        var memberAccess = propSelector.GetMemberAccess()
                           ?? throw new InvalidOperationException($"property {propSelector} not found");
        var name = set.EntityType
                       .FindProperty(memberAccess)
                       ?.GetColumnName(storeObjId)
                   ?? throw new InvalidOperationException($"property {memberAccess} not found");
        return sqlGen.DelimitIdentifier(name);
    }
}
