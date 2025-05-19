// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Voting.Lib.Database.Postgres.IntegrationTest;

public class PostgresFixture : IAsyncLifetime
{
    private PostgreSqlContainer _sqlContainer = new PostgreSqlBuilder().Build();

    public TestDbContext NewContext() => new(_sqlContainer.GetConnectionString());

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();

        await using var ctx = NewContext();
        await ctx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.DisposeAsync();
    }
}
