// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.ObjectStorage.Config;

namespace Voting.Lib.ObjectStorage.DependencyInjection;

/// <summary>
/// A helper class to build object storage clients.
/// </summary>
public class ObjectStorageClientBuilder
{
    private readonly ObjectStorageConfig _config;

    internal ObjectStorageClientBuilder(IServiceCollection services, ObjectStorageConfig config)
    {
        _config = config;
        Services = services;
        AddCore();
    }

    /// <summary>
    /// Gets the service collection instance.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Adds a new bucket client implementation and ensures it's bucket exists during app startup.
    /// </summary>
    /// <typeparam name="T">The type of the client.</typeparam>
    /// <returns>The current instance.</returns>
    public ObjectStorageClientBuilder AddBucketClient<T>()
        where T : class, IBucketObjectStorageClient
    {
        Services.TryAddSingleton<T>();
        Services.TryAddEnumerable(ServiceDescriptor.Singleton<IBucketObjectStorageClient, T>(sp => sp.GetRequiredService<T>()));
        return this;
    }

    private void AddCore()
    {
        Services.AddLogging();
        Services.AddHostedService<BucketInitializerService>();
        Services.TryAddSingleton<IObjectStorageClient, ObjectStorageClient>();
        Services.TryAddSingleton(_config);
    }
}
