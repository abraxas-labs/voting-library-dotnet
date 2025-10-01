// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Iam.SecondFactor.Configuration;
using Voting.Lib.Iam.SecondFactor.Exceptions;
using Voting.Lib.Iam.SecondFactor.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Store;

namespace Voting.Lib.Iam.SecondFactor.Services;

/// <summary>
/// A service to create, poll and verify second factor transactions.
/// </summary>
public class SecondFactorTransactionService : ISecondFactorTransactionService
{
    private static readonly char[] CorrelationCodeAlphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

    private readonly IAuth _auth;
    private readonly IUserService _userService;
    private readonly SecondFactorTransactionConfig _config;
    private readonly ILogger<SecondFactorTransactionService> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly ISecondFactorTransactionRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecondFactorTransactionService"/> class.
    /// </summary>
    /// <param name="auth">The auth provider.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="repo">The storage repository.</param>
    /// <param name="timeProvider">The time provider.</param>
    public SecondFactorTransactionService(
        IAuth auth,
        IUserService userService,
        SecondFactorTransactionConfig config,
        ILogger<SecondFactorTransactionService> logger,
        ISecondFactorTransactionRepository repo,
        TimeProvider timeProvider)
    {
        _auth = auth;
        _userService = userService;
        _config = config;
        _logger = logger;
        _repo = repo;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Creates a new 2fa transaction.
    /// </summary>
    /// <param name="actionId">The action id which uniquely describes the verifiable action.</param>
    /// <param name="message">The message displayed to the user.</param>
    /// <returns>The info about the transaction.</returns>
    public async Task<SecondFactorTransactionInfo> Create(
        ISecondFactorTransactionActionId actionId,
        string message)
    {
        var actionIdHash = actionId.ComputeHash();

        // a code displayed to the user to correlate the second factor request
        var correlationCode = BuildCorrelationCode();
        var expireAt = _timeProvider.GetUtcNowDateTime().Add(_config.TransactionExpiration);
        var secondFactor = await _userService.RequestSecondFactor(
            _auth.User.Loginid,
            _config.Provider.ToString(),
            $"({correlationCode}) {message}");
        var transaction = new SecondFactorTransaction
        {
            Id = Guid.NewGuid(),
            UserId = _auth.User.Loginid,
            ActionIdHash = actionIdHash,
            LastUpdatedAt = _timeProvider.GetUtcNowDateTime(),
            ExpireAt = expireAt,
            ExternalTokenJwtIds = secondFactor.TokenJwtIds.ToList(),
        };
        await _repo.Create(transaction);
        _logger.LogInformation(
            SecurityLogging.SecurityEventId,
            "Created second factor transaction with ExternalTokenJwtIds <{SecondFactorExternalTokenJwtIds}> for action {ActionId}",
            string.Join(',', transaction.ExternalTokenJwtIds),
            actionId);
        return new SecondFactorTransactionInfo(transaction, correlationCode, message, secondFactor.Qr);
    }

    /// <summary>
    /// Ensures that the second factor transaction is verified (or polls for its result)
    /// and that the action id has not changed during the verification.
    /// Ensure that the target action is not executed before calling this method.
    /// Ensure that the target data is not modified while calling this method.
    /// </summary>
    /// <param name="transactionId">The transaction id.</param>
    /// <param name="actionProvider">The action provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task EnsureVerified(
        Guid transactionId,
        Func<Task<ISecondFactorTransactionActionId>> actionProvider,
        CancellationToken cancellationToken)
    {
        await EnsureAwaitVerification(transactionId, cancellationToken);

        // The action id must be fetched after the blocking verify request, to check for data changes in the aggregate during the request.
        var actionId = await actionProvider();
        await EnsureDataHasNotChanged(transactionId, actionId);
        _logger.LogInformation(
            SecurityLogging.SecurityEventId,
            "Second factor transaction {SecondFactorExternalId} for action {ActionId} verified",
            transactionId,
            actionId);
    }

    private async Task EnsureAwaitVerification(Guid transactionId, CancellationToken ct)
    {
        var secondFactorTransaction = await _repo.GetById(transactionId);
        secondFactorTransaction.PollCount++;
        secondFactorTransaction.LastUpdatedAt = _timeProvider.GetUtcNowDateTime();
        await _repo.Update(secondFactorTransaction);

        var isVerified = await _userService.VerifySecondFactor(
            _auth.User.Loginid,
            _config.Provider,
            secondFactorTransaction.ExternalTokenJwtIds,
            ct);
        if (!isVerified)
        {
            _logger.LogWarning(
                SecurityLogging.SecurityEventId,
                "Second factor transaction {SecondFactorExternalId} failed",
                transactionId);
            throw new SecondFactorTransactionNotVerifiedException();
        }
    }

    private async Task EnsureDataHasNotChanged(Guid transactionId, ISecondFactorTransactionActionId actionId)
    {
        var secondFactorTransaction = await _repo.GetById(transactionId);
        var actionIdHash = actionId.ComputeHash();
        if (!actionIdHash.Equals(secondFactorTransaction.ActionIdHash, StringComparison.CurrentCulture))
        {
            _logger.LogWarning(
                SecurityLogging.SecurityEventId,
                "Data changed during second factor transaction {SecondFactorTransactionId} for action {ActionId}",
                transactionId,
                actionId);
            throw new SecondFactorTransactionDataChangedException();
        }
    }

    private string BuildCorrelationCode()
        => RandomNumberGenerator.GetString(CorrelationCodeAlphabet, _config.CorrelationCodeLength);
}
