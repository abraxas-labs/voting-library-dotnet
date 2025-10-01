// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Voting.Lib.Iam.SecondFactor.Configuration;

/// <summary>
/// The config for the 2fa transaction provider.
/// </summary>
public class SecondFactorTransactionConfig
{
    /// <summary>
    /// Gets or sets the timespan after which a second factor transaction expires.
    /// </summary>
    public TimeSpan TransactionExpiration { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the length of the correlation code.
    /// </summary>
    public int CorrelationCodeLength { get; set; } = 4;

    /// <summary>
    /// Gets or sets the name of the 2FA provider to use.
    /// </summary>
    public V1SecondFactorProvider Provider { get; set; } = V1SecondFactorProvider.NEVIS;

    /// <summary>
    /// Gets or sets the interval in which the cleanup job should run.
    /// </summary>
    public TimeSpan CleanupJobInterval { get; set; } = TimeSpan.FromMinutes(5);
}
