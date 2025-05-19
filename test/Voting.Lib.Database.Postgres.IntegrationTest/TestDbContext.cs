// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Postgres.Locking;

namespace Voting.Lib.Database.Postgres.IntegrationTest;

public class TestDbContext(string connectionString) : DbContext
{
    public DbSet<TestObject> Objects { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(connectionString).AddLockInterceptors();
}
