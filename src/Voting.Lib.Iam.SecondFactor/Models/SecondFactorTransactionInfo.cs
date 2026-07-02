// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Voting.Lib.Iam.SecondFactor.Models;

/// <summary>
/// Information about the created 2fa transaction.
/// </summary>
/// <param name="Transaction">The created transaction.</param>
/// <param name="CorrelationCode">The correlation code.</param>
/// <param name="Message">The message.</param>
/// <param name="AvailableProviders">The available second factor providers.</param>
/// <param name="Nevis">The info for nevis.</param>
public record SecondFactorTransactionInfo(
    SecondFactorTransaction Transaction,
    string CorrelationCode,
    string Message,
    IReadOnlySet<V1SecondFactorProvider> AvailableProviders,
    SecondFactorNevisInfo? Nevis);
