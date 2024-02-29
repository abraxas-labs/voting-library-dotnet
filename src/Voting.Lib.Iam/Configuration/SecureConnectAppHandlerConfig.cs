// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Configuration;

/// <summary>
/// Configuration for the IAM app handler.
/// </summary>
public class SecureConnectAppHandlerConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecureConnectAppHandlerConfig"/> class.
    /// </summary>
    /// <param name="appHeader">The app header value.</param>
    public SecureConnectAppHandlerConfig(string appHeader)
    {
        AppHeader = appHeader;
    }

    /// <summary>
    /// Gets or sets the app header name.
    /// </summary>
    public string AppHeaderName { get; set; } = "X-APP";

    /// <summary>
    /// Gets or sets the app header value.
    /// </summary>
    public string AppHeader { get; set; } = string.Empty;
}
