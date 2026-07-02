// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

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
    /// Gets or sets the interval in which the cleanup job should run.
    /// </summary>
    public TimeSpan CleanupJobInterval { get; set; } = TimeSpan.FromMinutes(5);
}
