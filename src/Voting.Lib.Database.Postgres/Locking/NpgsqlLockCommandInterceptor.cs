// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Voting.Lib.Database.Postgres.Locking;

/// <summary>
/// Interceptor to add lock statements to read commands.
/// </summary>
public class NpgsqlLockCommandInterceptor : DbCommandInterceptor
{
    /// <summary>
    /// The tag to mark queries as for update skip locked.
    /// </summary>
    internal const string LockForUpdateSkipLockedAnnotation = "<vo-lib-lock-for-update-skip-locked />";

    /// <summary>
    /// The tag to mark queries as for update.
    /// </summary>
    internal const string LockForUpdateAnnotation = "<vo-lib-lock-for-update />";

    /// <inheritdoc />
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        AdjustCommand(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        AdjustCommand(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static void AdjustCommand(DbCommand command)
    {
        if (command.CommandText.Contains(LockForUpdateSkipLockedAnnotation))
        {
            command.CommandText += " FOR UPDATE SKIP LOCKED";
            return;
        }

        if (command.CommandText.Contains(LockForUpdateAnnotation))
        {
            command.CommandText += " FOR UPDATE";
            return;
        }
    }
}
