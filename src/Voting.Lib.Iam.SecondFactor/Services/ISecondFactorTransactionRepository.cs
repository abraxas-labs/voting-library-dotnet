// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.SecondFactor.Models;

namespace Voting.Lib.Iam.SecondFactor.Services;

/// <summary>
/// The repository for <see cref="SecondFactorTransaction"/>.
/// </summary>
public interface ISecondFactorTransactionRepository
{
    /// <summary>
    /// Resolves a transaction by its id.
    /// </summary>
    /// <param name="transactionId">The id of the transaction.</param>
    /// <returns>The transaction.</returns>
    Task<SecondFactorTransaction> GetById(Guid transactionId);

    /// <summary>
    /// Store the created transaction in the underlying storage.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <returns>The task.</returns>
    Task Create(SecondFactorTransaction transaction);

    /// <summary>
    /// Store the created transaction in the underlying storage.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <returns>The task.</returns>
    Task Update(SecondFactorTransaction transaction);

    /// <summary>
    /// Deletes all expired transactions.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteExpired();
}
