// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Test.TestModels;
using Xunit;

namespace Voting.Lib.Database.Test.Repositories;

public class DbRepositoryTest : BaseDbContextTest
{
    [Fact]
    public void TableNameShouldReturnCorrectName()
    {
        Repo.ProtectedTableName.Should().Be("TestEntities");
    }

    [Fact]
    public void DelimitedSchemaAndTableNameShouldReturnCorrectName()
    {
        Repo.ProtectedDelimitedSchemaAndTableName.Should().Be("\"TestEntities\"");
    }

    [Fact]
    public void GetDelimitedColumnNameShouldReturnCorrectName()
    {
        Repo.GetProtectedDelimitedColumnName(x => x.Name).Should().Be("\"Name\"");
    }

    [Fact]
    public void QueryShouldReturnIQueryable()
    {
        Repo.Query()
            .ToQueryString()
            .Replace("\r", string.Empty)
            .Should()
            .Be("SELECT \"t\".\"Id\", \"t\".\"Name\", \"t\".\"Value\"\nFROM \"TestEntities\" AS \"t\"");
    }

    [Fact]
    public async Task GetByKeyShouldReturnObject()
    {
        var obj = await Repo.Query().FirstAsync(x => x.Name == "X1");
        var resp = await Repo.GetByKey(obj.Id);
        resp.Should().NotBeNull();
        resp!.Name.Should().Be("X1");
    }

    [Fact]
    public async Task GetByKeyShouldReturnNullIfNotFound()
    {
        var resp = await Repo.GetByKey(Guid.Parse("43a3384e-c1a4-448a-9cb9-3dd0df1cefca"));
        resp.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByKeyShouldReturnTrueIfExists()
    {
        var obj = await Repo.Query().FirstAsync(x => x.Name == "X1");
        var resp = await Repo.ExistsByKey(obj.Id);
        resp.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByKeyShouldReturnFalseIfNotExists()
    {
        var resp = await Repo.ExistsByKey(Guid.Parse("43a3384e-c1a4-448a-9cb9-3dd0df1cefca"));
        resp.Should().BeFalse();
    }

    [Fact]
    public async Task CreateShouldReturnOk()
    {
        var entity = new TestEntity { Name = "Y1" };
        await Repo.Create(entity);
        var entity2 = await Repo.GetByKey(entity.Id);
        entity2.Should().NotBeNull();
        entity2!.Name.Should().Be("Y1");
    }

    [Fact]
    public async Task CreateRangeShouldReturnOk()
    {
        var newEntities = new[]
        {
                new TestEntity { Name = "Y1" },
                new TestEntity { Name = "Y2" },
                new TestEntity { Name = "Y3" },
        };

        var count = await Repo.Query().CountAsync();
        await Repo.CreateRange(newEntities);

        var updatedCount = await Repo.Query().CountAsync();
        updatedCount.Should().Be(count + newEntities.Length);

        var entity2 = await Repo.GetByKey(newEntities[0].Id);
        entity2.Should().NotBeNull();
        entity2!.Name.Should().Be("Y1");
    }

    [Fact]
    public async Task UpdateShouldReturnOk()
    {
        var entity = await Repo.Query().FirstAsync(x => x.Name == "X1");
        entity.Name = "Y2";
        await Repo.Update(entity);

        var entity2 = await Repo.GetByKey(entity.Id);
        entity2.Should().NotBeNull();
        entity2!.Name.Should().Be("Y2");
    }

    [Fact]
    public async Task UpdateShouldThrowIfUnknown()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await Repo.Update(new TestEntity
        {
            Id = Guid.Parse("884311ec-522f-44d1-95ef-5a2d358815c2"),
            Name = "Y3",
        }));
    }

    [Fact]
    public async Task UpdateRangeShouldReturnOk()
    {
        var entities = await Repo.Query().Take(5).ToListAsync();
        foreach (var entity in entities)
        {
            entity.Value += 100;
            entity.Name += "-updated";
        }

        await Repo.UpdateRange(entities);

        var ids = entities.Select(x => x.Id).ToList();
        var updatedEntities = await Repo.Query().Where(x => ids.Contains(x.Id)).ToListAsync();
        updatedEntities.All(x => x.Name.EndsWith("-updated", StringComparison.Ordinal) && x.Value >= 100)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task UpdateRangeShouldThrowIfUnknown()
    {
        var entity = await Repo.Query().FirstAsync();
        entity.Value += 100;
        entity.Name += "-updated";

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await Repo.UpdateRange(new[]
        {
                entity,
                new TestEntity
                {
                    Id = Guid.Parse("884311ec-522f-44d1-95ef-5a2d358815c2"),
                    Name = "not-found",
                    Value = 101,
                },
        }));
    }

