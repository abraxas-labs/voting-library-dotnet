// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Database.Test.Repositories;
using Voting.Lib.Database.Test.TestModels;

namespace Voting.Lib.Database.Test;

public abstract class BaseDbContextTest<TContext>
    where TContext : TestDbContext
{
    protected BaseDbContextTest()
    {
        var sp = new ServiceCollection()
            .AddDbContext<TContext>()
            .AddVotingLibDatabase<TContext>()
            .AddTransient(typeof(ExposingTestDbRepository<,>))
            .AddTransient(typeof(OverridingTestDbRepository<>))
            .BuildServiceProvider();
        Context = sp.GetRequiredService<TContext>();
        Repo = sp.GetRequiredService<ExposingTestDbRepository<TContext, TestEntity>>();
        RepoOverride = sp.GetRequiredService<OverridingTestDbRepository<TContext>>();

        LoadInitialData();
    }

    protected TContext Context { get; }

    protected ExposingTestDbRepository<TContext, TestEntity> Repo { get; }

    protected OverridingTestDbRepository<TContext> RepoOverride { get; }

    private void LoadInitialData()
    {
        Context.Database.EnsureCreated();
        Context.TestEntities.AddRange(
            Enumerable.Range(0, 20)
                .Select(x => new TestEntity { Value = x, Name = "X" + x }));
        Context.SaveChanges();
    }
}

public abstract class BaseDbContextTest : BaseDbContextTest<TestDbContext>
{
}
