// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Models;

/// <summary>
/// Represents a user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the login ID.
    /// </summary>
    public string Loginid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name. May be null if it is a service account.
    /// </summary>
    public string? Firstname { get; set; }

    /// <summary>
    /// Gets or sets the last name. May be null if it is a service account.
    /// </summary>
    public string? Lastname { get; set; }

    /// <summary>
    /// Gets or sets the service name. May be null if it is not a service account.
    /// </summary>
    public string? Servicename { get; set; }

    /// <summary>
    /// Gets or sets the primary email or the first email if no primary is set.
    /// </summary>
    public string? PrimaryOrFirstEmail { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string Username { get; set; } = string.Empty;
}
