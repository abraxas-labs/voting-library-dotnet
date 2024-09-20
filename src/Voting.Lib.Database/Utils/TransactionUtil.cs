// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Transactions;

namespace Voting.Lib.Database.Utils;

/// <summary>
/// Util for db transactions.
/// </summary>
public static class TransactionUtil
{
    /// <summary>
    /// <para>Gets a new transaction scope, which could be used within a using block statement to commit multiple save operations at once.</para>
    /// <para>Therefore start your using block i.e. with:</para>
    /// <c>using var transaction = TransactionUtil.NewTransactionScope();</c>.
    /// <para>perform your changes on the repos and end it with:</para>
    /// <c>transaction.Complete();</c>.
    /// <para>Note: By default isolation level is Serializable but all modern db's use read committed as default. Therefore using ReadCommited.</para>
    /// </summary>
    /// <remarks>
    /// Use DbContext.Database.BeginTransactionAsync() instead, since the transaction scope brings lots of pitfalls.
    /// Eg. if a database connection is opened before the transaction scope is opened, the connection may not be enlisted in the transaction
    /// (see https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/distributed-transactions?redirectedfrom=MSDN#automatically-enlisting-in-a-distributed-transaction).
    /// </remarks>
    /// <returns>A new <see cref="TransactionScope"/>.</returns>
    [Obsolete("Use DbContext.Database.BeginTransactionAsync instead")]
    public static TransactionScope NewTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}
