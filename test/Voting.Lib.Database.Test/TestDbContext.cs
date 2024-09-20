// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Database.Test.TestModels;

namespace Voting.Lib.Database.Test;

public class TestDbContext : DbContext
{
    private readonly SqliteConnection _connection;

    public TestDbContext()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }

    public DbSet<TestEntity> TestEntities { get; set; } = null!;

    public DbSet<SchemaScopedTestEntity> SchemaScopedTestEntities { get; set; } = null!;

    public override async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<SchemaScopedTestEntity>()
            .ToTable("my-table", "myschema")
            .Property(x => x.Name)
            .HasColumnName("my-name");
    }
}
