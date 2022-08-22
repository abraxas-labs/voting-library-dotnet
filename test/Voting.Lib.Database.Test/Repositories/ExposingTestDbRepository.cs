// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq.Expressions;
using Voting.Lib.Database.Models;
using Voting.Lib.Database.Repositories;

namespace Voting.Lib.Database.Test.Repositories;

public class ExposingTestDbRepository<TContext, TEntity> : DbRepository<TContext, TEntity>
    where TContext : TestDbContext
    where TEntity : BaseEntity, new()
{
    public ExposingTestDbRepository(TContext context)
        : base(context)
    {
    }

    public string ProtectedTableName => TableName;

    public string ProtectedDelimitedSchemaAndTableName => DelimitedSchemaAndTableName;

    public string GetProtectedDelimitedColumnName<TProp>(Expression<Func<TEntity, TProp>> expr) => GetDelimitedColumnName(expr);
}
