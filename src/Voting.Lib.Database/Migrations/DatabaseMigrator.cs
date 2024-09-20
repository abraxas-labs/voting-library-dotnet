// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.Database.Migrations;

/// <summary>
/// Database migrator to migrate a database.
/// </summary>
/// <typeparam name="TContext">Type of the EF Core DbContext for which the migration should be performed.</typeparam>
public class DatabaseMigrator<TContext> : IDatabaseMigrator
    where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrator<TContext>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseMigrator{TContext}"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public DatabaseMigrator(IServiceProvider serviceProvider, ILogger<DatabaseMigrator<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Migrate()
    {
        _logger.LogInformation("Migrating database for context {DbContext}", typeof(TContext).Name);

        using var scope = _serviceProvider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<TContext>().Database.MigrateAsync().ConfigureAwait(false);
    }
}
