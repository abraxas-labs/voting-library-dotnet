// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Voting.Lib.Database.Test.EfUtils;

public class DbSetExtensionsTest : BaseDbContextTest
{
    [Fact]
    public void GetDelimitedColumnNameShouldWork()
    {
        Context.TestEntities.GetDelimitedColumnName(x => x.Name)
            .Should()
            .Be("\"Name\"");
        Context.TestEntities.GetDelimitedColumnName(x => x.Value)
            .Should()
            .Be("\"Value\"");
        Context.SchemaScopedTestEntities.GetDelimitedColumnName(x => x.Name)
            .Should()
            .Be("\"my-name\"");
        Context.SchemaScopedTestEntities.GetDelimitedColumnName(x => x.Value)
            .Should()
            .Be("\"Value\"");
    }

    [Fact]
    public void GetDelimitedSchemaAndTableNameShouldWork()
    {
        Context.TestEntities.GetDelimitedSchemaAndTableName()
            .Should()
            .Be("\"TestEntities\"");
        Context.SchemaScopedTestEntities.GetDelimitedSchemaAndTableName()
            .Should()
            .Be("\"my-table\""); // sqlite doesn't support schemas (the schema is simply not considered)
    }
}
