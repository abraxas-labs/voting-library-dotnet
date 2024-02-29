// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.ObjectStorage.Config;
using Voting.Lib.ObjectStorage.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service collection extensions for object storage.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the core services to access an object storage (S3).
    /// Additional bucket clients can be added via the returned builder.
    /// </summary>
    /// <param name="services">The service collection instance.</param>
    /// <param name="config">The configuration.</param>
    /// <returns>The object storage client builder.</returns>
    public static ObjectStorageClientBuilder AddVotingLibObjectStorage(this IServiceCollection services, ObjectStorageConfig config)
        => new(services, config);
}
