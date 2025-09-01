// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Iam.TokenHandling.OnBehalfToken;

/// <summary>
/// The options to resolve the ob_token.
/// </summary>
public class SecureConnectOnBehalfOptions
{
    /// <summary>
    /// Gets the string added to the name of the http client used to resolve the ob_token.
    /// </summary>
    internal const string TokenClientSuffix = "-ObTokenClient";

    /// <summary>
    /// Gets or sets the resource to resolve in the ob_token.
    /// </summary>
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timeframe when a token is considered invalid before the real expiry for the token.
    /// This needs to be smaller than the actual lifetime of the tokens issued by SecureConnect.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromSeconds(10);

    internal void ApplyFrom(SecureConnectOnBehalfOptions options)
    {
        Resource = options.Resource;
    }
}
