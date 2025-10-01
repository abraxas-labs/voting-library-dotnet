// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.SecondFactor.Models;

/// <summary>
/// The 2fa transaction.
/// </summary>
public class SecondFactorTransaction
{
    /// <summary>
    /// Gets or sets the id of this transaction.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of status polls already done.
    /// </summary>
    public int PollCount { get; set; }

    /// <summary>
    /// Gets or sets when the transaction was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the transaction was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the transaction expires.
    /// </summary>
    public DateTime ExpireAt { get; set; }

    /// <summary>
    /// Gets or sets the hash of the action id.
    /// </summary>
    public string ActionIdHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external token jwt ids associated with this transaction.
    /// </summary>
    public List<string> ExternalTokenJwtIds { get; set; } = [];
}
