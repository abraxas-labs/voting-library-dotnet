// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.SecondFactor.Models;

namespace Voting.Lib.Iam.SecondFactor.Services;

/// <summary>
/// Service to handle second factor transactions.
/// </summary>
public interface ISecondFactorTransactionService
{
    /// <summary>
    /// Creates a new 2fa transaction.
    /// </summary>
    /// <param name="actionId">The action id which uniquely describes the verifiable action.</param>
    /// <param name="message">The message displayed to the user.</param>
    /// <returns>The info about the transaction.</returns>
    Task<SecondFactorTransactionInfo> Create(
        ISecondFactorTransactionActionId actionId,
        string message);

    /// <summary>
    /// Ensures that the second factor transaction is verified (or polls for its result)
    /// and that the action id has not changed during the verification.
    /// </summary>
    /// <param name="transactionId">The transaction id.</param>
    /// <param name="actionProvider">The action provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnsureVerified(
        Guid transactionId,
        Func<Task<ISecondFactorTransactionActionId>> actionProvider,
        CancellationToken cancellationToken);
}
