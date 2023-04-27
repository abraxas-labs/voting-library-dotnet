// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

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
    /// <para>Note: By default isiolation level is Serializable but all modern db's use read committed as default. Therefore using ReadCommited.</para>
    /// </summary>
    /// <returns>A new <see cref="TransactionScope"/>.</returns>
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
