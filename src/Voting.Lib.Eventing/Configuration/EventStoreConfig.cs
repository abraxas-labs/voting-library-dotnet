// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using EventStore.Client;

namespace Voting.Lib.Eventing.Configuration;

/// <summary>
/// Configuration for the EventStore.
/// </summary>
public class EventStoreConfig
{
    /// <summary>
    /// Gets the Connection string to be used to connect to the event store for gRPC connections.
    /// i.e.
    /// <ul>
    ///     <li>ES-single instance: 'esdb://admin:changeit@single-instance-hostname:2113?tls=true'</li>
    ///     <li>ES-cluster with DNS discovery: 'esdb+discover://admin:changeit@cluster-dns-hostname:2113?tls=true'</li>
    ///     <li>ES-cluster with gossip seeds: 'esdb://admin:changeit@node-1-hostname:2113,node-2-hostname:2113,node-3-hostname:2113?tls=true'</li>
    /// </ul>
    /// </summary>
    public virtual string ConnectionString
        => $"{Scheme}://{Username}:{Password}@{string.Join(',', Authorities)}?tls={UseTls}";

    /// <summary>
    /// Gets or sets the Authorities used in the gRPC connection string.
    /// An authority must include the host and port.
    /// </summary>
    public HashSet<string> Authorities { get; set; } = new();

    /// <summary>
    /// Gets or sets the username to be used for all connections.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password to be used for all connections.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets the scheme to be used to connect to the eventstore.
    /// </summary>
    public string Scheme
    {
        get
        {
            return UseClusterDiscoveryViaDns ? "esdb+discover" : "esdb";
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the connection uses cluster discovery via DNS or not.
    /// </summary>
    public bool UseClusterDiscoveryViaDns { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the connection uses tls.
    /// </summary>
    public bool UseTls { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of the http client which is used for the event store connections.
    /// </summary>
    public string HttpClientName { get; set; } = "EventStore";

    /// <summary>
    /// Gets or sets the interval, in which the latest event position is fetched.
    /// This is only used for monitoring / metrics purposes.
    /// </summary>
    public TimeSpan LatestEventPositionMonitorInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the timeout of EventStore operations.
    /// <c>null</c> means no timeout.
    /// </summary>
    public TimeSpan? OperationTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets maximum event processing failure count per subscription scope, after which the event processing is deemed unsuccessful.
    /// This is used for monitoring purposes (health checks).
    /// </summary>
    public uint MaxEventProcessingFailureCount { get; set; } = 10;

    /// <summary>
    /// Hook to customize the built <see cref="EventStoreClientSettings"/>.
    /// </summary>
    /// <param name="eventStoreSettings">The <see cref="EventStoreClientSettings"/> to customize.</param>
    public virtual void Customize(EventStoreClientSettings eventStoreSettings)
    {
    }
}
