// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.DocPipe.Configuration;

/// <summary>
/// DocPipe configuration.
/// </summary>
public class DocPipeConfig
{
    internal const string DocPipeAuthenticationScheme = "Token";

    /// <summary>
    /// Gets or sets the token used to access DocPipe.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base address of the DocPipe API (incl. version and trailing slash).
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the timeout used for DocPipe calls.
    /// Use <c>null</c> for no timeout.
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Gets or sets the DocPipe client.
    /// </summary>
    public string Client { get; set; } = "abraxas";

    /// <summary>
    /// Gets or sets the DocPipe instance.
    /// </summary>
    public string Instance { get; set; } = "prod";
}
