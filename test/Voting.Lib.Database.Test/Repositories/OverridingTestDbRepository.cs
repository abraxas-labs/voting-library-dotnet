// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Repositories;
using Voting.Lib.Database.Test.TestModels;

namespace Voting.Lib.Database.Test.Repositories;

public class OverridingTestDbRepository<TContext> : DbRepository<TContext, TestEntity>
    where TContext : TestDbContext
{
    public OverridingTestDbRepository(TContext context)
        : base(context)
    {
    }

    public override IQueryable<TestEntity> Query()
    {
        return base.Query().Where(x => x.Value < 10);
    }

    public override async Task<TestEntity?> GetByKey(Guid key)
    {
        var entity = await base.GetByKey(key);
        return entity?.Value < 10 ? entity : null;
    }

    public override Task Create(TestEntity value)
    {
        value.Name = value.Name.ToLowerInvariant();
        return base.Create(value);
    }

    public override Task Update(TestEntity value)
    {
        value.Name = value.Name.ToLowerInvariant();
        return base.Update(value);
    }

    public override async Task DeleteByKey(Guid key)
    {
        var entity = await Context.TestEntities.FirstOrDefaultAsync(e => e.Id == key);
        if (entity?.Value >= 10)
        {
            throw new UnauthorizedAccessException("Unauthorized ressource access.");
        }

        await base.DeleteByKey(key);
    }
}
