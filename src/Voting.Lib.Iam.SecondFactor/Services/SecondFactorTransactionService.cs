// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.SecondFactor.Configuration;
using Voting.Lib.Iam.SecondFactor.Exceptions;
using Voting.Lib.Iam.SecondFactor.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Services.ApiClient.Identity;
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
        IActionId actionId,
        string message)
    {
        var actionIdHash = actionId.ComputeHash();

        var now = _timeProvider.GetUtcNowDateTime();
        var expireAt = now.Add(_config.TransactionExpiration);

        // only nevis and otp supported
        var availableProviders = await _userService.GetReadySecondFactorProviders(_auth.User.Loginid);
        availableProviders = availableProviders.Where(p => p is V1SecondFactorProvider.OTP or V1SecondFactorProvider.NEVIS).ToHashSet();

        // a code displayed to the user to correlate the second factor request
        var correlationCode = BuildCorrelationCode();
        message = $"({correlationCode}) {message}";

        // Only initiate NEVIS if it is among the available providers.
        // If only OTP is available, we skip the NEVIS request and the user verifies via OTP.
        SecondFactorNevisInfo? nevisInfo = null;
        if (availableProviders.Contains(V1SecondFactorProvider.NEVIS))
        {
            nevisInfo = await RequestNevis(message, _auth.User.Loginid).ConfigureAwait(false);
        }

        var transaction = new SecondFactorTransaction
        {
            Id = Guid.NewGuid(),
            UserId = _auth.User.Loginid,
            ActionIdHash = actionIdHash,
            CreatedAt = now,
            LastUpdatedAt = now,
            ExpireAt = expireAt,
            NevisExternalTokenJwtIds = nevisInfo?.TokenJwtIds.ToList(),
        };
        await _repo.Create(transaction);
        _logger.LogInformation(
            SecurityLogging.SecurityEventId,
            "Created second factor transaction with Providers {Providers}, ExternalTokenJwtIds <{SecondFactorExternalTokenJwtIds}> for action {ActionId}",
            string.Join(',', availableProviders.Select(p => p.ToString())),
            string.Join(',', transaction.NevisExternalTokenJwtIds ?? []),
            actionId);
        return new SecondFactorTransactionInfo(
            transaction,
            correlationCode,
            message,
            availableProviders,
            nevisInfo);
    }

    /// <summary>
    /// Ensures that the second factor transaction is verified (or polls for its result)
    /// and that the action id has not changed during the verification.
    /// Ensure that the target action is not executed before calling this method.
    /// Ensure that the target data is not modified while calling this method.
    /// </summary>
    /// <param name="transactionId">The transaction id.</param>
    /// <param name="provider">The provider to use.</param>
    /// <param name="actionIdProvider">The action provider.</param>
    /// <param name="otpCode">The OTP code (if OTP provider).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task EnsureVerified(
        Guid transactionId,
        V1SecondFactorProvider provider,
        Func<Task<IActionId>> actionIdProvider,
        string? otpCode = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureAwaitVerification(transactionId, provider, otpCode, cancellationToken);

        // The action id must be fetched after the blocking verify request, to check for data changes in the aggregate during the request.
        var actionId = await actionIdProvider();
        await EnsureDataHasNotChanged(transactionId, actionId);
        _logger.LogInformation(
            SecurityLogging.SecurityEventId,
            "Second factor transaction {SecondFactorExternalId} for action {ActionId} verified",
            transactionId,
            actionId);
    }

    private async Task EnsureAwaitVerification(
        Guid transactionId,
        V1SecondFactorProvider provider,
        string? otpCode,
        CancellationToken ct)
    {
        var secondFactorTransaction = await _repo.GetById(transactionId);
        secondFactorTransaction.PollCount++;
        secondFactorTransaction.LastUpdatedAt = _timeProvider.GetUtcNowDateTime();
        secondFactorTransaction.LastAttemptedProvider = provider;
        await _repo.Update(secondFactorTransaction);

        var isVerified = await _userService.VerifySecondFactor(
            _auth.User.Loginid,
            provider,
            code: otpCode,
            nevisTokenJwtIds: secondFactorTransaction.NevisExternalTokenJwtIds,
            ct: ct);
        if (!isVerified)
        {
            _logger.LogWarning(
                SecurityLogging.SecurityEventId,
                "Second factor transaction {SecondFactorExternalId} failed",
                transactionId);
            throw new SecondFactorTransactionNotVerifiedException();
        }
    }

    private async Task EnsureDataHasNotChanged(Guid transactionId, IActionId actionId)
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

    private async Task<SecondFactorNevisInfo?> RequestNevis(
        string message,
        string loginId)
    {
        var secondFactor = await _userService.RequestSecondFactor(
            loginId,
            V1SecondFactorProvider.NEVIS,
            message);
        return secondFactor.Nevis;
    }

    private string BuildCorrelationCode()
        => RandomNumberGenerator.GetString(CorrelationCodeAlphabet, _config.CorrelationCodeLength);
}