    [Fact]
    public async Task UpdateIgnoreRelationsShouldReturnOk()
    {
        var entity = await Repo.Query().FirstAsync(x => x.Name == "X1");
        entity.Name = "Y2";
        await Repo.UpdateIgnoreRelations(entity);

        var entity2 = await Repo.GetByKey(entity.Id);
        entity2.Should().NotBeNull();
        entity2!.Name.Should().Be("Y2");
    }

    [Fact]
    public async Task UpdateIgnoreRelationsShouldThrowIfUnknown()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await Repo.UpdateIgnoreRelations(new TestEntity
        {
            Id = Guid.Parse("884311ec-522f-44d1-95ef-5a2d358815c2"),
            Name = "Y3",
        }));
    }

    [Fact]
    public async Task UpdateRangeIgnoreRelationsShouldReturnOk()
    {
        var entities = await Repo.Query().Take(5).ToListAsync();
        foreach (var entity in entities)
        {
            entity.Value += 100;
            entity.Name += "-updated";
        }

        await Repo.UpdateRangeIgnoreRelations(entities);

        var ids = entities.Select(x => x.Id).ToList();
        var updatedEntities = await Repo.Query().Where(x => ids.Contains(x.Id)).ToListAsync();
        updatedEntities.All(x => x.Name.EndsWith("-updated", StringComparison.Ordinal) && x.Value >= 100)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task UpdateRangeIgnoreRelationsShouldThrowIfUnknown()
    {
        var entity = await Repo.Query().FirstAsync();
        entity.Value += 100;
        entity.Name += "-updated";

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await Repo.UpdateRangeIgnoreRelations(new[]
        {
                entity,
                new TestEntity
                {
                    Id = Guid.Parse("884311ec-522f-44d1-95ef-5a2d358815c2"),
                    Name = "not-found",
                    Value = 101,
                },
        }));
    }

    [Fact]
    public async Task DeleteByKeyShouldReturnOk()
    {
        var entity = await Repo.Query().FirstAsync(x => x.Name == "X1");
        await Repo.DeleteByKey(entity.Id);

        var exists = await Repo.ExistsByKey(entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteByKeyIfExistsShouldReturnTrue()
    {
        var entity = await Repo.Query().FirstAsync(x => x.Name == "X1");
        var resp = await Repo.DeleteByKeyIfExists(entity.Id);
        resp.Should().BeTrue();

        var exists = await Repo.ExistsByKey(entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteByKeyIfExistsShouldReturnFalseIfNotExisting()
    {
        var resp = await Repo.DeleteByKeyIfExists(Guid.Empty);
        resp.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteRangeByKeyShouldThrowIfUnknown()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            await Repo.DeleteByKey(Guid.Parse("e49f5c72-d2b2-45a7-b867-ea0dd0c61ab2")));
    }

    [Fact]
    public async Task DeleteRangeByKeyShouldReturnOk()
    {
        var count = await Repo.Query().CountAsync();
        var entityIds = await Repo.Query()
            .Take(5)
            .Select(x => x.Id)
            .ToListAsync();
        await Repo.DeleteRangeByKey(entityIds);

        var countAfterDelete = await Repo.Query().CountAsync();
        countAfterDelete.Should().Be(count - 5);
    }

    [Fact]
    public async Task DeleteRangeByKeyWithUnknownKeyShouldDeleteNothingAndThrow()
    {
        var count = await Repo.Query().CountAsync();
        var entityIds = await Repo.Query()
            .Take(5)
            .Select(x => x.Id)
            .ToListAsync();

        // not found id
        entityIds.Add(Guid.Parse("8952f50c-a3c1-4920-a88c-ddcc45201440"));

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            await Repo.DeleteRangeByKey(entityIds));

        var newCount = await Repo.Query().CountAsync();
        newCount.Should().Be(count);
    }

    [Fact]
    public async Task DeleteRangeByKeyShouldReturnOkIfEmpty()
    {
        var count = await Repo.Query().CountAsync();
        await Repo.DeleteRangeByKey(Array.Empty<Guid>());

        var newCount = await Repo.Query().CountAsync();
        newCount.Should().Be(count);
    }
}
