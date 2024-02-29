// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Voting.Lib.ObjectStorage;

/// <summary>
/// Ensures the buckets of all <see cref="IBucketObjectStorageClient"/> exists.
/// </summary>
public class BucketInitializerService : IHostedService
{
    private readonly IEnumerable<IBucketObjectStorageClient> _bucketClients;
    private readonly ILogger<BucketInitializerService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BucketInitializerService"/> class.
    /// </summary>
    /// <param name="bucketClients">The registered bucket clients.</param>
    /// <param name="logger">The logger.</param>
    public BucketInitializerService(IEnumerable<IBucketObjectStorageClient> bucketClients, ILogger<BucketInitializerService> logger)
    {
        _bucketClients = bucketClients;
        _logger = logger;
    }

    /// <summary>
    /// Starts this service and ensures that all buckets exist.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var client in _bucketClients)
        {
            try
            {
                await client.EnsureBucketExists(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "could not initialize bucket");
            }
        }
    }

    /// <summary>
    /// Stops this service (a no-op).
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
