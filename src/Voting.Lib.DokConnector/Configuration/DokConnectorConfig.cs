// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.DokConnector.Configuration;

/// <summary>
/// The configuration for the DOK connector.
/// </summary>
public class DokConnectorConfig
{
    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    public Uri Endpoint { get; set; } = new("http://localhost:3000");
}
