// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;

namespace Voting.Lib.Database.Migrations;

/// <summary>
/// Interface for migrating a database.
/// </summary>
public interface IDatabaseMigrator
{
    /// <summary>
    /// Migrate a database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Migrate();
}
