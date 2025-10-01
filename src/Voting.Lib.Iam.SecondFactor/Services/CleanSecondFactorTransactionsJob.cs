// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Scheduler;

namespace Voting.Lib.Iam.SecondFactor.Services;

/// <summary>
/// A job that cleans expired second factor transactions.
/// </summary>
public class CleanSecondFactorTransactionsJob : IScheduledJob
{
    private readonly ISecondFactorTransactionRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="CleanSecondFactorTransactionsJob"/> class.
    /// </summary>
    /// <param name="repo">The repo.</param>
    public CleanSecondFactorTransactionsJob(ISecondFactorTransactionRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public Task Run(CancellationToken ct) => _repo.DeleteExpired();
}
