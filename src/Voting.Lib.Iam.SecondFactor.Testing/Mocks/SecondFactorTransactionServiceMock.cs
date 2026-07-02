// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Common;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.SecondFactor.Exceptions;
using Voting.Lib.Iam.SecondFactor.Models;
using Voting.Lib.Iam.SecondFactor.Services;
using Voting.Lib.Iam.Services.ApiClient.Identity;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Voting.Lib.Iam.SecondFactor.Testing.Mocks;

/// <summary>
/// Mock implementation of <see cref="ISecondFactorTransactionService"/>.
/// </summary>
public class SecondFactorTransactionServiceMock : ISecondFactorTransactionService
{
    /// <summary>
    /// The default transaction id string returned by <see cref="Create(IActionId, string)"/>.
    /// The constructor pre-registers it so <c>EnsureVerified</c> succeeds unless a test overrides the setup.
    /// </summary>
    public const string VerifiedTransactionIdString = "1b697d45-3b20-4e6f-83d3-cd2022228a1f";

    /// <summary>
    /// A transaction id string that is not pre-registered for verification in this mock.
    /// </summary>
    public const string UnverifiedTransactionIdString = "4c08d5a0-1c2f-49be-a62a-ee56bf87c42d";

    /// <summary>
    /// A transaction id string that simulates changed transaction data during verification.
    /// </summary>
    public const string ChangedDataTransactionIdString = "ed6ee887-42f3-4357-8f38-cf7eddbee141";

    /// <summary>
    /// The default transaction id returned by <see cref="Create(IActionId, string)"/>.
    /// The constructor pre-registers it so <c>EnsureVerified</c> succeeds unless a test overrides the setup.
    /// </summary>
    public static readonly Guid VerifiedTransactionId = Guid.Parse(VerifiedTransactionIdString);

    /// <summary>
    /// A transaction id that is not pre-registered in the mock and therefore fails <c>EnsureVerified</c>.
    /// </summary>
    public static readonly Guid UnverifiedTransactionId = Guid.Parse(UnverifiedTransactionIdString);

    /// <summary>
    /// A transaction id that simulates changed data during <c>EnsureVerified</c>.
    /// </summary>
    public static readonly Guid ChangedDataTransactionId = Guid.Parse(ChangedDataTransactionIdString);

    /// <summary>
    /// A default mocked nevis token id.
    /// </summary>
    public static readonly string NevisTokenId = SecureConnectTestDefaults.MockedVerified2faId;

    private readonly ConcurrentDictionary<Guid, string?> _verificationExpectationsByTransactionId = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SecondFactorTransactionServiceMock"/> class.
    /// Pre-registers the default transaction returned by <see cref="Create(IActionId, string)"/> so tests can use the happy path without extra setup.
    /// </summary>
    public SecondFactorTransactionServiceMock()
    {
        MarkAsVerified(VerifiedTransactionId);
    }

    /// <summary>
    /// Pre-registers a transaction id for <c>EnsureVerified</c>.
    /// When an OTP code is provided, the same OTP must be passed to verification.
    /// </summary>
    /// <param name="transactionId">The transaction id to mark as verified.</param>
    /// <param name="otpCode">The expected OTP code.</param>
    public void MarkAsVerified(Guid transactionId, string? otpCode = null)
        => _verificationExpectationsByTransactionId[transactionId] = otpCode;

    /// <inheritdoc />
    public Task<SecondFactorTransactionInfo> Create(IActionId actionId, string message)
    {
        var actionIdHash = actionId.ComputeHash();
        var transaction = new SecondFactorTransaction
        {
            Id = VerifiedTransactionId,
            ActionIdHash = actionIdHash,
            UserId = SecureConnectTestDefaults.MockedUserDefault.Loginid,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddMinutes(10),
            NevisExternalTokenJwtIds = [SecureConnectTestDefaults.MockedVerified2faId],
        };

        return Task.FromResult(new SecondFactorTransactionInfo(
            transaction,
            "mocked correlation code",
            "mocked message",
            new HashSet<V1SecondFactorProvider> { V1SecondFactorProvider.NEVIS, V1SecondFactorProvider.OTP },
            new SecondFactorNevisInfo("mocked-qr-code", [SecureConnectTestDefaults.MockedVerified2faId])));
    }

    /// <inheritdoc />
    public Task EnsureVerified(
        Guid transactionId,
        V1SecondFactorProvider provider,
        Func<Task<IActionId>> actionIdProvider,
        string? otpCode = null,
        CancellationToken cancellationToken = default)
    {
        if (transactionId == ChangedDataTransactionId)
        {
            throw new SecondFactorTransactionDataChangedException();
        }

        if (_verificationExpectationsByTransactionId.TryGetValue(transactionId, out var expectedOtpCode)
            && (expectedOtpCode is null || expectedOtpCode == otpCode))
        {
            return Task.CompletedTask;
        }

        throw new SecondFactorTransactionNotVerifiedException();
    }
}
