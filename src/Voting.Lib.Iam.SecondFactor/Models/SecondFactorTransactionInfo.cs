// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.SecondFactor.Models;

/// <summary>
/// Information about the created 2fa transaction.
/// </summary>
/// <param name="Transaction">The created transaction.</param>
/// <param name="CorrelationCode">The correlation code.</param>
/// <param name="Message">The message.</param>
/// <param name="QrCode">The QR Code as returned from the provider.</param>
public record SecondFactorTransactionInfo(
    SecondFactorTransaction Transaction,
    string CorrelationCode,
    string Message,
    string QrCode);
