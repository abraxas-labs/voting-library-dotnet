// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Voting.Lib.Messaging.Configuration;

/// <summary>
/// Configuration for RabbitMQ.
/// </summary>
public class RabbitMqConfig
{
    /// <summary>
    /// Gets or sets the RabbitMQ host name.
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the hosts.
    /// </summary>
    public List<string> Hosts { get; set; } = new();

    /// <summary>
    /// Gets or sets the virtual host.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string Username { get; set; } = "user";

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = "password";

    /// <summary>
    /// Gets or sets the retry intervals.
    /// </summary>
    public List<TimeSpan> RetryIntervals { get; set; } = new() { TimeSpan.FromMilliseconds(10) };
}
