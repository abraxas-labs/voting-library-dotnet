// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Cryptography.Kms.Configuration;

/// <summary>
/// KMS configuration.
/// </summary>
public class KmsConfig
{
    /// <summary>
    /// Gets or sets the aes key size.
    /// </summary>
    public int AesKeySize { get; set; } = 256;

    /// <summary>
    /// Gets or sets the KMS base uri endpoint.
    /// Example: https://pre.kmp.abraxas-apis.ch/api.
    /// </summary>
    public Uri? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lead time for token refresh.
    /// E.g., If this is set to 10 seconds, the token gets refreshed 10 seconds before expiration.
    /// </summary>
    public TimeSpan TokenExpiryLeadTime { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Gets or sets the labels which are set on created keys.
    /// </summary>
    public Dictionary<string, string> AesKeyLabels { get; set; } = [];

    /// <summary>
    /// Gets or sets the labels which are set on created keys.
    /// </summary>
    public Dictionary<string, string> MacKeyLabels { get; set; } = [];
}
